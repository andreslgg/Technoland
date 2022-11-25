using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Dynamic;
using technoland.Models;
using Microsoft.AspNetCore.Identity;
using Technoland.Data;
using Technoland.Data.Migrations;
using Technoland.Models;
using System.Security.Claims;

namespace Technoland.Controllers
{
    public class CartController : Controller
    {
        // GET: CartController

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        CheckoutViewModel checkoutViewModel = new CheckoutViewModel();
        InvoiceViewModel invoiceCheckoutModel = new InvoiceViewModel();

        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context=context;            //contexto de la base de datos y el usuario actual
            _userManager = userManager;
        }



        public List<int>  listaIDs()   //metodo para obtener lista de IDs de los productos guardados en las cookies
        {
            var carritoJson = Request.Cookies["carrito"];  
            List<int> listaID = new List<int>();
            if (carritoJson != null)
            {
                listaID = new List<int>(carritoJson.FromJson<int[]>());  
            }
            return listaID;
        }

        public ActionResult Index()
        {
            SmartphonesController Smartphones = new SmartphonesController(_context);

            checkoutViewModel.CartProductIDs = listaIDs();
            checkoutViewModel.CartProducts = Smartphones.GetSmartphones(listaIDs());
   
            return View(checkoutViewModel); //le pasamos a la vista un modelo con las listas de IDs y productos de las cookies
        }

        public ActionResult addToCart(int id)
        {
            List<int> listaID = listaIDs(); 
            listaID.Add(id);
            Response.Cookies.Append("carrito", value: listaID.ToJson());

            return RedirectToAction("Index",PartialView("Cart"));
        }

        public ActionResult SubstractToCart(int id)
        {
            List<int> listaID = listaIDs();
            var index = listaID.IndexOf(id);
            if (index > -1) listaID.RemoveAt(index);
            Response.Cookies.Append("carrito", value: listaID.ToJson()); 

            return RedirectToAction("Index", PartialView("Cart"));
        }

   
    public async Task<IActionResult> DeleteItem(int? id)
        {
            List<int> listaID = listaIDs();
            listaID.RemoveAll(item => item == id);
            Response.Cookies.Append("carrito", value: listaID.ToJson()); 

            return RedirectToAction("Index", PartialView("Cart"));
        }

        [Authorize]
        public async Task<IActionResult> CreateInvoice()
        {
            var ID = Guid.NewGuid();
           

            if (ModelState.IsValid)
            {
                Invoices invoices = new Invoices();
                SmartphonesController Smartphones = new SmartphonesController(_context);

                var user = await _userManager.GetUserAsync(HttpContext.User); 

                List<int> listaID = listaIDs();
                checkoutViewModel.CartProductIDs = listaID;
                checkoutViewModel.CartProducts = Smartphones.GetSmartphones(listaID);
                
          
                double subtotal = checkoutViewModel.CartProducts.Sum(x => x.Price * checkoutViewModel.CartProductIDs.Where(productId => productId == x.Id).Count());
                double total = (subtotal  * 0.16)+(subtotal+100);

                invoices.Id = ID;
                invoices.CreatedDate = DateTime.Now;
                invoices.Total = Math.Round(total,2);
                invoices.Cart = listaID.ToJson();
                invoices.UserId = user.Id;


                _context.Add(invoices);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Invoices", new { ID });
        }

        public async Task<IActionResult> Invoices(Guid? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }
            var invoices = await _context.Invoices
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoices == null)
            {
                return NotFound();
            }

            SmartphonesController Smartphones = new SmartphonesController(_context);

            List<int> listaID = listaIDs();
            checkoutViewModel.CartProductIDs = listaID;
            checkoutViewModel.CartProducts = Smartphones.GetSmartphones(listaID);

            invoiceCheckoutModel.invoice = invoices;
            invoiceCheckoutModel.listas = checkoutViewModel;


            return View(invoiceCheckoutModel);
        }
    }
}
