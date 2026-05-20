using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Products.Queries;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<Product>>
{
    private readonly IRepository<Product> _productRepository;

    public GetProductQueryHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (product is null)
        {
            return Result.Failure<Product>(new Error("Product.NotFound", $"The product with Id {request.Id} was not found."));
        }
        
        return Result.Success(product);
    }
}
