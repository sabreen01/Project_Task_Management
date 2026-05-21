using Core.Application.Features.Projects.Commands;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Moq;

namespace Tests.Features.Projects;

public class DeleteProjectCommandHandlerTests
{
    private readonly Mock<IRepository<Project>> _mockProjectRepo;
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        _mockProjectRepo = new Mock<IRepository<Project>>();
        _handler = new DeleteProjectCommandHandler(_mockProjectRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectExists()
    {
        // Arrange - المشروع موجود
        var projectId = Guid.NewGuid();
        var project = new Project { Id = projectId, Name = "Test Project" };

        _mockProjectRepo.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var command = new DeleteProjectCommand(projectId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert - نتأكد إن المسح نجح
        Assert.True(result.IsSuccess);
        Assert.Equal("Project deleted successfully.", result.Message);

        // نتأكد إن Delete و SaveChangesAsync اتنادوا
        _mockProjectRepo.Verify(r => r.Delete(project), Times.Once);
        _mockProjectRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProjectNotFound()
    {
        // Arrange - المشروع مش موجود
        var projectId = Guid.NewGuid();

        _mockProjectRepo.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        var command = new DeleteProjectCommand(projectId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert - نتأكد إن الـ Handler رجع Failure
        Assert.False(result.IsSuccess);
        Assert.Equal("Project not found.", result.Message);

        // نتأكد إن Delete مش اتنادت
        _mockProjectRepo.Verify(r => r.Delete(It.IsAny<Project>()), Times.Never);
    }
}
