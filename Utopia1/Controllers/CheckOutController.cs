using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utopia1.Models;

namespace Utopia1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CheckOutController : ApiController
    {
        private Repository m_repository;

        public CheckOutController()
        {
            m_repository = new Repository();
        }

        [HttpGet]
        [Route("api/checkout")]
        public void CheckOut(string roomId) {
            // assume we cannot share group and single check ins

            m_repository.CheckOut(roomId);
        }
    }
}
