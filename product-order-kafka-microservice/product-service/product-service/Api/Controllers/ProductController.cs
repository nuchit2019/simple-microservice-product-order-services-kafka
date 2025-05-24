using Microsoft.AspNetCore.Mvc;
using ProductService.Domain.Entities;
using ProductService.Application.Interfaces;

namespace ProductService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(Product product) => Ok(await productService.CreateAsync(product));

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await productService.GetAllAsync());


}
