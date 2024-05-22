using MediatR;
using Test.Core.Logger;
using Microsoft.Extensions.Options;
using Test.DAL.Repository;

namespace Test.Application.Handlers.UpdateCandidate
{
    internal class UpdateCandidateRequestHandler : IRequestHandler<UpdateCandidateRequest, AddOrUpdateCandidateResponse>
    {
        private readonly ILogger _logger;
        private readonly IEntityRepository _repository;
        private readonly Configuration.Configuration _configuration;

        public UpdateCandidateRequestHandler(ILogger logger, IEntityRepository repository, IOptions<Configuration.Configuration> options)
        {
            _logger = logger;
            _repository = repository;
            _configuration = options.Value;
        }

        public async Task<AddOrUpdateCandidateResponse> Handle(UpdateCandidateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Candidate == null)
                {
                    _logger.LogInformation($"Candidate is not updated. Candidate value is null");
                    return new AddOrUpdateCandidateResponse { IsSuccess = false };
                }
                await _repository.UpdateCandidateAsync(request.Candidate);
                return new AddOrUpdateCandidateResponse { IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                return new AddOrUpdateCandidateResponse { IsSuccess = false };
            }
        }
    }
}
