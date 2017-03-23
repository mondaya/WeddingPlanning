using System;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models {

    public class EventView : BaseEntity {
        
        [Key]
        public int id {get; set;}
             
        [RequiredAttribute()]
        public string Title {get; set;} 
        public int Count {get; set;} 

        public string Action {get; set;}      
        
        public DateTime CreatedAt {get; set;}


       



    }
}