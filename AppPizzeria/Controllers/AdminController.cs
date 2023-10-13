using AppPizzeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AppPizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AggiungiProdotto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiProdotto(Prodotto prodotto)
        {
            prodotto.Foto = "Logo.jpg";

            if (ModelState.IsValid)
            {
                if (prodotto.FotoFile != null && prodotto.FotoFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(prodotto.FotoFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/"), fileName);
                    prodotto.FotoFile.SaveAs(path);

                    prodotto.Foto = fileName;
                }
                db.Prodotto.Add(prodotto);
                db.SaveChanges();
            }
            return View(prodotto);
        }

        public ActionResult ListaProdotti()
        {
            var prodotti = db.Prodotto.ToList();
            return View(prodotti);
        }

        public ActionResult EliminaProdotto(int id)
        {
            var prodotto = db.Prodotto.Find(id);

            if (prodotto != null)
            {
                db.Prodotto.Remove(prodotto);
                db.SaveChanges();
            }
            return RedirectToAction("ListaProdotti");
        }

        [HttpGet]
        public ActionResult ModificaProdotto(int id)
        {
            var prodotto = db.Prodotto.Find(id);
            return View(prodotto);
        }

        [HttpPost]
        public ActionResult ModificaProdotto(Prodotto prodotto)
        {
            if (ModelState.IsValid)
            {
                if (prodotto.FotoFile != null && prodotto.FotoFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(prodotto.FotoFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img/"), fileName);
                    prodotto.FotoFile.SaveAs(path);

                    prodotto.Foto = fileName;
                }

                db.Entry(prodotto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListaProdotti");
            }
            return View(prodotto);
        }

        [HttpGet]
        public ActionResult GestioneOrdini()
        {
            var listaOrdini = db.Ordine.Where(o => o.IsEvaso == false).ToList();
            return View(listaOrdini);
        }

        [HttpPost]
        public ActionResult GestioneOrdini(Ordine ordine)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiornaOrdine(int id)
        {
            var order = db.Ordine.Find(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Ordine non trovato");
            }
            order.IsEvaso = true;
            //db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true });
        }

        [HttpGet]
        public JsonResult OrdiniEvasi()
        {
            var listaOrdini = db.Ordine.Where(o => o.IsEvaso == true && o.DataOrdine == DateTime.Today).Count();

            return Json(listaOrdini, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Incasso(DateTime dataOrdine)
        {
            var ordini = db.Ordine.Where(o => o.IsEvaso == true && o.DataOrdine == dataOrdine);
            var incasso = ordini.Any() ? ordini.Sum(o => o.CostoTotale) : 0;

            return Json(incasso, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cassa()
        {
            return View();
        }
    }
}