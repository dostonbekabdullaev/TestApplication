using Microsoft.EntityFrameworkCore;
using Test.DAL.Models;

namespace Test.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Candidate> Candidates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}