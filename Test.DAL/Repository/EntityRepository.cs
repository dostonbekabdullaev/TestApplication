using Test.DAL.Data;
using Test.DAL.Models;

namespace Test.DAL.Repository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDbContext _context;

        public EntityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCandidateAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task<Candidate?> GetCandidateAsync(string emailAddress)
        {
            return await _context.Candidates.FindAsync(emailAddress);
        }

        public async Task UpdateCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync();
        }
    }
}
