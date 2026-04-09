using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailShop.Data;
using RetailShop.DTO;

namespace RetailShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly RetailDbContext context;
        public ProductController(RetailDbContext context)
        {
            this.context = context;
        }

        [HttpGet("get-products")]

        public IActionResult GetProducts()
        {
            var product = context.Products
    .Include(p => p.Category)
    .Select(p => new ProductResponseDTO
    {
        Name = p.Name,
        Price = p.Price,
        Stock = p.Stock,
        CategoryName = p.Category.CategoryName
    })
    .FirstOrDefault();
            return Ok(product);
        }
        [HttpGet("get-product/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = context.Products
    .Include(p => p.Category)
    .Where(p => p.Id == id)
    .Select(p => new ProductResponseDTO
    {
        Name = p.Name,
        Price = p.Price,
        Stock = p.Stock,
        CategoryName = p.Category.CategoryName
    })
    .FirstOrDefault();
            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }

        [HttpPost("add-product")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct([FromBody] ProductDTO product)
        {
            var pr = new Product
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };
            context.Products.Add(pr);
            context.SaveChanges();
            return Ok("Product added successfully.");
        }

            [HttpPut("update-product/{id}")]
            [Authorize(Roles = "Admin")]
            public IActionResult UpdateProduct(int id, [FromBody] ProductDTO product)
            {
                var existingProduct = context.Products.FirstOrDefault(p => p.Id == id);
                if (existingProduct == null)
                    return NotFound("Product not found.");
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.CategoryId = product.CategoryId;
                context.SaveChanges();
                return Ok("Product updated successfully.");
        }

        [HttpDelete("delete-product/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound("Product not found.");
            context.Products.Remove(product);
            context.SaveChanges();
            return Ok("Product deleted successfully.");
        }


        }
}
