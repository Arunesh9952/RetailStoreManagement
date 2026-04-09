using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailShop.Data;
using RetailShop.DTO;

namespace RetailShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly RetailDbContext context;

        public OrderController(RetailDbContext context)
        {
            this.context = context;
        }

        [HttpPost("placeOrder")]

        public IActionResult PlaceOrder()
        {
            int userId = int.Parse(User.FindFirst("id").Value);
            var cartItems = context.Carts.Where(c=> c.UserId ==userId).ToList();
            if(cartItems.Count == 0)
            {
                return BadRequest("Cart is empty.");
            }
            decimal totalAmount = 0;
            foreach(var item in cartItems)
            {
                var product = context.Products.Find(item.ProductId);
                if(product == null || product.Stock < item.Quantity)
                {
                    return BadRequest($"Product with ID {item.ProductId} is out of stock.");
                }
                product.Stock -= item.Quantity;

                var order = new Order
                {
                    UserId = userId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = product.Price * item.Quantity,
                    OrderDate = DateTime.Now,
                    Status = "Placed"
                };

                totalAmount += order.TotalPrice;
                context.Orders.Add(order);
            }
            context.Carts.RemoveRange(cartItems);
            context.SaveChanges();
            return Ok($"Order placed successfully. Total Amount: {totalAmount}");

        }

        [HttpGet("get-orders/{userId}")]
        public IActionResult GetOrders(int userId)
        {
            var orders = context.Orders.Where(o => o.UserId == userId).Include(o => o.Product) // 🔥 load product
        .Select(o => new OrderResponseDTO
        {
            OrderId = o.Id,
            ProductName = o.Product.Name,
            Price = o.Product.Price,
            Quantity = o.Quantity,
            TotalPrice = o.TotalPrice,
            OrderDate = o.OrderDate
        })
        .ToList();
            var totalAmount = orders.Sum(o => o.TotalPrice);
            return Ok(new
            {
                Orders = orders,
                TotalAmount = totalAmount
            });
        }



        }
}
