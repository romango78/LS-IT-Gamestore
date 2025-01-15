namespace Gamestore.Domain.Entities;

public class OrderEntity
{

    public string Id { get; }

    public CustomerEntity Customer { get; }

    public IEnumerable<BookedProductEntity> BookedProducts { get; }

    public OrderEntity(string id, CustomerEntity customer, IEnumerable<BookedProductEntity> bookedProducts)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(bookedProducts);

        Id = id;
        Customer = customer;

        if (!bookedProducts.Any())
        {
            throw new ArgumentException();
        }

        BookedProducts = new HashSet<BookedProductEntity>(bookedProducts);
    }

}