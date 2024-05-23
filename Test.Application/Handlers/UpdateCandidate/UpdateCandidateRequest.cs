using MediatR;
using Test.Core.Models;

namespace Test.Application.Handlers.UpdateCandidate
{
    public class UpdateCandidateRequest : IRequest<AddOrUpdateCandidateResponse>
    {
        public Candidate Candidate { get; set; }

        public UpdateCandidateRequest(Candidate candidate)
        {
            Candidate = candidate;
        }
    }
}
