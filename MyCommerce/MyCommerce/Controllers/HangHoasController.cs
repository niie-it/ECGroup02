﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCommerce.Data;

namespace MyCommerce.Controllers
{
    public class HangHoasController : Controller
    {
        private readonly MyeStoreContext _context;

        public HangHoasController(MyeStoreContext context)
        {
            _context = context;
        }

        // GET: HangHoas
        public async Task<IActionResult> Index()
        {
            var myeStoreContext = _context.HangHoas.Include(h => h.MaLoaiNavigation).Include(h => h.MaNccNavigation);
            return View(await myeStoreContext.ToListAsync());
        }

        // GET: HangHoas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HangHoas == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        // GET: HangHoas/Create

        [Authorize]
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new SelectList(_context.Loais, "MaLoai", "MaLoai");
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc");
            return View();
        }

        // POST: HangHoas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHh,TenHh,TenAlias,MaLoai,MoTaDonVi,DonGia,Hinh,NgaySx,GiamGia,SoLanXem,MoTa,MaNcc")] HangHoa hangHoa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hangHoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoai"] = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

        // GET: HangHoas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HangHoas == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
            {
                return NotFound();
            }
            ViewData["MaLoai"] = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

        // POST: HangHoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHh,TenHh,TenAlias,MaLoai,MoTaDonVi,DonGia,Hinh,NgaySx,GiamGia,SoLanXem,MoTa,MaNcc")] HangHoa hangHoa)
        {
            if (id != hangHoa.MaHh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hangHoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HangHoaExists(hangHoa.MaHh))
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
            ViewData["MaLoai"] = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

        // GET: HangHoas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HangHoas == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        // POST: HangHoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HangHoas == null)
            {
                return Problem("Entity set 'MyeStoreContext.HangHoas'  is null.");
            }
            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa != null)
            {
                _context.HangHoas.Remove(hangHoa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HangHoaExists(int id)
        {
          return (_context.HangHoas?.Any(e => e.MaHh == id)).GetValueOrDefault();
        }
    }
}
