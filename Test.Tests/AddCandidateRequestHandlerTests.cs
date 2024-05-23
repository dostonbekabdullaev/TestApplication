using Moq;
using Test.Application.Handlers.AddCandidate;
using Test.Application.Handlers;
using Test.Core.Models;
using Test.DAL.Repository;
using Test.Core.Logger;

namespace Test.Tests
{
    public class AddCandidateRequestHandlerTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IEntityRepository> _repository;
        private readonly AddCandidateRequestHandler _handler;

        public AddCandidateRequestHandlerTests()
        {
            _logger = new();
            _repository = new();
            _handler = new AddCandidateRequestHandler(_logger.Object, _repository.Object);
        }

        [Fact]
        public async Task Handle_ReturnsFalseAndError_WhenCandidateIsNull()
        {
            // Arrange
            var request = new AddCandidateRequest(null);
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
            var request = new AddCandidateRequest(new Candidate { Email = "test@example.com" });

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
            var request = new AddCandidateRequest(new Candidate { Email = "test@example.com" });
            var errorMessage = "Test exception";
            var exception = new Exception(errorMessage);

            _repository
                .Setup(x => x.AddCandidateAsync(It.IsAny<Candidate>()))
                .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
        }
    }
}
