namespace OrderService.Domain.Entities;

    public record Product(
        int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock = 0,
    DateTime? CreatedAt = null,
    DateTime? UpdatedAt = null
        );