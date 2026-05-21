using Core.Application.Features.ProjectTasks.Commands;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Moq;

namespace Tests.Features.ProjectTasks;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<IRepository<ProjectTask>> _mockTaskRepo;
    private readonly Mock<IRepository<Project>> _mockProjectRepo;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _mockTaskRepo = new Mock<IRepository<ProjectTask>>();
        _mockProjectRepo = new Mock<IRepository<Project>>();
        _handler = new CreateTaskCommandHandler(_mockTaskRepo.Object, _mockProjectRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProjectExists()
    {
        // Arrange - المشروع موجود
        var projectId = Guid.NewGuid();
        var project = new Project { Id = projectId, Name = "Test Project" };

        _mockProjectRepo.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        var command = new CreateTaskCommand(projectId, "New Task", "Task Description", DateTime.UtcNow.AddDays(7), TaskPriority.High);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert - نتأكد إن التاسك اتعملت بنجاح
        Assert.True(result.IsSuccess);
        Assert.Equal("Task created successfully within the project.", result.Message);

        // نتأكد إن AddAsync و SaveChangesAsync اتنادوا على الـ Task Repository
        _mockTaskRepo.Verify(r => r.AddAsync(It.IsAny<ProjectTask>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockTaskRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProjectNotFound()
    {
        // Arrange - المشروع مش موجود
        var projectId = Guid.NewGuid();

        _mockProjectRepo.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        var command = new CreateTaskCommand(projectId, "New Task", "Task Description", DateTime.UtcNow.AddDays(7), TaskPriority.High);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert - نتأكد إن الـ Handler رجع Failure
        Assert.False(result.IsSuccess);
        Assert.Equal("The specified project does not exist.", result.Message);

        // نتأكد إن AddAsync مش اتنادت على الـ Task Repository
        _mockTaskRepo.Verify(r => r.AddAsync(It.IsAny<ProjectTask>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
