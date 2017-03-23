using Microsoft.EntityFrameworkCore;
using System.Linq;
using WeddingPlanner.Models;

namespace WeddingPlanner
{

    public class WeddingPlannerContext : DbContext{
        
        // base() calls the parent class' constructor passing the "options" parameter along
        public WeddingPlannerContext(DbContextOptions<WeddingPlannerContext> options):base(options){

        }

        // This DbSet contains "Person" objects and is called "Users"
        public DbSet<User> Users { get; set; }   
        public DbSet<Transaction> Transactions { get; set; }  
        public DbSet<Event> Events { get; set; }  
        public DbSet<Rsvp> Rsvps { get; set; } 

        public int getBalance(int userId){
             Transaction userTransactionDb = this.Transactions
                .OrderByDescending(key=>key.CreatedAt)
                .FirstOrDefault(record=>record.UserId == userId); 
                return userTransactionDb != null ? userTransactionDb.Balance : 0;
        }   
    }
}