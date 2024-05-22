using MediatR;
using Test.DAL.Models;

namespace Test.Application.Handlers.AddCandidate
{
    public class AddCandidateRequest : IRequest<AddOrUpdateCandidateResponse>
    {
        public Candidate? Candidate { get; set; }

        public AddCandidateRequest(Candidate? candidate)
        {
            Candidate = candidate;
        }
    }
}
