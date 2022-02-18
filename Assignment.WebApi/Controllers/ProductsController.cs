using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment.WebApi;
using Assignment.WebApi.Models;
using Assignment.WebApi.Models.Entities;

namespace Assignment.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts()
        {
            return (await _context.Products.Include(p=> p.Category).ToListAsync())
                .Select(p => new ProductModel(p.Id, p.Name, p.Description, p.Price, p.Category.Name))
                .ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProductEntity(int id)
        {
            var productEntity = await _context.Products.Include(p=> p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            return new ProductModel(productEntity.Id, productEntity.Name, productEntity.Description, productEntity.Price, productEntity.Category.Name);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductEntity(int id, UpdateProductModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var productEntity = await _context.Products.FindAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(model.CategoryId);
            if (category == null)
            {
                return BadRequest();
            }

            productEntity.Name = model.Name;
            productEntity.Description = model.Description;
            productEntity.Price = model.Price;
            productEntity.CategoryId = model.CategoryId;

            _context.Entry(productEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> PostProductEntity(CreateProductModel model)
        {
            var productEntity = new ProductEntity(model.Name, model.Description, model.Price, model.CategoryId);
            var category = await _context.Categories.FindAsync(model.CategoryId);
            if (category == null)
                return BadRequest();

            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductEntity", new { id = productEntity.Id }, 
                new ProductModel(productEntity.Id, productEntity.Name, productEntity.Description, productEntity.Price, category.Name));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductEntity(int id)
        {
            var productEntity = await _context.Products.FindAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            _context.Products.Remove(productEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductEntityExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
