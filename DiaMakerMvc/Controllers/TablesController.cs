using Common.CommonLib.Interfaces;
using DiaMakerModel;
using DiaMakerModel.Tracing;
using DiaMakerMvc.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DiaMakerMvc.Controllers
{
    public class TablesController : ApiController
    {
        private string connectionString = "";
        private int maxDurability = 20;
        public TablesController()
        {
            ConnectionStringSettingsCollection connections;
            connections = ConfigurationManager.ConnectionStrings;

            if (connections.Count != 0)
            {

                // Get the collection elements.
                foreach (ConnectionStringSettings connection in connections)
                {
                    if (connection.Name == "LRA")
                        connectionString = connection.ConnectionString;
                }
            }
        }

        public IEnumerable<TableDto> Get(string id)
        {

            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(connectionString);

            var tableKeys = dataBaseRepository.GetTables(id);

            List<TableDto> result = new List<TableDto>(tableKeys.Tables.Count);
            foreach (var t in tableKeys.Tables.Values)
                result.Add(new TableDto(t));
            
            var fullGraph = tableKeys.CreateGraph(true);
            GraphLib.GraphSplitter graphSplitter = new GraphLib.GraphSplitter(fullGraph);
            var subGraphList = graphSplitter.CreateConnectedSubGraphes();
            Table.CreateTableColorSet(subGraphList.Count);

            int lGroup = 0;
            foreach (IGraph<IVertex> sg in subGraphList.OrderByDescending(g => g.Vertices.Count))
            {
                List<TableDto> foundTables = result.Where(t => sg.Vertices.Any(v => v.Name == t.Name)).ToList();
                for (int i = 0; i < foundTables.Count; i++)
                    foundTables[i].Group = lGroup;
                lGroup++;
            }

            result = result.OrderBy(t => t.Group).ToList();

            return result;
        }

        private double startOriginePanelHeight;
        private double startOriginePanelWidth;
        private double OriginePanelHeight = 450;
        private double OriginePanelWidth = 800;
        private double minimalMultiplicationHeightDiaPanel;
        private double minimalMultiplicationWidthDiaPanel;
        private double MultiplicationHeightDiaPanel = 1;
        private double MultiplicationWidthDiaPanel = 1;

        public GraphDto Post(string id, IList<TableDto> tables)
        {
//            return "";
            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(connectionString);

            var tableKeys = dataBaseRepository.GetTables(id);
            tableKeys.Tables.Values.AsParallel().Where(tk => tables.Any(t => t.IsSelected && t.Name == tk.Name)).ForAll(t => t.IsSelected = true);
            //tableKeys.Tables.Values.AsParallel().AsOrdered().Zip(tables.AsParallel().AsOrdered(), (tf, ts) => 
            //    new { tFirst = tf, tSecond = ts }).ForAll(p => p.tFirst.IsSelected = p.tSecond.IsSelected);
            
            IGraph<IVertex> lGraph = tableKeys.CreateGraph();

            startOriginePanelWidth = OriginePanelWidth;
            startOriginePanelHeight = OriginePanelHeight;

            PresentationData presentationData = new PresentationData(OriginePanelWidth, OriginePanelHeight, lGraph);
            minimalMultiplicationHeightDiaPanel = presentationData.MinimalMultiplicationHeightDiaPanel;
            minimalMultiplicationWidthDiaPanel = presentationData.MinimalMultiplicationWidthDiaPanel;
            if (MultiplicationHeightDiaPanel - 0.00001 < minimalMultiplicationHeightDiaPanel)
                MultiplicationHeightDiaPanel = presentationData.MinimalMultiplicationHeightDiaPanel;
            if (MultiplicationWidthDiaPanel - 0.00001 < minimalMultiplicationWidthDiaPanel)
                MultiplicationWidthDiaPanel = presentationData.MinimalMultiplicationWidthDiaPanel;
            Tracing tracing = new Tracing(lGraph, presentationData, "Enumerate");
            
            tracing.Execute(maxDurability * 1000);
//            ChangeOptimalNumber = tracing.ChangeOptimalNumber;

            tracing.CreateRectangleAndLine();

             GraphDto graphDto = CreateDiaElements(tracing);

             return graphDto;
        }

        //----------------------------------------------------------------------------------------------------------------------
        private  GraphDto  CreateDiaElements(Tracing tracing)
        {
            GraphDto graphDto = new GraphDto() { Rectangles = new List<RectangleInfoDto>(),
                                                 ConnectLines = new List<ConnectLineInfoDto>(),
                                                 Arrows = new List<ArrowInfoDto>()
            }; 
            if (tracing.SectionMatrix != null)
            {

                foreach (var v in tracing.SectionMatrix.OfType<SectionVertex>().Where(v => !v.IsEmptyPlace))
                    graphDto.Rectangles.Add(new RectangleInfoDto(v));
                foreach (var l in tracing.ConnectingLines)
                    graphDto.ConnectLines.Add(new ConnectLineInfoDto(l));
                foreach (var a in tracing.ArrowDatas)
                    graphDto.Arrows.Add(new ArrowInfoDto(a));

            }
            return graphDto;
        }
    }
}
