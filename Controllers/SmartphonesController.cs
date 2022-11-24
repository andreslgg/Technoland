using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using technoland.Models;
using Technoland.Data;
using Technoland.Models;

namespace Technoland.Controllers
{
    public class SmartphonesController : Controller
    {
        private readonly ApplicationDbContext _context;


        public SmartphonesController(ApplicationDbContext context)
        {
            _context = context;
        }

      

        public List<Smartphones> GetSmartphones(List<int> ids)
        {
         
                return _context.Smartphones.Where(x => ids.Contains(x.Id)).ToList();
            
        }

        public async Task<IActionResult> addToCart(int? id)
            {

                var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(7),  //La cookie del carrito se guarda por una semana
                };
                var carritoJson = Request.Cookies["carrito"]; //se guarda la cookie del carrito que obtiene un json
                List<int> listaIDs=new List<int>();
                if (carritoJson != null)   //cuando exista la cookie, se convierte el json en un arreglo
                {
                    listaIDs = new List<int>(carritoJson.FromJson<int[]>()); 
                }

                listaIDs.Add((int)id); //se agrega el id a la lista

                Response.Cookies.Append("carrito", value: listaIDs.ToJson(), cookieOptions); //se envia la cookie navegador del cliente


            return Redirect(Request.Headers["Host"].ToString()); //Se queda en la misma pagina para seguir navegando productos
            }



            // GET: Products
            public async Task<IActionResult> Index()
            {
                return View(await _context.Smartphones.ToListAsync());
            }

            // GET: Products/Details/5
            public async Task<IActionResult> Details(int? id)
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


    }


    }