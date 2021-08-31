using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JBKClubs1.Models;
using Microsoft.AspNetCore.Http;

namespace JBKClubs1.Controllers
{    // this accesses and maintains the Province table
    // Junbeom Kim Sep 2020
    public class JBKProvinceController : Controller
    {
        private readonly ClubsContext _context;

        public JBKProvinceController(ClubsContext context)
        {
            _context = context;
        }

        // display all provinces records on file
        public async Task<IActionResult> Index(string countryCode,string CountryName)
        {
            if(countryCode != null)
            {
                HttpContext.Session.SetString("CountryCode", countryCode);
                HttpContext.Session.SetString(nameof(CountryName), CountryName);
            }
            else if(HttpContext.Session.GetString("CountryCode")!=null)
            {
                countryCode = HttpContext.Session.GetString("CountryCode");

            }
            else
            {
                TempData["message"] = "please select countryCode before looking for provinces";
                return Redirect("/JBKCountry/index");
            }       
            var clubsContext = _context.Province.Where(a => a.CountryCode==countryCode).Include(p => p.CountryCodeNavigation).OrderBy(a =>a.Name);
            return View(await clubsContext.ToListAsync());
        }

        // show all properties for the selected province
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // show an empty province page to create new one 
        public IActionResult Create()
        {
            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode");
            return View();
        }

        // add new province to database if it passes the edits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProvinceCode,Name,CountryCode,SalesTaxCode,SalesTax,IncludesFederalTax,FirstPostalLetter")] Province province)
        {
            //check if creating a duplicate key
            if (ProvinceExists(province.ProvinceCode))
                ModelState.AddModelError("ProvinceCode", "Province code is already on file");

            // check if creating a duplicate name
            var existingRecord = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .FirstOrDefaultAsync(m => m.Name == province.Name);

            if(existingRecord != null)
                ModelState.AddModelError("Name", "Province name is already on file");

            if (ModelState.IsValid)
            {
                _context.Add(province);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);
            return View(province);
        }

        // display the selcted province for unpdating
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }
            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);
            return View(province);
        }

        // update the selected province record if it passed edits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProvinceCode,Name,CountryCode,SalesTaxCode,SalesTax,IncludesFederalTax,FirstPostalLetter")] Province province)
        {
            var existingRecord = await _context.Province
            .Include(p => p.CountryCodeNavigation)
            .FirstOrDefaultAsync(m => m.Name == province.Name);

            if (existingRecord.ProvinceCode != province.ProvinceCode)
                ModelState.AddModelError("Name", "Province name is already on file");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(province);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvinceExists(province.ProvinceCode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);
            return View(province);
        }

        // display the selcted province to confirm delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // deleted confirmed ... remove province from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var province = await _context.Province.FindAsync(id);
            _context.Province.Remove(province);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvinceExists(string id)
        {
            return _context.Province.Any(e => e.ProvinceCode == id);
        }
    }
}
