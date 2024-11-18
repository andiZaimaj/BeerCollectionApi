using BeerCollectionApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class BeersController : ControllerBase
{
    private readonly BeerDbContext _context;

    public BeersController(BeerDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Beer>> CreateBeer(Beer beer)
    {
        if (_context.Beers == null)
        {
            return Problem("'BeerDbContext.Beers' is null.");
        }

        if (beer.Ratings != null && beer.Ratings.Any(rating => rating > 5 || rating < 1))
        {
            return Problem("One or more ratings are outside the valid range (1–5).");
        }

        _context.Beers.Add(beer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBeer), new { id = beer.Id }, beer);
    }


    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetAllBeers()
    {
        if (_context.Beers == null)
        {
            return NotFound("No beers found.");
        }

        var beers = await _context.Beers.Select(b => new { b.Id, b.Name, b.Type, b.AverageRating }).ToListAsync();
        return Ok(beers);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetBeer(int id)
    {
        var beer = await _context.Beers.FirstOrDefaultAsync(b => b.Id == id);
        if (beer == null) return NotFound();
        return Ok(beer);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchBeer(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Query cannot be empty.");
        }

        var beers = await _context.Beers.Where(b => b.Name.Contains(query)).ToListAsync();
        return Ok(beers);
    }


    [HttpPost("{id}")]
    public async Task<IActionResult> UpdateRating(int id, double rating)
    {
        if (rating < 1 || rating > 5)
        {
            return BadRequest("Rating must be between 1 and 5.");
        }

        var beer = await _context.Beers.FindAsync(id);
        if (beer == null) return NotFound();

        beer.Ratings.Add(rating);
        await _context.SaveChangesAsync();
        return Ok(new { beer.Id, beer.Name, beer.Type, beer.AverageRating });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBeer(int id)
    {
        var beer = await _context.Beers.FindAsync(id);
        if (beer == null)
        {
            return NotFound($"Beer with ID {id} not found.");
        }

        _context.Beers.Remove(beer);
        await _context.SaveChangesAsync();
        return Ok($"Beer with ID {id} has been deleted.");
    }
}
