 

namespace ProductService.Contracts;

public record ProductCreatedEvent(int Id, string Name, string Description, decimal Price);
