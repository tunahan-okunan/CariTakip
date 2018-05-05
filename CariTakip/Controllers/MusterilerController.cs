using CariTakip.Models;
using CariTakip.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using CariTakip.InfraStructure;
namespace CariTakip.Controllers
{
    [Authorize(Roles = "Admin,Guest")]

    [SelectedTabAttribute("musteriler")]
    public class MusterilerController : Controller
    {
        // GET: /Musteriler/  
        public ActionResult Index(string SecilenArama, string arama)
        {

            var musteriler = Database.Session.Query<Musteri>().OrderByDescending(x => x.Id);


            if (!String.IsNullOrEmpty(arama))
            {
                if (SecilenArama == "Ad")
                {
                    musteriler = Database.Session.Query<Musteri>().Where(x => x.Ad.Contains(arama)).OrderByDescending(x => x.Id);
                }
                else if (SecilenArama == "FirmaAd")
                {
                    musteriler = Database.Session.Query<Musteri>().Where(x => x.FirmaAdi.Contains(arama)).OrderByDescending(x => x.Id); ;
                }
                else if (SecilenArama == "TcNo")
                {
                    musteriler = Database.Session.Query<Musteri>().Where(x => x.Tc.StartsWith(arama)).OrderByDescending(x => x.Id); ;
                }
                else if (SecilenArama == "VergiNo")
                {
                    musteriler = Database.Session.Query<Musteri>().Where(x => x.VergiNo.StartsWith(arama)).OrderByDescending(x => x.Id); ;
                }
                else if (SecilenArama == "Adres")
                {
                    musteriler = Database.Session.Query<Musteri>().Where(x => x.Adres.Contains(arama)).OrderByDescending(x => x.Id); ;
                }
            }

            return View(new MusterilerIndex()
            {
                Musteriler = musteriler
            }
            );
        }


        public ActionResult SahisYeni()
        {
            return View(new MusterilerSahisYeni() { });
        }
        [HttpPost]
        public ActionResult SahisYeni(MusterilerSahisYeni form)
        {

            if (!ModelState.IsValid)
            {
                return View(form);
            }
            if (form.Tel.Length < 10)
            {
                ModelState.AddModelError("Telefon", "Telefon 10 haneden oluşmak zorundadır.");
                return View(form);
            }
            if (form.Tc.Length < 11)
            {
                ModelState.AddModelError("TC", "TC 11 haneden oluşmak zorundadır.");
                return View(form);
            }
            var Musteri = new Musteri()
            {
                Tc = form.Tc,
                Ad = form.Ad,
                Soyad = form.Soyad,
                Tel = form.Tel,
                Email = form.Email,
                Adres = form.Adres,

            };

            Database.Session.Save(Musteri);
            Database.Session.Flush();
            return RedirectToAction("Index");
        }
        public ActionResult FirmaYeni()
        {
            return View(new MusterilerFirmaYeni() { });
        }
        [HttpPost]
        public ActionResult FirmaYeni(MusterilerFirmaYeni form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            if (form.Tel.Length < 10)
            {
                ModelState.AddModelError("Telefon", "Telefon 10 haneden oluşmak zorundadır.");
                return View(form);
            }
            if (form.VergiNo.Length < 10)
            {
                ModelState.AddModelError("VergiNo", "Vergi no 10 haneden oluşmak zorundadır.");
                return View(form);
            }

            var Musteri = new Musteri()
            {
                VergiNo = form.VergiNo,
                Ad = form.Ad,
                Soyad = form.Soyad,
                FirmaAdi = form.FirmaAdi,
                Tel = form.Tel,
                Email = form.Email,
                Adres = form.Adres,
            };

            Database.Session.Save(Musteri);
            Database.Session.Flush();

            return RedirectToAction("Index");
        }

