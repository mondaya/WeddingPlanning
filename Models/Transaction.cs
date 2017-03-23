using System;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models {

    public class Transaction : BaseEntity {
        
        [Key]
        public int id {get; set;}

        [Display(Name = "Description")]       
        [RequiredAttribute(ErrorMessage = "Description is required")]
        [MinLengthAttribute(8)]
        public string Description {get; set;}
        
        [Display(Name = "Balance")]      
        [RequiredAttribute(ErrorMessage = "Balance is required")]       
        public int Balance {get; set;}

        [Display(Name = "Amount")]      
        [RequiredAttribute(ErrorMessage = "Amount is required")] 
        public int Amount {get; set;}

        [Display(Name = "Transaction Date")]      
        [RequiredAttribute(ErrorMessage = "Transaction Date")] 
        public DateTime CreatedAt {get; set;}

        public int UserId { get; set; }
        public User User { get; set; }

        public Transaction(){
            CreatedAt = DateTime.Now;
        }

       



    }
}