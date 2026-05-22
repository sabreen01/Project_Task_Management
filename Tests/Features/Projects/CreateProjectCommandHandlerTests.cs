using Core.Application.Features.Projects.Commands;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests.Features.Projects;

public class CreateProjectCommandHandlerTests
{
    private readonly Mock<IRepository<Project>> _repositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _repositoryMock = new Mock<IRepository<Project>>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new CreateProjectCommandHandler(_repositoryMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessAndProjectId()
    {
        // Arrange
        var command = new CreateProjectCommand("New Project", "Project Description");
        
        _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Callback<Project, CancellationToken>((p, c) => p.Id = Guid.NewGuid())
            .Returns(Task.CompletedTask);
        
        _repositoryMock.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeEmpty();

        _repositoryMock.Verify(repo => repo.AddAsync(It.Is<Project>(p => p.Name == command.Name && p.Description == command.Description), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
