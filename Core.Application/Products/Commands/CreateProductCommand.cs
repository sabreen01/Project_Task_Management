using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Products.Commands;

public record CreateProductCommand(string Name, decimal Price) : IRequest<Result<Guid>>;
