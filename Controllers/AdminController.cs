using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using technoland.Models;
using Technoland.Data;

namespace Technoland.Controllers
{
    //Solo los usuarios en la base de datos que tengan el rol AspNetRoles 1, que es Administrator, pueden acceder.
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        //Contexto de la base de datos
        private readonly ApplicationDbContext _context;
        //Contexto de la ruta actual
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Smartphones.ToListAsync());
        }


        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Description,Price,ImageUrl")] Smartphones smartphones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(smartphones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(smartphones);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Smartphones == null)
            {
                return NotFound();
            }

            var smartphones = await _context.Smartphones.FindAsync(id);
            if (smartphones == null)
            {
                return NotFound();
            }
            return View(smartphones);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Description,Price,ImageUrl")] Smartphones smartphones)
        {
            if (id != smartphones.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(smartphones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SmartphonesExists(smartphones.Id))
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
            return View(smartphones);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Smartphones == null)
            {
                return NotFound();
            }

            var smartphones = await _context.Smartphones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (smartphones == null)
            {
                return NotFound();
            }

            return View(smartphones);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Smartphones == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Smartphones'  is null.");
            }
            var smartphones = await _context.Smartphones.FindAsync(id);
            if (smartphones != null)
            {
                var smartphone = _context.Smartphones.AsNoTracking().FirstOrDefault(p => p.Id == smartphones.Id);
                string webRootPath = _webHostEnvironment.WebRootPath;
                string currentPath = webRootPath + WC.ImagePath;

                var imagenAnterior = Path.Combine(currentPath, smartphone.ImageUrl);
                if (System.IO.File.Exists(imagenAnterior))
                {
                    System.IO.File.Delete(imagenAnterior);
                }
                _context.Smartphones.Remove(smartphones);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SmartphonesExists(int id)
        {
            return _context.Smartphones.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Smartphones smartphones)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (smartphones.Id == 0)
                {
                    //Se crea un nombre para el archivo donde se guardara en la ruta de la carpeta products
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    //Se guarda la imagen dentro de la carpeta products del servidor
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    //se guarda solamente la ruta de la imagen en la base de datos
                    smartphones.ImageUrl = fileName + extension;
                    _context.Smartphones.Add(smartphones);
                }
                else
                {
                    var smartphone = _context.Smartphones.AsNoTracking().FirstOrDefault(p => p.Id == smartphones.Id);
                    if (files.Count > 0)//Si se carga nueva imagen, se reemplaza
                    {
                        string currentPath = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //Borra la imagen anterior
                        var previousImage = Path.Combine(currentPath, smartphone.ImageUrl);
                        if (System.IO.File.Exists(previousImage))
                        {
                            System.IO.File.Delete(previousImage);
                        }

                        //Se crea la imagen en la ruta
                        using (var fileStream = new FileStream(Path.Combine(currentPath, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        smartphones.ImageUrl = fileName + extension;
                    }//Si no se carga otra imagen, se deja la misma.
                    else
                    {
                        smartphones.ImageUrl = smartphone?.ImageUrl;
                    }
                    _context.Smartphones.Update(smartphones);
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(smartphones);
        }
    }
}
