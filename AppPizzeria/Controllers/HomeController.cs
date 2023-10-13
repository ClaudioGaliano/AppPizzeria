using AppPizzeria.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AppPizzeria.Controllers
{
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult Registrazione()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrazione(Registrazione registrazione)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState
            //        .Where(x => x.Value.Errors.Count > 0)
            //        .Select(x => new { x.Key, x.Value.Errors })
            //        .ToArray();
            //}

            if (ModelState.IsValid)
            {
                var utente = new Utente
                {
                    Username = registrazione.Username,
                    Password = registrazione.Password,
                    Cognome = registrazione.Cognome,
                    Nome = registrazione.Nome,
                    Ruolo = "User"
                };

                db.Utente.Add(utente);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login utente)
        {
            if (ModelState.IsValid)
            {
                Utente user = db.Utente.Where(u => u.Username == utente.Username && u.Password == utente.Password).FirstOrDefault();
                FormsAuthentication.SetAuthCookie(user.Username, false);
                //Session["utente"] = user;
                Session["userId"] = user.IdUtente;
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
