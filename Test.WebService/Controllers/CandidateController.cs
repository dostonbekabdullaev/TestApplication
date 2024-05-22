using MediatR;
using Microsoft.AspNetCore.Mvc;
using Test.Application.Handlers;
using Test.Application.Handlers.AddCandidate;
using Test.Application.Handlers.GetCandidate;
using Test.Application.Handlers.UpdateCandidate;
using Test.DAL.Models;

namespace Test.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CandidateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ActionName("AddOrUpdateCandidate")]
        public async Task<AddOrUpdateCandidateResponse> AddOrUpdateCandidate(Candidate candidate)
        {
            var candidateResult = await _mediator.Send(new GetCandidateRequest(candidate.Email));
            return candidateResult.Candidate != null ? 
                await _mediator.Send(new UpdateCandidateRequest(candidate)) : 
                await _mediator.Send(new AddCandidateRequest(candidate));
        }
    }
}
