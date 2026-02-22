using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MVC_Project__advance_web_.Models;
namespace MVC_Project__advance_web_.Data
   
{
    public class AppDbContext : DbContext
    {
        //INHERIT DB CONTEXT FOR DATABASE 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        
        }
        
        //GENERATE USER TABLE
        public DbSet<Users> Users => Set<Users>(); 
    }
}
