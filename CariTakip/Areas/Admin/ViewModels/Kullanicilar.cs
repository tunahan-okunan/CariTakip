using CariTakip.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CariTakip.Areas.Admin.ViewModels
{
    public class KullanicilarIndex
    {
        public IEnumerable<Kullanici> Kullanicilar { get; set; }

    }

   
    public class KullanicilarYeni
    {
        [Required(ErrorMessage = "Ad alanı boş geçilemez.")]
        [MaxLength(20)]
        public string Adi { get; set; }
        [Required(ErrorMessage = "Soyad alanı boş geçilemez.")]
        [MaxLength(12)]
        public string Sifre { get; set; }

        public int RolId { get; set; }

        public IEnumerable<SelectListItem> OrdersList { get; set; }

     

    }

    public class KullanicilarDuzenle
    {
        [Required(ErrorMessage = "Ad alanı boş geçilemez.")]
        [MaxLength(20)]
        public string Adi { get; set; }
        
    }
    public class KullanicilarSifreDegistir
    {
        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        [MaxLength(12)]
        public string Sifre { get; set; }

    }
}