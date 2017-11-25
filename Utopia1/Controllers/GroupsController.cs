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
    /// <summary>
    /// Returns all groups, or returns a group by ID.
    /// This is for displaying the groups in the UI.
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GroupsController : ApiController
    {
        Repository m_repository;

        public GroupsController()
        {
            m_repository = new Repository();
        }

        [HttpGet]
        [Route("api/groups")]
        public IEnumerable<Group> GetGroups()
        {
            return m_repository.GetGroups();
        }

        [HttpGet]
        [Route("api/groups")]
        public Group GetGroupById(int groupId)
        {
            return m_repository.GetGroupById(groupId);
        }
    }
}
