using Microsoft.AspNetCore.Mvc;
using Bacnkend.Data;
using Bacnkend.Models;

namespace Bacnkend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly BankingContext _context;

        public ClientController(BankingContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Clients.ToList());
        }
    }
}
