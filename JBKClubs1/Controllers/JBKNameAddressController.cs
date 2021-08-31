using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JBKClubs1.Models;

namespace JBKClubs1.Controllers
{// this accesses and maintains the nameaddress table
    // Junbeom Kim Sep 2020
    public class JBKNameAddressController : Controller
    {
        private readonly ClubsContext _context;

        public JBKNameAddressController(ClubsContext context)
        {
            _context = context;
        }

        // display all nameaddress records on file
        public async Task<IActionResult> Index()
        {

            var clubsContext = await _context.NameAddress.Include(n => n.ProvinceCodeNavigation).ToListAsync();
            foreach (var item in clubsContext)
            {
                if (item.FirstName != null && !string.IsNullOrEmpty(item.LastName))
                    item.FirstName= item.LastName + ", " + item.FirstName;
                if (item.FirstName != null && string.IsNullOrEmpty(item.LastName))
                    item.FirstName = item.FirstName;
                if (item.FirstName == null && !string.IsNullOrEmpty(item.LastName))
                    item.FirstName = item.LastName;
                if (item.FirstName == null &&   string.IsNullOrEmpty(item.LastName))
                    item.FirstName = "";
            }

            return View(clubsContext.OrderBy(n=>n.FirstName));

        }

        // show all properties for the selected nameaddress
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nameAddress = await _context.NameAddress
                .Include(n => n.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.NameAddressId == id);
            if (nameAddress == null)
            {
                return NotFound();
            }

            return View(nameAddress);
        }

        // show an empty nameaddress page to create new one 
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode");
            return View();
        }

        // add new nameaddress to database if it passes the edits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameAddressId,FirstName,LastName,CompanyName,StreetAddress,City,PostalCode,ProvinceCode,Email,Phone")] NameAddress nameAddress)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(nameAddress);
                    await _context.SaveChangesAsync();
                    TempData["message"] = $"record added";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", nameAddress.ProvinceCode);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                ModelState.AddModelError("", "exception thrown on create : " + ex.GetBaseException().Message);
            }
            Create();
            return View(nameAddress);
        }

        // display the selcted nameaddress for unpdating
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nameAddress = await _context.NameAddress.FindAsync(id);
            if (nameAddress == null)
            {
                return NotFound();
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province.OrderBy(n=>n.Name), "ProvinceCode", "Name", nameAddress.ProvinceCode);
            return View(nameAddress);
        }

        // update the selected nameaddress record if it passed edits

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NameAddressId,FirstName,LastName,CompanyName,StreetAddress,City,PostalCode,ProvinceCode,Email,Phone")] NameAddress nameAddress)
        {
            if (id != nameAddress.NameAddressId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nameAddress);
                    TempData["message"] = "record updated";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NameAddressExists(nameAddress.NameAddressId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                    catch (Exception ex)
                    {

                        while (ex.InnerException != null) ex = ex.InnerException;
                        ModelState.AddModelError("", "exception thrown on update : " + ex.GetBaseException().Message);
                    }
                    return RedirectToAction(nameof(Index));
            }

            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", nameAddress.ProvinceCode);
            return View(nameAddress);
        }

        // display the selcted nameaddress to confirm delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nameAddress = await _context.NameAddress
                .Include(n => n.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.NameAddressId == id);
            if (nameAddress == null)
            {
                return NotFound();
            }

            return View(nameAddress);
        }

        // deleted confirmed ... remove nameaddress from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var nameAddress = await _context.NameAddress.FindAsync(id);
                _context.NameAddress.Remove(nameAddress);
                await _context.SaveChangesAsync();
                TempData["message"] = "record deleted";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["message"] = $"exception on delete " + ex.GetBaseException().Message;
                throw;
            }

        }

        private bool NameAddressExists(int id)
        {
            return _context.NameAddress.Any(e => e.NameAddressId == id);
        }
        //public async Task<string> FullName(Int32 id)
        //{
        //    string fullName = "";
        //    var nameAddress = await _context.NameAddress
        //        .Include(n => n.ProvinceCodeNavigation)
        //        .FirstOrDefaultAsync(m => m.NameAddressId == id);
        //    if(nameAddress.FirstName!=null&&nameAddress.LastName!=null)
        //        fullName = nameAddress.LastName +", "+nameAddress.FirstName;
        //    if (nameAddress.FirstName != null && nameAddress.LastName == null)
        //        fullName = nameAddress.FirstName;
        //    if (nameAddress.FirstName == null && nameAddress.LastName != null)
        //        fullName = nameAddress.LastName;
        //    if(nameAddress.FirstName == null && nameAddress.LastName == null)
        //        fullName = "";

        //    return fullName;
        //}
    }
}
