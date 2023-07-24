using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;
using Caculation.GraphLib;

namespace DiaMakerModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class TableKeys
    //----------------------------------------------------------------------------------------------------------------------
    public class TableKeys
    {
        private Dictionary<string, Table> tables = new Dictionary<string, Table>();
        private List<ForeignKey> foreignKeys = new List<ForeignKey>();
        //----------------------------------------------------------------------------------------------------------------------
        public TableKeys()
        {
            tables = new Dictionary<string, Table>();
            foreignKeys = new List<ForeignKey>();
        }
        //----------------------------------------------------------------------------------------------------------------------
        public Dictionary<string, Table> Tables
        {
            get
            {
                return tables;
            }
        }
        public List<ForeignKey> ForeignKeys
        {
            get
            {
                return foreignKeys;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void AddTable(string pName, string pSchemaName)
        {
            var table = new Table() { Name = pName, Scheme = pSchemaName };
            tables.Add(table.FullName, table);
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void AddForeignKey( string pName, string pTableNameFrom, string pTableNameTo, string pKeyFrom, string pKeyTo)
        {
            if (tables.ContainsKey(pTableNameFrom) && tables.ContainsKey(pTableNameTo))
            {
                Table tableFrom = tables[pTableNameFrom];
                Table tableTo = tables[pTableNameTo];
                ForeignKey key = new ForeignKey()
                {
                    Name = pName,
                    TableFrom = tableFrom,
                    TableTo = tableTo,
                    KeyFrom = pKeyFrom,
                    KeyTo = pKeyTo
                };
                foreignKeys.Add(key);
                tableFrom.ForeignKeysFrom.Add(key);
                tableTo.ForeignKeysTo.Add(key);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public IGraph<IVertex> CreateGraph(bool pIsAll = false)
        {
            Dictionary<string, int> tableNumbers = new Dictionary<string, int>();
            IGraph<IVertex> lGraph = new Graph<IVertex>();
            int i = 0;
            foreach (var pair in Tables)
            {
                if (pair.Value.IsSelected || pIsAll)
                {
                    IVertex v = new CVertex(pair.Key);
                    lGraph.Vertices.Add(v);
                    tableNumbers.Add(pair.Key, i);
                    i++;
                }
            }
            foreach (var r in ForeignKeys)
            {
                if (tableNumbers.ContainsKey(r.TableFrom.FullName) && tableNumbers.ContainsKey(r.TableTo.FullName))
                    lGraph.AddEdge(tableNumbers[r.TableFrom.FullName], tableNumbers[r.TableTo.FullName]);

            }
            return lGraph;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
