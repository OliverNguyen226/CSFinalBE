using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSFinal.Models;
using Microsoft.AspNetCore.SignalR;
using CSFinal.Hubs;

namespace CSFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaintingsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IHubContext<PaintHub> _hub;
        public PaintingsController(DatabaseContext context, IHubContext<PaintHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        // GET: api/Paintings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Painting>>> GetPaintings()
        {
            if (_context.Paintings == null)
            {
                return NotFound();
            }
            return await _context.Paintings.ToListAsync();
        }

        // GET: api/Paintings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Painting>> GetPainting(int id)
        {
            if (_context.Paintings == null)
            {
                return NotFound();
            }
            var painting = await _context.Paintings.FindAsync(id);

            if (painting == null)
            {
                return NotFound();
            }

            return painting;
        }

        // PUT: api/Paintings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPainting(int id, Painting painting)
        {
            Console.WriteLine("A painting has been edited");
            if (id != painting.Id)
            {
                return BadRequest();
            }

            _context.Entry(painting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaintingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await _hub.Clients.Group(painting.Id.ToString()).SendAsync("PaintingUpdated", painting);
            return NoContent();
        }

        // POST: api/Paintings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Painting>> PostPainting(Painting painting)
        {
            Console.WriteLine("A painting has been created");
            if (_context.Paintings == null)
            {
                return Problem("Entity set 'DatabaseContext.Paintings'  is null.");
            }
            _context.Paintings.Add(painting);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("PaintingCreated", painting);
            return CreatedAtAction("GetPainting", new { id = painting.Id }, painting);
        }

        // DELETE: api/Paintings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePainting(int id)
        {
            if (_context.Paintings == null)
            {
                return NotFound();
            }
            var painting = await _context.Paintings.FindAsync(id);
            if (painting == null)
            {
                return NotFound();
            }

            _context.Paintings.Remove(painting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaintingExists(int id)
        {
            return (_context.Paintings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
