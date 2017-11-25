using System.ComponentModel.DataAnnotations.Schema;

namespace Utopia1.Models
{
    [Table("Groups")]
    public class Group
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int NumberOfPeople { get; set; }

        /// <summary>
        /// Actually gets the <cref name="Room"/> ID
        /// </summary>
        //public string RoomId { get; set; }
    }
}