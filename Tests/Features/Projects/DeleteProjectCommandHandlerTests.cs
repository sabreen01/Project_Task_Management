using Core.Application.Features.Projects.Commands;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Moq;

namespace Tests.Features.Projects;

public class DeleteProjectCommandHandlerTests
{
    private readonly Mock<IRepository<Project>> _mockProjectRepo;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        _mockProjectRepo = new Mock<IRepository<Project>>();
        _mockCacheService = new Mock<ICacheService>();
        _handler = new DeleteProjectCommandHandler(_mockProjectRepo.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectExists()
    {
        var projectId = Guid.NewGuid();
        var project = new Project { Id = projectId, Name = "Test Project" };

        _mockProjectRepo.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var command = new DeleteProjectCommand(projectId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Project deleted successfully.", result.Message);

        _mockProjectRepo.Verify(r => r.Delete(project), Times.Once);
        _mockProjectRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProjectNotFound()
    {
        var projectId = Guid.NewGuid();

        _mockProjectRepo.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        var command = new DeleteProjectCommand(projectId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Project not found.", result.Message);

        _mockProjectRepo.Verify(r => r.Delete(It.IsAny<Project>()), Times.Never);
    }
}
