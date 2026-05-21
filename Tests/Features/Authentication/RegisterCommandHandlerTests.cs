using System.Linq.Expressions;
using Core.Application.Features.Authentication.Commands;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Moq;

namespace Tests.Features.Authentication;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IRepository<User>> _mockUserRepo;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _mockUserRepo = new Mock<IRepository<User>>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _handler = new RegisterCommandHandler(_mockUserRepo.Object, _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenEmailIsNew()
    {
        // Arrange - الإيميل مش موجود (نرجع null)
        _mockUserRepo.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        _mockPasswordHasher.Setup(h => h.Hash(It.IsAny<string>()))
            .Returns("hashed_password");

        var command = new RegisterCommand("Mohamed", "Ali", "mohamed@test.com", "password123");

        // Act - نشغل الـ Handler
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert - نتأكد إن النتيجة Success
        Assert.True(result.IsSuccess);
        Assert.Equal("User registered successfully.", result.Message);

        // نتأكد إن AddAsync و SaveChangesAsync اتنادوا مرة واحدة
        _mockUserRepo.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUserRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        // Arrange - الإيميل موجود (نرجع يوزر)
        _mockUserRepo.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Email = "mohamed@test.com" });

        var command = new RegisterCommand("Mohamed", "Ali", "mohamed@test.com", "password123");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert - نتأكد إن النتيجة Failure
        Assert.False(result.IsSuccess);
        Assert.Equal("Email is already registered.", result.Message);

        // نتأكد إن AddAsync مش اتنادت أصلاً (مرجعناش حتى للخطوة دي)
        _mockUserRepo.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
