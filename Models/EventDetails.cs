using System;
using System.Collections.Generic;



namespace WeddingPlanner.Models {

    public class EventDetails : BaseEntity {   


        public string Title {get; set;}  
        public string GroomName {get; set;}  
        public string BrideName {get; set;}  
        public List<string> GrooomList  {get; set;}  
        public List<string> BrideList    {get; set;}  
        public DateTime CreatedAt {get; set;}

         public EventDetails(){
            GrooomList = new List<string>();
            BrideList = new List<string>();
        }       



    }
}