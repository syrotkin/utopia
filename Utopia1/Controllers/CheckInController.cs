using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utopia1.Models;
using System.Web.Http.Cors;

namespace Utopia1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CheckInController : ApiController
    {
        Repository m_repository;

        public CheckInController()
        {
            m_repository = new Repository();
        }

        [HttpGet]
        [Route("api/checkin")]
        public Room DoCheckIn(string type, byte fullDay, int groupId = -1)
        {
            // type has to be "single" or "group"
            // fullDay can be 0 or 1
            // groupId -- -1 is the invalid groupId, otherwise has to be valid if type="group"

            if (Constants.Single.Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                return m_repository.CheckInSingle(fullDay);

                // find the first room (min location) where occupancy < capacity
                // update -> that room occupancy + 1;

              
            /*
            
             select * from rooms where location = (select min(location) from rooms where occupancy < capacity)

            /// if there is such room
            // update rooms set occupancy = occupancy + 1 where roomId = @roomId

            /// return room
                
              */
            }
            else if (Constants.Group.Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                // assume groups cannot share rooms
                // select roomId from GroupRoomAssignment where groupId = @groupId
                // if (roomId != null) {
                // // already assigned --> return (select * from rooms where roomId = @roomId)
                // }
                // else:
                // numberOfPeople = select NumberOfPeople from Groups where GroupId = @groupId
                // select min(location) from Rooms where occupancy = 0 and capacity >= NumberOfPeople

                /*
             
            1st step -- check assignment table

            /// assumption -- cannot share group and single in same room => hence occupancy = 0
            select * from rooms where location = 
(select min(location) from Rooms where occupancy = 0 and capacity 
	>= (select NumberOfPeople from Groups where GroupId = 1))
                

            such room available:

            update the GroupRoomAssignment table
            set RoomId = @roomId
             */

                return m_repository.CheckInGroup(fullDay, groupId);

            }

            else
            {
                throw new ArgumentException($"{type} is not a valid check-in type.");
            }

            //string allInfo  = type + " " + fullDay + " " + groupId;
            return new Room
            {
                RoomId = "G120"
            };
        }
    }
}
