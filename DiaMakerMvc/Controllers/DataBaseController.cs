using DiaMakerModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DiaMakerMvc.Controllers
{
    public class DataBaseController : ApiController
    {
        private ConnectionStringSettingsCollection connections;
        public DataBaseController()
        {
            connections = ConfigurationManager.ConnectionStrings;
        }

        public IEnumerable<DataBase> Get()
        {


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

//        protected override void HandleUnknownAction(string actionName)
//        {
//        //    this.ResponseMessage.Write(string.Format("You requested the {0} action", actionName));
//            HttpResponseMessage rm = new HttpResponseMessage();
////            rm.Content = new HttpContent();
//            this.ResponseMessage(rm);

// //           ActionContext.Response.Content = new ContentResult(string.Format("You requested the {0} action", actionName));
//        }

    }
}