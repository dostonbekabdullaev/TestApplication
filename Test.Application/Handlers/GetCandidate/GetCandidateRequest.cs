using MediatR;

namespace Test.Application.Handlers.GetCandidate
{
    public class GetCandidateRequest : IRequest<GetCandidateResponse>
    {
        public string EmailAddress { get; set; }

        public GetCandidateRequest(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
