using System;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models
{

    public class RsvpView : BaseEntity
    {


        [RequiredAttribute()]
        public string RsvpName { get; set; }

        public char RsvpSide { get; set; }

    }
}