using AppPizzeria.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AppPizzeria.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AggiungiAlCarrello()
        {
            var prodotti = db.Prodotto.ToList();
            return View(prodotti);
        }

        [HttpPost]
        public ActionResult MoveToCart(int id, int quantita)
        {
            if (Session["IdOrdine"] != null)
            {
                var ordine = new DettaglioOrdini
                {
                    Quantita = quantita,
                    IdProdotto = id,
                    IdOrdine = Convert.ToInt32(Session["IdOrdine"]),
                };
                db.DettaglioOrdini.Add(ordine);
                db.SaveChanges();
                return RedirectToAction("AggiungiAlCarrello");
            }
            return View("Ordine");
        }

        [HttpGet]
        public ActionResult Ordine()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ordine(Ordine ordine)
        {
            ordine.DataOrdine = DateTime.Today;
            ordine.CostoTotale = 0;
            ordine.IsEvaso = false;
            ordine.IdUtente = Convert.ToInt32(Session["userId"]);

            if (ModelState.IsValid)
            {
                db.Ordine.Add(ordine);
                db.SaveChanges();
                Session["IdOrdine"] = ordine.IdOrdine;
            }
            return RedirectToAction("AggiungiAlCarrello");
        }

        [HttpGet]
        public ActionResult RiepilogoOrdine()
        {
            var idOrdine = Convert.ToInt16(Session["IdOrdine"]);
            Ordine ordine = db.Ordine.Where(o => o.IdOrdine == idOrdine).FirstOrDefault();
            decimal prezzoTotale = 0;

            if (ordine != null)
            {
                foreach (var dettaglio in ordine.DettaglioOrdini)
                {
                    decimal prezzoProdotto = dettaglio.Prodotto.Prezzo;
                    prezzoTotale += prezzoProdotto * dettaglio.Quantita;
                }

                ViewBag.PrezzoTotale = prezzoTotale;
                ordine.CostoTotale = prezzoTotale;
                db.SaveChanges();
                return View(ordine.DettaglioOrdini);
            }
            return View("Ordine");
        }

        public ActionResult ModificaOrdine(int id, int quantita)
        {
            var order = db.DettaglioOrdini.Find(id);
            order.Quantita = quantita;
            db.SaveChanges();

            return RedirectToAction("RiepilogoOrdine");
        }

        public ActionResult EliminaOrdine(int id)
        {
            var ordine = db.DettaglioOrdini.Find(id);
            db.DettaglioOrdini.Remove(ordine);
            db.SaveChanges();
            return RedirectToAction("RiepilogoOrdine");
        }

        public ActionResult InviaOrdine()
        {
            Session.Remove("IdOrdine");
            return RedirectToAction("Index", "Home");
        }
    }
}