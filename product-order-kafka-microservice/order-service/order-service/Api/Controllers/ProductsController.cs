using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IOrderProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await productService.GetAllProductsAsync());
}
