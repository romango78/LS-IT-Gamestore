namespace Gamestore.Domain.Entities;

public class OrderEntity
{
    private OrderEntity()
    {
    }

    public OrderEntity(CustomerEntity customer, IEnumerable<BookedProductEntity> bookedProducts)
        :this()
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(bookedProducts);

        Customer = customer;

        if (!bookedProducts.Any())
        {
            throw new ArgumentException();
        }

        BookedProducts = new HashSet<BookedProductEntity>(bookedProducts);
    }

    public Guid Id { get; }

    public CustomerEntity Customer { get; }

    public IEnumerable<BookedProductEntity> BookedProducts { get; }
}