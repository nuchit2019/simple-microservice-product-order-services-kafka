namespace OrderService.Contracts;

public record ProductCreatedEvent(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock = 0,
    DateTime? CreatedAt = null,
    DateTime? UpdatedAt = null
);