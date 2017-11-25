using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Utopia1.Models
{
    [Table("Rooms")]
    public class Room
    {
        // TODO: maybe we want to have a GUID??? Or just for the database??
        public string RoomId { get; set; }

        public int Capacity { get; set; }

        public int Occupancy { get; set; }

        public int Location { get; set; }
    }
}