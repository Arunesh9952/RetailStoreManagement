using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailShop.Data;
using RetailShop.Models;

namespace RetailShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly RetailDbContext context;

        public CategoryController(RetailDbContext context)
        {
            this.context = context;
        }

            [HttpGet("get-categories")]
            public IActionResult GetCategories()
            {
                var categories = context.Categories.ToList();
                return Ok(categories);
        }
                
                [HttpPost("add-category")]
        [Authorize(Roles = "Admin")]


                public IActionResult AddCategory([FromBody] Category name)
                {
                    var category = new Category
                    {
                        CategoryName = name.CategoryName
                    };
                    context.Categories.Add(category);
                    context.SaveChanges();
                    return Ok("Category added successfully.");
        }


    }
}
