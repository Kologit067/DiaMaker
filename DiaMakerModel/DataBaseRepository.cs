using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DiaMakerModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class DataBaseRepository
    //----------------------------------------------------------------------------------------------------------------------
    public class DataBaseRepository
    {
        private static List<DataBase> dataBases;
        private static DataBaseRepository dataBaseRepository;
        private static string connectionString;
        //----------------------------------------------------------------------------------------------------------------------
        private DataBaseRepository()
        {
            dataBases = new List<DataBase>();
        }
        //----------------------------------------------------------------------------------------------------------------------
        public static DataBaseRepository CreateInstance(string pConnectionString)
        {
            connectionString = pConnectionString;
            if (dataBaseRepository == null)
            {
                dataBaseRepository = new DataBaseRepository();
                CreateВataBases();
            }

            return dataBaseRepository;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private static void CreateВataBases()
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand(@"SELECT name, database_id, create_date,*
FROM sys.databases
WHERE owner_sid <> 0x01
", con);
            try
            {
                con.Open();
                try
                {
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        string dbName = reader.GetString(0);
                        DataBase db = new DataBase { Name = dbName };
                        dataBases.Add(db);
                    }
                    reader.Close();
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        //----------------------------------------------------------------------------------------------------------------------
        public List<DataBase> DataBases
        {
            get
            {
                return dataBases;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public TableKeys GetTables(string pDatabase)
        {
            TableKeys tables = new TableKeys();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand(@"USE "  + pDatabase + @"
SELECT t.Name, s.name as schemaname
FROM sys.tables t
INNER JOIN sys.schemas s
ON t.schema_id = s.schema_id
WHERE t.Name NOT LIKE 'sys%'
", con);
            //SqlParameter parameter = new SqlParameter();
            //parameter.ParameterName = "@Database";
            //parameter.SqlDbType = SqlDbType.NVarChar;
            //parameter.Direction = ParameterDirection.Input;
            //parameter.Value = pDatabase;
            //com.Parameters.Add(parameter);


            con.Open();
            try
            {
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    string tableName = reader.GetString(0);
                    string schemaName = reader.GetString(1);
                    tables.AddTable(tableName, schemaName);
                }
                reader.Close();

                com = new SqlCommand(@"USE " + pDatabase + @"
SELECT f.name AS ForeignKey, sf.name + '.' + OBJECT_NAME(f.parent_object_id) AS TableName, 
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    st.name + '.' + OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc
ON f.OBJECT_ID = fc.constraint_object_id
INNER JOIN sys.tables tf
ON tf.object_id = f.parent_object_id
INNER JOIN sys.schemas sf
ON tf.schema_id = sf.schema_id
INNER JOIN sys.tables tt
ON tt.object_id = f.referenced_object_id
INNER JOIN sys.schemas st
ON tt.schema_id = st.schema_id", con);
                //parameter = new SqlParameter();
                //parameter.ParameterName = "@Database";
                //parameter.SqlDbType = SqlDbType.NVarChar;
                //parameter.Direction = ParameterDirection.Input;
                //parameter.Value = pDatabase;
                //com.Parameters.Add(parameter);
                reader = com.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string tableFrom = reader.GetString(1);
                    string tableTo = reader.GetString(3);
                    string keyFrom = reader.GetString(2);
                    string keyTo = reader.GetString(4);
                    if (tableFrom !=  tableTo)
                        tables.AddForeignKey(name,  tableFrom,  tableTo,  keyFrom,  keyTo);
                }
                reader.Close();
            }
            finally
            {
                con.Close();
            }

            return tables;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