        public ActionResult Duzenle(int id)
        {
            var musteriler = Database.Session.Load<Musteri>(id);
            if (musteriler == null)
            {
                return HttpNotFound();
            }
            return View(new MusterilerDuzenle() {
                Tc = musteriler.Tc,
                VergiNo = musteriler.VergiNo,
                Ad = musteriler.Ad,
                Soyad = musteriler.Soyad,
                FirmaAdi = musteriler.FirmaAdi,
                Tel = musteriler.Tel,
                Email = musteriler.Email,
                Adres = musteriler.Adres

            });
        }
        [HttpPost]
        public ActionResult Duzenle(int id, MusterilerDuzenle form)
        {
            var musteri = Database.Session.Load<Musteri>(id);
            if (musteri == null)
            {
                return HttpNotFound();
            }
            else if (form.Tel.Length < 10)
            {
                ModelState.AddModelError("Telefon", "Telefon 10 haneden oluşmak zorundadır.");
                return View(form);
            }
            else if (form.Tc == null && form.VergiNo == null)
            {
                ModelState.AddModelError("TcVergi", "VergiNo ve TcNo alanlarından en az birisi doldurulmalıdır.");
                return View(form);
            }
            else if (form.FirmaAdi != null && form.Tc != null)
            {
                ModelState.AddModelError("TcFirma", "Şahıs için Firma adı girilemez.");
                return View(form);
            }
            else if (form.Tc != null && form.VergiNo != null)
            {
                ModelState.AddModelError("TcVergi", "VergiNo ve TcNo alanları aynı anda doldurulamaz.");
                return View(form);
            }
            else if (form.FirmaAdi == null && form.VergiNo != null)
            {
                ModelState.AddModelError("FirmaVergi", "FirmaAdi ve VergiNo alanları aynı anda doldurulmalı.");
                return View(form);
            }


            if (Database.Session.Query<Musteri>().Any(x=>x.Tc==form.Tc && x.Id!=id) && form.Tc!=null)
            {
                ModelState.AddModelError("Tc", "Tc kimlik numarası benzersiz olmalıdır.");
                return View(form);
            }
            if (Database.Session.Query<Musteri>().Any(x => x.VergiNo == form.VergiNo && x.Id != id) && form.VergiNo != null)
            {
                ModelState.AddModelError("VergiNo", "Vergi numarası benzersiz olmalıdır.");
                return View(form);
            }

            musteri.Tc = form.Tc;
            musteri.VergiNo = form.VergiNo;
            musteri.Ad = form.Ad;
            musteri.Soyad = form.Soyad;
            musteri.FirmaAdi = form.FirmaAdi;
            musteri.Tel = form.Tel;
            musteri.Email = form.Email;
            musteri.Adres = form.Adres;


            Database.Session.Update(musteri);
            Database.Session.Flush();
            return RedirectToAction("Index");
        }

        public ActionResult CopeAt(int id)
        {
            var musteri = Database.Session.Load<Musteri>(id);
            if (musteri==null)
            {
                return HttpNotFound();
            }
            musteri.SilmeTarihi = DateTime.Now;

            Database.Session.Update(musteri);
            Database.Session.Flush();

            return RedirectToAction("Index");
        }
        public ActionResult Kurtar(int id)
        {
            var musteri = Database.Session.Load<Musteri>(id);
            if (musteri == null)
            {
                return HttpNotFound();
            }
            musteri.SilmeTarihi = null;

            Database.Session.Update(musteri);
            Database.Session.Flush();

            return RedirectToAction("Index");
        }
        public ActionResult Sil(int id)
        {
            var musteri = Database.Session.Load<Musteri>(id);
            if (musteri == null)
            {
                return HttpNotFound();
            }
            Database.Session.Delete(musteri);
            Database.Session.Flush();

            return RedirectToAction("index");
        }

