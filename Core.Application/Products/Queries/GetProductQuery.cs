using Core.Domain.Entities;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Products.Queries;

public record GetProductQuery(Guid Id) : IRequest<Result<Product>>;
