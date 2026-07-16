using Microsoft.AspNetCore.Mvc;
using Cargo_Management_Project.Models;
using Cargo_Management_Project.Data;
// Matches the outer namespace from your ICustomsService.cs file
using Cargo_Management_Project.Services;
// Matches the inner nested namespace from your ICustomsService.cs file
using Cargo_Management_Project.Services.CargoManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Cargo_Management_Project.Controllers
{
    public class CustomsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICustomsService _customsService;

        // Dependency Injection mapping exactly to your AppDbContext and nested interface
        public CustomsController(AppDbContext context, ICustomsService customsService)
        {
            _context = context;
            _customsService = customsService;
        }

        // 1. GET: Customs/SubmitCustomsDeclaration
        [HttpGet]
        public IActionResult SubmitCustomsDeclaration()
        {
            return View();
        }

        // POST: Customs/SubmitCustomsDeclaration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitCustomsDeclaration(CustomsDeclaration declaration)
        {
            if (ModelState.IsValid)
            {
                if (_customsService.ValidateDocumentation(declaration))
                {
                    // Matches your exact DbSet name: CustomsDeclarations
                    _context.CustomsDeclarations.Add(declaration);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(GetClearanceStatus), new { id = declaration.DeclarationId });
                }
                ModelState.AddModelError("", "Customs documentation validation failed.");
            }
            return View(declaration);
        }

        // 2. POST: Customs/CalculateDuty
        [HttpPost]
        public IActionResult CalculateDuty(decimal declaredValue, string hsCode)
        {
            if (string.IsNullOrEmpty(hsCode))
            {
                return BadRequest("Invalid HS Code.");
            }

            decimal duty = _customsService.CalculateTariffRate(declaredValue, hsCode);
            return Json(new { calculatedDuty = duty });
        }

        // 3. GET: Customs/UploadCustomsDocument
        [HttpGet]
        public IActionResult UploadCustomsDocument()
        {
            return View();
        }

        // POST: Customs/UploadCustomsDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCustomsDocument(IFormFile customsFile, int declarationId)
        {
            if (customsFile != null && customsFile.Length > 0)
            {
                // File processing placeholder logic
                ViewBag.Message = "Document uploaded successfully!";
                return View();
            }

            ViewBag.Error = "Please select a valid file to upload.";
            return View();
        }

        // 4. GET: Customs/GetClearanceStatus/{id}
        [HttpGet]
        public async Task<IActionResult> GetClearanceStatus(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            // Mapped to look up integer IDs within the CustomsDeclarations table
            var declaration = await _context.CustomsDeclarations.FindAsync(id);
            if (declaration == null)
            {
                return NotFound();
            }

            return View(declaration);
        }
    }
}