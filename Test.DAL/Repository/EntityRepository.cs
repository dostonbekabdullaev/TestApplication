using Test.Core.Logger;
using Test.DAL.Data;
using Test.DAL.Models;

namespace Test.DAL.Repository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public EntityRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddCandidateAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"A candidate with the email {candidate.Email} has been successfully ADDED.");
        }

        public async Task DeleteCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"A candidate with the email {candidate.Email} has been successfully DELETED.");
        }

        public async Task<Candidate?> GetCandidateAsync(string emailAddress)
        {
            var candidateResult = await _context.Candidates.FindAsync(emailAddress);
            _logger.LogInformation($"A candidate with the email {emailAddress} has {(candidateResult == null ? "NOT" : "")} been RETRIEVED.");
            return candidateResult;
        }

        public async Task UpdateCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"A candidate with the email {candidate.Email} has been successfully UPDATED.");
        }
    }
}
