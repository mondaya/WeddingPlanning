using System;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models
{

    public class Rsvp : BaseEntity
    {

        [Key]
        public int id { get; set; }

        public char Side { get; set; }
        public bool ShowList { get; set; }

        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public Rsvp()
        {
            CreatedAt = DateTime.Now;
        }





    }
}