using MediatR;
using Test.Core.Logger;
using Microsoft.Extensions.Options;
using Test.DAL.Repository;
using Test.Core.Configuration;

namespace Test.Application.Handlers.AddCandidate
{
    internal class AddCandidateRequestHandler : IRequestHandler<AddCandidateRequest, AddOrUpdateCandidateResponse>
    {
        private readonly ILogger _logger;
        private readonly IEntityRepository _repository;
        
        public AddCandidateRequestHandler(ILogger logger, IEntityRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<AddOrUpdateCandidateResponse> Handle(AddCandidateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Candidate == null)
                {
                    var message = $"Candidate is not added. Candidate value is null";
                    _logger.LogInformation(message);
                    return new AddOrUpdateCandidateResponse { IsSuccess = false, Message = message };
                }
                await _repository.AddCandidateAsync(request.Candidate);
                return new AddOrUpdateCandidateResponse { IsSuccess = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                return new AddOrUpdateCandidateResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
