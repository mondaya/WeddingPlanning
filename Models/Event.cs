using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models {

    public class Event : BaseEntity {
        
        [Key]
        public int id {get; set;} 

        [RequiredAttribute()]
        public string CreatorName {get; set;}           
       

        [RequiredAttribute()]
        public string PartnerName {get; set;}  

        [RequiredAttribute()]
        public char CreatorType {get; set;}    

        [RequiredAttribute()]
        public char PartnerType {get; set;}  
        
        [DateRange()]        
        public DateTime CreatedAt {get; set;}

        public int UserId { get; set; }
        public User User { get; set; }

        
        public List<Rsvp> Rsvp { get; set; }

        public Event(){
            //CreatedAt = DateTime.Now;
            Rsvp = new List<Rsvp>();
        }
        

    public class DateRangeAttribute : ValidationAttribute
    {
        private DateTime maxDate;

       public DateRangeAttribute()
        {
            maxDate = DateTime.Now;
        }

       protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime inputDate = (DateTime)value;
                if (inputDate > maxDate)
                {
                    return new ValidationResult("Enter Valid Date");
                }
            
            }

           return ValidationResult.Success;
        }
    }
       



    }
}