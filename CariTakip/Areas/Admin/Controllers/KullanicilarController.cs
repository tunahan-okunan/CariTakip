using CariTakip.Areas.Admin.ViewModels;
using CariTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;

namespace CariTakip.Areas.Admin.Controllers
{
      [Authorize(Roles = "admin")]
    public class KullanicilarController : Controller
    {
        // GET: /Admin/Kullanicilar/
        public ActionResult Index()
        {
            var kullanicilar = Database.Session.Query<Kullanici>().OrderByDescending(x=>x.Id);
            return View(new KullanicilarIndex() 
            {
              Kullanicilar=kullanicilar
            });
        }

        public ActionResult Yeni()
        {
   
           // ViewBag.RolID = new SelectList(roller, "ID", "Adi");
 
            return View(new KullanicilarYeni()
            { 
           
             OrdersList=Database.Session.Query<Rol>().Select(x=> new SelectListItem{
             Text=x.Adi,
             Value=x.Id.ToString()
             } )
            });

        }
        [HttpPost]
        public ActionResult Yeni(KullanicilarYeni form)
        {
            var kullanici = new Kullanici();
          
            if (!ModelState.IsValid)
            {
             //   ViewBag.RolID = new SelectList(Database.Session.Query<Rol>(), "ID", "Adi");

                return View(new KullanicilarYeni()
                {

                    OrdersList = Database.Session.Query<Rol>().Select(x => new SelectListItem
                    {
                        Text = x.Adi,
                        Value = x.Id.ToString()
                    })

                });
            }
            

            if (Database.Session.Query<Kullanici>().Any(x=>x.Adi==form.Adi))
            {
                ModelState.AddModelError("Kullanici", "Böyle bir kullanıcı zaten mevcut.");
                return View(new KullanicilarYeni()
                {

                    OrdersList = Database.Session.Query<Rol>().Select(x => new SelectListItem
                    {
                        Text = x.Adi,
                        Value = x.Id.ToString()
                    })

                });
            }
            
            
            kullanici.Adi=form.Adi;
            kullanici.PasswordSet(form.Sifre);
            kullanici.Rol.Id= form.RolId;
            Database.Session.Save(kullanici);
            Database.Session.Flush();

            return RedirectToAction("index");
        }

      

        public ActionResult Sil(int id)
        {
            var kullanici = Database.Session.Load<Kullanici>(id);
            if (kullanici==null)
            {
                return HttpNotFound();
            }
            Database.Session.Delete(kullanici);
            Database.Session.Flush();

           return RedirectToAction("index");
        }

        public ActionResult Duzenle(int id) {

            var kullanici = Database.Session.Load<Kullanici>(id);
            var roller = Database.Session.Query<Rol>();
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            ViewBag.RolID = new SelectList(roller, "ID", "Adi",kullanici.Rol.Id);
            return View(new KullanicilarDuzenle()
            {
                Adi=kullanici.Adi
            });
        }

        [HttpPost]
        public ActionResult Duzenle(int RolID,int id,KullanicilarDuzenle form)
        {
            var kullanici = Database.Session.Load<Kullanici>(id);
            var rol = Database.Session.Load<Rol>(RolID);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            if (!ModelState.IsValid)
            {
               ViewBag.RolID = new SelectList( Database.Session.Query<Rol>(), "ID", "Adi",kullanici.Rol.Id);
               return View(form);
            }
            if (Database.Session.Query<Kullanici>().Any(x => x.Adi == form.Adi && x.Id!=id))
            {
                ModelState.AddModelError("Kullanici", "Böyle bir kullanıcı zaten mevcut.");
                ViewBag.RolID = new SelectList(Database.Session.Query<Rol>(), "ID", "Adi");
                return View(form);
            }

            kullanici.Adi = form.Adi;
            kullanici.Rol = rol;

            Database.Session.Update(kullanici);
            Database.Session.Flush();
            return RedirectToAction("index");
        }

        public ActionResult SifreDegistir(int id)
        {

            var kullanici = Database.Session.Load<Kullanici>(id);
       
            if (kullanici == null)
            {
                return HttpNotFound();
            }
       
            return View(new KullanicilarSifreDegistir()
            {
            });
        }

        [HttpPost]
        public ActionResult SifreDegistir(int id,KullanicilarSifreDegistir form)
        {

            var kullanici = Database.Session.Load<Kullanici>(id);

            if (kullanici == null)
            {
                return HttpNotFound();
            }
            kullanici.PasswordSet(form.Sifre);

            Database.Session.Update(kullanici);
            Database.Session.Flush();
            return RedirectToAction("index");
        }
	}
}