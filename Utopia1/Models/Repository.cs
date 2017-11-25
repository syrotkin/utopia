using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Utopia1.Models
{
    public class Repository
    {
        public IEnumerable<Group> GetGroups()
        {
            using (var context = new UtopiaContext())
            {
                return context.Groups.ToList();
            }
        }

        internal Group GetGroupById(int groupId)
        {
            using (var context = new UtopiaContext())
            {
                return context.Groups.SingleOrDefault(g => g.GroupId == groupId);
            }

        }

        public Room CheckInSingle(byte fullDay)
        {
            // TOOD: fullDay is not used at the moment

            using (var context = new UtopiaContext())
            {
                var room = context.Database.SqlQuery<Room>("select * from rooms where location = (select min(location) from rooms where occupancy < capacity and RoomId not in (select RoomId from GroupRoomAssignment))").FirstOrDefault();
                if (room != null) {
                    context.Database.ExecuteSqlCommand("update Rooms set occupancy = occupancy + 1 where roomId = @roomId", new SqlParameter("@roomId", room.RoomId));
                }

                return room;
            }
        }

        internal void CheckOut(string roomId)
        {
            // assume group and single checkins cannot share rooms // easier this way. 

            using (var context = new UtopiaContext())
            {
                DecrementOccupancy(context, roomId);

                var nullableGroupId = context.Database.SqlQuery<int?>("select GroupId from GroupRoomAssignment where RoomId = @roomId", new SqlParameter("@roomId", roomId)).FirstOrDefault();
                if (nullableGroupId != null) {
                    // check occupancy
                    var occupancy = context.Database.SqlQuery<int>("select occupancy from Rooms where roomId = @roomId", new SqlParameter("@roomId", roomId)).FirstOrDefault();
                    if (occupancy == 0) {
                        // this was the last person from the group that checked out
                        context.Database.ExecuteSqlCommand("delete from GroupRoomAssignment where GroupId = @groupId", new SqlParameter("@groupId", nullableGroupId.Value));
                    }
                }
            }
        }
       
        internal Room CheckInGroup(byte fullDay, int groupId)
        {
            // TODO: fullDay is not used at the moment
            
            using (var context = new UtopiaContext()) {
                // find out if there is a room already

                var roomId = context.Database.SqlQuery<string>("select RoomId from GroupRoomAssignment where GroupId = @groupId", new SqlParameter("@groupId", groupId)).FirstOrDefault();

                if (roomId != null) {
                    // already assigned
                    IncrementOccupancy(context, roomId);
                    var assignedRoom = context.Rooms.Where(r => r.RoomId == roomId).FirstOrDefault();
                    return assignedRoom;
                }

                // room has not been assigned yet

                var newRoom = context.Database.SqlQuery<Room>(@"select * from rooms where location = 
                                                                (select min(location) from Rooms where occupancy = 0 and capacity   
                                                                        >= (select NumberOfPeople from Groups where GroupId = @groupId))", new SqlParameter("@groupId", groupId)).FirstOrDefault();
                if (newRoom != null)
                {
                    context.Database.ExecuteSqlCommand("insert into GroupRoomAssignment (GroupId, RoomId) values (@groupId, @roomId)", new SqlParameter("@roomId", newRoom.RoomId), new SqlParameter("@groupId", groupId));
                    IncrementOccupancy(context, newRoom.RoomId);
                }

                return newRoom;
            }
        }

        private static void IncrementOccupancy(UtopiaContext context, string roomId)
        {
            context.Database.ExecuteSqlCommand(@"update Rooms
                                        set occupancy =
                                        case occupancy
                                        when capacity then capacity
                                        else occupancy + 1
                                        end
                                        where RoomId = @roomId", new SqlParameter("@roomId", roomId));
        }

        private void DecrementOccupancy(UtopiaContext context, string roomId)
        {
            context.Database.ExecuteSqlCommand(@"update Rooms 
                                        set occupancy = 
	                                        case occupancy
	                                        when 0 then 0 
	                                        else occupancy - 1
	                                        end 
                                        where RoomId = @roomId", new SqlParameter("@roomId", roomId));
        }
    }
}