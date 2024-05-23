using MediatR;
using Moq;
using Test.Application.Handlers;
using Test.Application.Handlers.AddCandidate;
using Test.Application.Handlers.GetCandidate;
using Test.Application.Handlers.UpdateCandidate;
using Test.Core.Models;
using Test.WebService.Controllers;

namespace Test.Tests
{
    public class CandidateControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CandidateController _controller;
        public CandidateControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new CandidateController(_mediatorMock.Object);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_ExistingCandidate_UpdatesCandidate()
        {
            // Arrange
            var candidate = new Candidate { Email = "test@example.com" };
            var existingCandidateResponse = new GetCandidateResponse { Candidate = new Candidate() };
            var updateResponse = new AddOrUpdateCandidateResponse();

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetCandidateRequest>(r => r.EmailAddress == candidate.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCandidateResponse);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateCandidateRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateResponse);

            // Act
            var result = await _controller.AddOrUpdateCandidate(candidate);

            // Assert
            Assert.Equal(updateResponse, result);
            _mediatorMock.Verify(m => m.Send(It.Is<UpdateCandidateRequest>(r => r.Candidate == candidate), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AddCandidateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_NewCandidate_AddsCandidate()
        {
            // Arrange
            var candidate = new Candidate { Email = "new@example.com" };
            var newCandidateResponse = new GetCandidateResponse { Candidate = null };
            var addResponse = new AddOrUpdateCandidateResponse();

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetCandidateRequest>(r => r.EmailAddress == candidate.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newCandidateResponse);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AddCandidateRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(addResponse);

            // Act
            var result = await _controller.AddOrUpdateCandidate(candidate);

            // Assert
            Assert.Equal(addResponse, result);
            _mediatorMock.Verify(m => m.Send(It.Is<AddCandidateRequest>(r => r.Candidate == candidate), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateCandidateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}