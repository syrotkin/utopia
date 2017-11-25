using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utopia1.Models;

namespace Utopia1.Controllers
{
    public class RoomsController : ApiController
    {
        private Repository m_repository;

        public RoomsController()
        {
            m_repository = new Repository();
        }

        public IEnumerable<Room> GetRooms()
        {
            return m_repository.GetRooms();
        }
    }
}
