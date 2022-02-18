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
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
        {
           return (await _context.Categories.ToListAsync()).Select(c=> new CategoryModel(c.Id, c.Name)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryEntity(int id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            return new CategoryModel(categoryEntity.Id, categoryEntity.Name);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryEntity(int id, CategoryModel categoryModel)
        {
            if (id != categoryModel.Id)
            {
                return BadRequest();
            }

            var categoryEntity = await _context.Categories.FindAsync(id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            categoryEntity.Id = categoryModel.Id;
            categoryEntity.Name = categoryModel.Name;
            _context.Entry(categoryEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryEntityExists(id))
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
        public async Task<ActionResult<CategoryEntity>> PostCategoryEntity(CreateCategoryModel categoryModel)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == categoryModel.Name);
            if (category != null)
                return new ConflictObjectResult(new CategoryModel(category.Id, category.Name));

            var categoryEntity = new CategoryEntity(categoryModel.Name);

            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryEntity", new { id = categoryEntity.Id }, new CategoryModel(categoryEntity.Id, categoryEntity.Name));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryEntity(int id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);
            if (categoryEntity == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categoryEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryEntityExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