         private const int PostPerPage=10;
        public ActionResult KasaIndex(int page=1,int id=0)
        {  
            var kur = new KasaIndex();
            double giren = 0, cikan = 0; 
            double guncelDolar=kur.DolarKurİslem(),guncelEuro=kur.EuroKurİslem();

          
            
            if (page<1)
            {
                return HttpNotFound();
            }
            var TotalCount = Database.Session.Query<CariHesap>().Where(x => x.Musteri.Id == id).Count();

            var currentPage=Database.Session.Query<CariHesap>().Where(x=>x.Musteri.Id==id)
                .OrderByDescending(x=>x.Tarih)
                .Skip((page-1)*PostPerPage)
                .Take(PostPerPage).ToList();

         
            var carihesaplar = Database.Session.Query<CariHesap>().Where(x=>x.Musteri.Id==id).OrderByDescending(x=>x.Tarih);
          
            foreach (var item in carihesaplar)
            {
                if (item.OdemeSekli.Adi =="DOLAR")
                {
                    giren = giren + item.GirilenMiktar*guncelDolar;
                    cikan = cikan + item.CikanMiktar * guncelDolar;
                }
                else if (item.OdemeSekli.Adi == "EURO")
                {
                    giren = giren + item.GirilenMiktar*guncelEuro;
                    cikan = cikan + item.CikanMiktar * guncelEuro;
                }
                else
                {
                    giren = giren + item.GirilenMiktar;
                    cikan = cikan + item.CikanMiktar;
                }
            }
            var musteri = Database.Session.Load<Musteri>(id);
          //  var b = Database.Session.Query<Musteri>().Single(x => x.Id == id).Ad;
            
            return View(new KasaIndex()
            {
                CariHesap=new PagedData<CariHesap>(currentPage,TotalCount,page,PostPerPage),
                MusteriAdi=musteri.Ad,
                MusteriSoyadi=musteri.Soyad,
                GirenMiktar = giren,
                CikanMiktar = -cikan,
                KalanMiktar = giren - cikan,
                MusteriID = id,
          
            });
        }
        public ActionResult KasaYeni(int musteriId)
        {
            var musteri=Database.Session.Load<Musteri>(musteriId);;
            var turler=Database.Session.Query<Tur>();
            var odeme=Database.Session.Query<OdemeSekli>();
            ViewBag.TurID = new SelectList(turler, "Id", "Adi");
            ViewBag.OdemeID = new SelectList(odeme, "Id", "Adi");
            return View(new KasaYeni()
            {
                MusteriID=musteri.Id,
                MusteriAdi=musteri.Ad,
                MusteriSoyadi=musteri.Soyad,
                Tarih=DateTime.Now  
            });
        }
          [HttpPost]
        public ActionResult KasaYeni(int TurID,int OdemeID,int musteriId,KasaYeni form)
        {
            double  giren=0,cikan=0;


            if (!ModelState.IsValid)
            {
                ViewBag.TurID = new SelectList(Database.Session.Query<Tur>(), "Id", "Adi");
                ViewBag.OdemeID = new SelectList(Database.Session.Query<OdemeSekli>(), "Id", "Adi");
                return View(form);
            }
      
            var carihesap = new CariHesap();
            
 
            foreach (var item in  Database.Session.Query<CariHesap>().Where(x => x.Musteri.Id == musteriId))
            {
                giren = giren + item.GirilenMiktar;
                cikan = cikan + item.CikanMiktar;
            }

             carihesap.Tarih = form.Tarih;
             carihesap.Aciklama = form.Aciklama;
             carihesap.GirilenMiktar = form.Verilen;
             carihesap.CikanMiktar = form.Alinan;

             carihesap.OdemeSekli = Database.Session.Load<OdemeSekli>(OdemeID);
             carihesap.Musteri =Database.Session.Load<Musteri>(musteriId);
             carihesap.Tur = Database.Session.Load<Tur>(TurID);
             if (carihesap.Tur.Id == 1 && carihesap.GirilenMiktar == 0)
             {
                 return Redirect("KasaYeni?MusteriID=" + musteriId);
             }
             else if (carihesap.Tur.Id == 2 && carihesap.CikanMiktar == 0)
             {
                 return Redirect("KasaYeni?MusteriID=" + musteriId);
             }

            Database.Session.Save(carihesap);
             Database.Session.Flush();
             return Redirect("KasaIndex?Id=" + musteriId);
        }

          public ActionResult KasaDuzenle(int id)
          {
              var carihesap = Database.Session.Load<CariHesap>(id);
              if(carihesap==null)
              {
                  return HttpNotFound();
              }   

              ViewBag.TurID = new SelectList(Database.Session.Query<Tur>(), "Id", "Adi", carihesap.Tur.Id);
              ViewBag.OdemeID = new SelectList(Database.Session.Query<OdemeSekli>(), "Id", "Adi",carihesap.OdemeSekli.Id);
              return View(new KasaDuzenle()
              {
                  MusteriID = carihesap.Musteri.Id,
                  MusteriAdi=carihesap.Musteri.Ad,
                  MusteriSoyadi=carihesap.Musteri.Soyad,
                  Tarih = carihesap.Tarih,
                  Aciklama=carihesap.Aciklama,
                    Verilen=carihesap.GirilenMiktar,
                  Alinan=carihesap.CikanMiktar
              });
          }

