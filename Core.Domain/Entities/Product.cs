namespace Core.Domain.Entities;

public sealed class Product : Entity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private Product(Guid id, string name, decimal price) 
        : base(id)
    {
        Name = name;
        Price = price;
    }

    private Product()
    {
    }

    public static Product Create(string name, decimal price)
    {
        return new Product(Guid.NewGuid(), name, price);
    }
    
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            throw new ArgumentException("Price cannot be negative", nameof(newPrice));
        }
        
        Price = newPrice;
    }
}
