using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Test.Cache.CacheService;
using Test.Core.Configuration;
using Test.Core.Logger;
using Test.DAL.Data;
using Test.Core.Models;

namespace Test.DAL.Repository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IRedisCacheService _cache;
        private readonly Configuration _configuration;

        public EntityRepository(ApplicationDbContext context, ILogger logger, IRedisCacheService cache, IOptions<Configuration> options)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
            _configuration = options.Value;
        }

        public async Task AddCandidateAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();
            
            var message = $"A candidate with the email {candidate.Email} has been successfully ADDED";
            _logger.LogInformation(message);

            if (_configuration.IsRedisEnabled)
            {
                await _cache.SetValueAsync(candidate.Email, candidate);
                _logger.LogInformation($"{message} in the cache");
            }
        }

        public async Task DeleteCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();

            var message = $"A candidate with the email {candidate.Email} has been successfully DELETED";
            _logger.LogInformation(message);

            if (_configuration.IsRedisEnabled)
            {
                await _cache.DeleteKeyAsync(candidate.Email);
                _logger.LogInformation($"{message} from the cache");
            }
        }

        public async Task<Candidate?> GetCandidateAsync(string emailAddress)
        {
            var doesCacheValueExist = _configuration.IsRedisEnabled && await _cache.ExistsKeyAsync(emailAddress);
            if (doesCacheValueExist)
            {
                var cachedValue = await _cache.GetValueAsync<Candidate>(emailAddress);

                _logger.LogInformation($"A candidate with the email {emailAddress} has been RETRIEVED from the cache. Value is {(cachedValue == null ? "" : "not ")}null");

                return cachedValue;
            }

            var candidateResult = await _context.Candidates.AsNoTracking().FirstOrDefaultAsync(x => x.Email == emailAddress);

            // In case cache is enabled when there is data in the DB already.
            if (_configuration.IsRedisEnabled && !doesCacheValueExist && candidateResult != null)
            {
                await _cache.SetValueAsync(emailAddress, candidateResult);
            }

            _logger.LogInformation($"A candidate with the email {emailAddress} has {(candidateResult == null ? "NOT" : "")} been RETRIEVED.");
            
            return candidateResult;
        }

        public async Task UpdateCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync();

            var message = $"A candidate with the email {candidate.Email} has been successfully UPDATED";
            _logger.LogInformation(message);

            if (_configuration.IsRedisEnabled)
            {
                await _cache.UpdateValueAsync(candidate.Email, candidate);
                _logger.LogInformation($"{message} in the cache");
            }
        }
    }
}
