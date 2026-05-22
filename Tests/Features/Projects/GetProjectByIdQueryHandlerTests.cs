using AutoMapper;
using Core.Application.Features.Projects.DTOs;
using Core.Application.Features.Projects.Queries;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests.Features.Projects;

public class GetProjectByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Project>> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProjectByIdQueryHandler _handler;

    public GetProjectByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IRepository<Project>>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetProjectByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ProjectExists_ReturnsSuccessWithDto()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Project { Id = projectId, Name = "Test", Description = "Test" };
        var projectDto = new ProjectDto { Id = projectId, Name = "Test", Description = "Test" };
        var query = new GetProjectByIdQuery(projectId);

        _repositoryMock.Setup(repo => repo.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _mapperMock.Setup(m => m.Map<ProjectDto>(It.IsAny<Project>()))
            .Returns(projectDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(projectDto);
    }

    [Fact]
    public async Task Handle_ProjectDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var query = new GetProjectByIdQuery(projectId);

        _repositoryMock.Setup(repo => repo.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project)null!);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Data.Should().BeNull();
    }
}
