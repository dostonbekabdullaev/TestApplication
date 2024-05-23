using Moq;
using Test.Application.Handlers.GetCandidate;
using Test.Core.Logger;
using Test.Core.Models;
using Test.DAL.Repository;

namespace Test.Tests
{
    public class GetCandidateRequestHandlerTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IEntityRepository> _repository;
        private readonly GetCandidateRequestHandler _handler;

        public GetCandidateRequestHandlerTests()
        {
            _logger = new();
            _repository = new();
            _handler = new GetCandidateRequestHandler(_logger.Object, _repository.Object);
        }

        [Fact]
        public async Task Handle_ReturnsGetCandidateResponse_WhenCandidateIsFound()
        {
            // Arrange
            var email = "test@example.com";
            var candidate = new Candidate { Email = email };
            var request = new GetCandidateRequest(email);

            _repository
                .Setup(repo => repo.GetCandidateAsync(request.EmailAddress))
                .ReturnsAsync(candidate);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(candidate, result.Candidate);
        }

        [Fact]
        public async Task Handle_Rethrows_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new GetCandidateRequest("test@example.com");
            var errorMessage = "Test exception";
            var exception = new Exception(errorMessage);

            _repository
                .Setup(repo => repo.GetCandidateAsync(request.EmailAddress))
                .ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(request, CancellationToken.None));
            Assert.Equal(exception, ex);
            Assert.Equal(errorMessage, ex.Message);
        }
    }
}
