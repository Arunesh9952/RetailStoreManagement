using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailShop.Data;
using RetailShop.DTO;

namespace RetailShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly RetailDbContext context;

        public CartController(RetailDbContext context)
        {
            this.context = context;
        }
            [HttpGet("get-cart")]
            public IActionResult GetCart()
            {
            int userId = int.Parse(User.FindFirst("id").Value);

            var cart = context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                 .Select(c => new CartResponse
                 {
                     CartId = c.Id,
                     ProductName = c.Product.Name,
                     Price = c.Product.Price,
                     Quantity = c.Quantity
                 }).ToList();
            return Ok(cart);
        }
        [HttpPost("add-to-cart")]
        public IActionResult AddToCart( int productId, int quantity)
        {
            int userId = int.Parse(User.FindFirst("id").Value);
            var cartItem = new Cart
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };
            context.Carts.Add(cartItem);
            context.SaveChanges();
            return Ok("Product added to cart.");
        }

        [HttpDelete("remove-from-cart/{cartItemId}")]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = context.Carts.FirstOrDefault(c => c.Id == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }
            context.Carts.Remove(cartItem);
            context.SaveChanges();
            return Ok("Product removed from cart.");
        }

        [HttpPut("update-cart/{cartItemId}")]
        public IActionResult UpdateCart(int cartItemId, int quantity)
        {
            var cartItem = context.Carts.FirstOrDefault(c => c.Id == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }
            cartItem.Quantity = quantity;
            context.SaveChanges();
            return Ok("Cart updated successfully.");
        }

        }
}
