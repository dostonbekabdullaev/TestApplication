using Moq;
using Test.DAL.Repository;
using Test.Application.Handlers.UpdateCandidate;
using Test.Core.Logger;
using Test.Application.Handlers;
using Test.Core.Models;

namespace Test.Tests
{
    public class UpdateCandidateRequestHandlerTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IEntityRepository> _repository;
        private readonly UpdateCandidateRequestHandler _handler;

        public UpdateCandidateRequestHandlerTests()
        {
            _logger = new();
            _repository = new();
            _handler = new UpdateCandidateRequestHandler(_logger.Object, _repository.Object);
        }

        [Fact]
        public async Task Handle_ReturnsFalseAndError_WhenCandidateIsNull()
        {
            // Arrange
            var request = new UpdateCandidateRequest(null);
            var expectedResponse = new AddOrUpdateCandidateResponse { IsSuccess = false };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResponse.IsSuccess, result.IsSuccess);
            Assert.True(!string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public async Task Handle_ReturnsTrueAndEmptyError_WhenCandidateIsNotNull()
        {
            // Arrange
            var request = new UpdateCandidateRequest(new Candidate { Email = "test@example.com" });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public async Task Handle_ReturnsFalseAndError_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new UpdateCandidateRequest(new Candidate { Email = "test@example.com" });
            var errorMessage = "Test exception";
            var exception = new Exception(errorMessage);

            _repository
                .Setup(x => x.UpdateCandidateAsync(It.IsAny<Candidate>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
        }
    }
}