           [HttpPost]
          public ActionResult KasaDuzenle(int id,int TurID,int OdemeID, KasaDuzenle form)
          {

              var carihesap = Database.Session.Query<CariHesap>().FirstOrDefault(x => x.Id == id);
     
              if (carihesap == null)
              {
                  return HttpNotFound();
              }
              if (!ModelState.IsValid)
              {
                  ViewBag.TurID = new SelectList(Database.Session.Query<Tur>(), "Id", "Adi", carihesap.Tur.Id);
                  ViewBag.OdemeID = new SelectList(Database.Session.Query<OdemeSekli>(), "Id", "Adi", carihesap.OdemeSekli.Id);
                  return View(form);
              }
              carihesap.Tarih = form.Tarih;
              carihesap.Aciklama = form.Aciklama;
              carihesap.GirilenMiktar = form.Verilen;
              carihesap.CikanMiktar = form.Alinan;
              carihesap.OdemeSekli = Database.Session.Load<OdemeSekli>(OdemeID);
              carihesap.Tur = Database.Session.Load<Tur>(TurID);

              Database.Session.Update(carihesap);
              Database.Session.Flush();
              return Redirect("KasaIndex?Id=" + carihesap.Musteri.Id);

          }

           public ActionResult KasaSil(int id)
           {
               var carihesap = Database.Session.Load<CariHesap>(id);
               if (carihesap == null)
               {
                   return HttpNotFound();
               }
               Database.Session.Delete(carihesap);
               Database.Session.Flush();
               return Redirect("KasaIndex?Id=" + carihesap.Musteri.Id);
           }
          public ActionResult KasaExcellCikart(int musteriId)
          {
             var cariHesap = Database.Session.Query<CariHesap>().Where(x => x.Musteri.Id == musteriId).Select(x=>new {
                  x.Tarih,
                  x.Aciklama,
                  x.GirilenMiktar,
                  x.CikanMiktar,
              });
     

              var musteri = Database.Session.Load<Musteri>(musteriId);

              var gv = new GridView();
              gv.DataSource = cariHesap;
              gv.DataBind();
              Response.ClearContent();
              Response.Buffer = true;
              Response.AddHeader("content-disposition", "attachment; filename=" + musteri.Ad +"-"+musteri.Soyad+".xls");
              Response.ContentType = "application/vnd.ms-excel";
              Response.Charset = "";
              StringWriter objStringWriter = new StringWriter();
              HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

              gv.AllowPaging = false;
      
              //Baslik rowlarinin arka planini beyaz olarak ayarliyoruz. 
              gv.HeaderRow.Style.Add("background-color", "#FFFFFF");
              //hücre basliklarinin arka planini yesil yapiyoruz 
              gv.HeaderRow.Cells[0].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[1].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[2].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[3].Style.Add("background-color", "lightgreen");
              for (int i = 0; i < gv.Rows.Count; i++)
              {
                  GridViewRow row = gv.Rows[i];
                  //Arka plan rengini beyaz olarak ayarliyoruz 
                  row.BackColor = System.Drawing.Color.White;
                  //Her row’un text özelligine bir class atiyoruz 
                  row.Attributes.Add("class", "textmode");
                  //2. Row’larin arka planlarina farkli bir renk veriyoruz 
                  if (i % 2 != 0)
                  {
                      row.Cells[0].Style.Add("background-color", "#C2D69B");
                      row.Cells[1].Style.Add("background-color", "#C2D69B");
                      row.Cells[2].Style.Add("background-color", "#C2D69B");
                      row.Cells[3].Style.Add("background-color", "#C2D69B");
                  }
              }

              gv.RenderControl(objHtmlTextWriter);
              Response.Output.Write(objStringWriter.ToString());
              Response.Flush();
              Response.End();
              return Redirect("KasaIndex?Id=" + musteriId);
          }

