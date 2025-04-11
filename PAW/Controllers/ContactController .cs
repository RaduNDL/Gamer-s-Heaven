using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;

namespace PAW.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _context.Contacts.OrderByDescending(f => f.Date).ToListAsync();
            return View(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Email,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Date = DateTime.Now;
                _context.Add(contact);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

       
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("FeedbackID,Name,Email,Message")] Contact contact)
        {
            if (id != contact.FeedbackID) return NotFound();

            if (ModelState.IsValid)
            {
                var original = await _context.Contacts.FindAsync(id);
                if (original == null) return NotFound();

                original.Name = contact.Name;
                original.Email = contact.Email;
                original.Message = contact.Message;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
