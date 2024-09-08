using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IndustryConnect_Week5_WebApi.Models;
using IndustryConnect_Week5_WebApi.Dtos;

namespace IndustryConnect_Week5_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IndustryConnectWeek2Context _context;

        public SaleController(IndustryConnectWeek2Context context)
        {
            _context = context;
        }

        // GET: api/Sale
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales()
        {
            var sales = _context.Sales
                 .Include(s => s.Customer)
                 .Include(s => s.Product)
                 .Include(s => s.Store)
                 .Select(s => new SaleDto
                 {
                     //Id = s.Id,
                     StoreName = s.Store.Name,
                     CustomerName = s.Customer != null ? $"{s.Customer.FirstName} {s.Customer.LastName}" : "Unknown Customer",
                     ProductName = s.Product.Name
                    
                 }).ToList();

            if (sales.Count == 0)
            {
                return NotFound("No sales available.");
            }

            return Ok(sales);


        }

        // GET: api/Sale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetSale(int id)
        {

            var sale = await _context.Sales
                .Include(p => p.Product)
                .Include(c => c.Customer)
                .Include(s => s.Store)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
            {
                return NotFound();
            }

            // Map Sale to SaleDto
            var saleDto = new SaleDto
            {
                //Id = sale.Id,
                StoreName = sale.Store != null ? sale.Store.Name : "Unknown Store",
                CustomerName = sale.Customer != null ? $"{sale.Customer.FirstName} {sale.Customer.LastName}" : "Unknown Customer",
                ProductName = sale.Product != null ? sale.Product.Name : "Unknown Product",
                
            };

            return Ok(saleDto);
        }

        // PUT: api/Sale/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }

            _context.Entry(sale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
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

        // POST: api/Sale
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSale", new { id = sale.Id }, sale);
        }

        // DELETE: api/Sale/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
