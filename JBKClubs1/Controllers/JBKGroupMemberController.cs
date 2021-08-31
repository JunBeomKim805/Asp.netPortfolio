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
{// this accesses and maintains the groupmember table
    // Junbeom Kim Sep 2020
    public class JBKGroupMemberController : Controller
    {
        private readonly ClubsContext _context;

        public JBKGroupMemberController(ClubsContext context)
        {
            _context = context;
        }

        // display all groupmember records on file
        public async Task<IActionResult> Index(Int32? artistId, string artistName)
        {
            if (artistId != null)
            {
                HttpContext.Session.SetString(nameof(artistId), artistId.ToString());
                HttpContext.Session.SetString(nameof(artistName), artistName);
            }
            else if (HttpContext.Session.GetString("ArtistId") != null)
            {
                artistId = Convert.ToInt32(HttpContext.Session.GetString("ArtistId"));
            }
            else
            {
                TempData["message"] = "please select artist before looking for groupmembers";
                return Redirect("/JBKArtist/index");
            }
            var groupMember = await _context.GroupMember.FirstOrDefaultAsync(m => m.ArtistIdGroup == artistId);
            var member = await _context.GroupMember.FirstOrDefaultAsync(m => m.ArtistIdMember == artistId);
            if (groupMember != null)
            {
                var clubsContext = _context.GroupMember.Where(a => a.ArtistIdGroup == artistId)
                    .Include(g => g.ArtistIdGroupNavigation)
                    .Include(g => g.ArtistIdMemberNavigation)
                    .Include(g => g.ArtistIdMemberNavigation.NameAddress).OrderBy(a=>a.DateLeft).ThenBy(a=>a.DateJoined);
                return View(await clubsContext.ToListAsync());

             }
             else if(member != null)
            {
                var clubsContext = _context.GroupMember.Where(a => a.ArtistIdMember== artistId)
                .Include(g => g.ArtistIdGroupNavigation)
                .Include(g => g.ArtistIdMemberNavigation)
                .Include(g => g.ArtistIdMemberNavigation.NameAddress).OrderBy(a => a.DateLeft).ThenBy(a => a.DateJoined);
                TempData["message"] = "artsist is an individual, not a group";
                return View("GroupsForArtist",clubsContext);
            }
            else
            {
                TempData["message"] = "neither a group nor a group member";
                return Redirect("/JBKGroupMember/Create");
            }



        }

        // show all properties for the selected groupmember

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMember
                .Include(g => g.ArtistIdGroupNavigation)
                .Include(g => g.ArtistIdMemberNavigation)
                .FirstOrDefaultAsync(m => m.ArtistIdGroup == id);
            if (groupMember == null)
            {
                return NotFound();
            }

            return View(groupMember);
        }

        // show an empty groupmemeber page to create new one 
        public IActionResult Create()
        {
            List<Artist> individualArtist = _context.Artist.Include(a => a.NameAddress)
                .Where(a => a.GroupMemberArtistIdGroupNavigation.Count == 0).Include
                (a => a.NameAddress).ToList();
            List<KeyValue> availableArtists = new List<KeyValue>();
            foreach (var item in individualArtist)
            {
                var member = _context.GroupMember.
                           FirstOrDefault(a => a.ArtistIdMember == item.ArtistId && a.DateLeft == null);
                //individualArtist.Remove(item);
                if (member == null)
                {
                    if(item.NameAddress.LastName!=null)
                    availableArtists.Add(new KeyValue(item.ArtistId, item.NameAddress.LastName
                      + ", " + item.NameAddress.FirstName));
                    if(item.NameAddress.LastName==null)
                        availableArtists.Add(new KeyValue(item.ArtistId,item.NameAddress.FirstName));
                }

            }

            //ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId");
            ViewData["ArtistIdMember"] = new SelectList(availableArtists, "Key", "Value");
            return View();
        }

        // add new groupmember to database if it passes the edits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistIdGroup,ArtistIdMember,DateJoined,DateLeft")] GroupMember groupMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            ViewData["ArtistIdMember"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdMember);
            return View(groupMember);
        }

        // display the selcted groupmember for unpdating
        public async Task<IActionResult> Edit(int? groupId,int? memberId)
        {
            if (groupId == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMember.FindAsync(groupId,memberId);
            if (groupMember == null)
            {
                return NotFound();
            }
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            ViewData["ArtistIdMember"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdMember);
            return View(groupMember);
        }

        // update the selected groupmember record if it passed edits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistIdGroup,ArtistIdMember,DateJoined,DateLeft")] GroupMember groupMember)
        {
            if (id != groupMember.ArtistIdGroup)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupMemberExists(groupMember.ArtistIdGroup))
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
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            ViewData["ArtistIdMember"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdMember);
            return View(groupMember);
        }

        // display the selcted groupmember to confirm delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMember
                .Include(g => g.ArtistIdGroupNavigation)
                .Include(g => g.ArtistIdMemberNavigation)
                .FirstOrDefaultAsync(m => m.ArtistIdGroup == id);
            if (groupMember == null)
            {
                return NotFound();
            }

            return View(groupMember);
        }

        // deleted confirmed ... remove groupmember from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupMember = await _context.GroupMember.FindAsync(id);
            _context.GroupMember.Remove(groupMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupMemberExists(int id)
        {
            return _context.GroupMember.Any(e => e.ArtistIdGroup == id);
        }
    }
}