        public ActionResult TumMusteriBilgileriniExcelleVer()
        {

            var cariHesap = Database.Session.Query<CariHesap>().Select(x => new {
                x.Musteri.Ad,
                x.Musteri.Soyad,
                x.Musteri.FirmaAdi,
                x.Tarih,
                x.Aciklama,
                x.GirilenMiktar,
                x.CikanMiktar,

            });

            var gv = new GridView();
            gv.DataSource = cariHesap;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=TumMusteri.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.AllowPaging = false;

            //Baslik rowlarinin arka planini beyaz olarak ayarliyoruz. 
            gv.HeaderRow.Style.Add("background-color", "#FFFFFF");
            //hücre basliklarinin arka planini yesil yapiyoruz 
            gv.HeaderRow.Cells[0].Style.Add("background-color", "lightgreen");
            gv.HeaderRow.Cells[1].Style.Add("background-color", "lightgreen");
            gv.HeaderRow.Cells[2].Style.Add("background-color", "lightgreen");
            gv.HeaderRow.Cells[3].Style.Add("background-color", "lightgreen");
            gv.HeaderRow.Cells[4].Style.Add("background-color", "lightgreen");
            gv.HeaderRow.Cells[5].Style.Add("background-color", "lightgreen");
            gv.HeaderRow.Cells[6].Style.Add("background-color", "lightgreen");
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                //Arka plan rengini beyaz olarak ayarliyoruz 
                row.BackColor = System.Drawing.Color.White;
                //Her row’un text özelligine bir class atiyoruz 
                row.Attributes.Add("class", "textmode");
                //2. Row’larin arka planlarina farkli bir renk veriyoruz 
                if (i % 2 != 0)
                {
                    row.Cells[0].Style.Add("background-color", "#C2D69B");
                    row.Cells[1].Style.Add("background-color", "#C2D69B");
                    row.Cells[2].Style.Add("background-color", "#C2D69B");
                    row.Cells[3].Style.Add("background-color", "#C2D69B");
                    row.Cells[4].Style.Add("background-color", "#C2D69B");
                    row.Cells[5].Style.Add("background-color", "#C2D69B");
                    row.Cells[6].Style.Add("background-color", "#C2D69B");
                }
            }

            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }
          
          public ActionResult KasaMakbuz(int id,int musteriId)
          {
              var cariHesap = Database.Session.Query<CariHesap>().Where(x => x.Musteri.Id == musteriId && x.Id==id).Select(x => new
              {
                  x.Musteri.Ad,
                  x.Musteri.Soyad,
                  Tur_Adi=x.Tur.Adi,
                  Odeme_Adi=x.OdemeSekli.Adi,
                  x.Tarih,
                  x.Aciklama,
                  Borcumuz=x.GirilenMiktar,
                  Alacagimiz=x.CikanMiktar,
                 
              });
              var musteri = Database.Session.Load<Musteri>(musteriId);
              var gv = new GridView();
              gv.DataSource = cariHesap;
              gv.DataBind();
              Response.ClearContent();
              Response.Buffer = true;
              Response.AddHeader("content-disposition", "attachment; filename=" + musteri.Ad +"-"+musteri.Soyad+".doc");
              Response.ContentType = "application/ms-word";
              Response.Charset = "";
              StringWriter objStringWriter = new StringWriter();
              HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

              gv.AllowPaging = false;
              gv.DataBind();
              //Baslik rowlarinin arka planini beyaz olarak ayarliyoruz. 
              gv.HeaderRow.Style.Add("background-color", "#FFFFFF");
              //hücre basliklarinin arka planini yesil yapiyoruz 
              gv.HeaderRow.Cells[0].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[1].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[2].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[3].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[4].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[5].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[6].Style.Add("background-color", "lightgreen");
              gv.HeaderRow.Cells[7].Style.Add("background-color", "lightgreen");
            for (int i = 0; i < gv.Rows.Count; i++)
              {
                  GridViewRow row = gv.Rows[i];
                  //Arka plan rengini beyaz olarak ayarliyoruz 
                  row.BackColor = System.Drawing.Color.White;
                  //Her row’un text özelligine bir class atiyoruz 
                  row.Attributes.Add("class", "textmode");
                  //2. Row’larin arka planlarina farkli bir renk veriyoruz 
                  if (i % 2 != 0)
                  {
                      row.Cells[0].Style.Add("background-color", "#C2D69B");
                      row.Cells[1].Style.Add("background-color", "#C2D69B");
                      row.Cells[2].Style.Add("background-color", "#C2D69B");
                      row.Cells[3].Style.Add("background-color", "#C2D69B");
                  }
              }

              gv.RenderControl(objHtmlTextWriter);
              Response.Output.Write(objStringWriter.ToString());
              Response.Flush();
              Response.End();  
              return View();
          }
	}
}