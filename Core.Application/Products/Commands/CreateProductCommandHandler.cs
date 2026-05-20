using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IRepository<Product> _productRepository;

    public CreateProductCommandHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(request.Name, request.Price);
        
        await _productRepository.AddAsync(product, cancellationToken);
        
        return Result.Success(product.Id);
    }
}
