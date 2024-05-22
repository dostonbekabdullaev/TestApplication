using MediatR;

using Microsoft.Extensions.Options;
using Test.Core.Logger;
using Test.DAL.Repository;

namespace Test.Application.Handlers.GetCandidate
{
    public class GetCandidateRequestHandler : IRequestHandler<GetCandidateRequest, GetCandidateResponse>
    {
        private readonly ILogger _logger;
        private readonly IEntityRepository _repository;
        private readonly Configuration.Configuration _configuration;
        
        public GetCandidateRequestHandler(ILogger logger, IEntityRepository repository, IOptions<Configuration.Configuration> options)
        {
            _logger = logger;
            _repository = repository;
            _configuration = options.Value;
        }

        public async Task<GetCandidateResponse> Handle(GetCandidateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var candidate = await _repository.GetCandidateAsync(request.EmailAddress);
                return new GetCandidateResponse { Candidate = candidate };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                return new GetCandidateResponse { Candidate = null };
            }
        }
    }
}
