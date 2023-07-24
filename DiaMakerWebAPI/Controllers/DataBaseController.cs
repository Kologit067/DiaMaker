using DiaMakerModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DiaMakerWebAPI.Controllers
{
    public class DataBaseController : ApiController
    {
        public IEnumerable<DataBase> Get()
        {
            /*
            ConnectionStringsSection cs = (ConnectionStringsSection)ConfigurationManager.GetSection("connectionStrings");
            if (cs == null)
                throw new ProviderException(Resources.An_error_occurred_retrieving_the_connection_strings_section);
            if (cs.ConnectionStrings[_connectionString] == null)
                throw new ProviderException(Resources.The_connection_string_could_not_be_found);
            else
                _connectionString = cs.ConnectionStrings[_connectionString].ConnectionString;
            */

            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            string connectionString = "";

            if (connections.Count != 0)
            {


                //      ConnectionStringSettings connection = connections
                // Get the collection elements.
                foreach (ConnectionStringSettings connection in connections)
                {
                    if (connection.Name == "LRA")
                        connectionString = connection.ConnectionString;


                }
            }


            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(connectionString);
            List<DataBase> dataBases = dataBaseRepository.DataBases.ToList();
            return dataBases;
        }
    }
}
