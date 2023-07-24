using DiaMakerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DiaMakerWebAPI.Controllers
{
    public class TableController : ApiController
    {
        // GET: api/Table
        public IEnumerable<TableDto> Get()
        {
            return new List<TableDto>() { new TableDto { Name = "Product" }, new TableDto { Name = "Client" } };
        }

        // GET: api/Table/5
        public TableDto Get(int id)
        {
            return null;
        }

        // POST: api/Table
        public void Post([FromBody]TableDto value)
        {
        }

        // PUT: api/Table/5
        public void Put(int id, [FromBody]TableDto value)
        {
        }

        // DELETE: api/Table/5
        public void Delete(int id)
        {
        }
    }
}
