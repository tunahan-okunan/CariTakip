using CariTakip.InfraStructure;
using CariTakip.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CariTakip.ViewModels
{
    public class MusterilerIndex
    {

        public IEnumerable<Musteri> Musteriler { get; set; }
  
    }
    public class MusterilerSahisYeni
    {
        
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Lütfen Tc alanına sadece rakamla yazınız.")]
        [Required(ErrorMessage ="Tc alanının doldurulması zorunludur.")]
        public string Tc { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Ad alanının doldurulması zorunludur.")]
        public string Ad { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Soyad alanının doldurulması zorunludur.")]
        public string Soyad { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Lütfen Telefon alanına sadece rakamla yazınız.")] 
        [DataType(DataType.PhoneNumber)]
        
        [Required(ErrorMessage = "Telefon alanının doldurulması zorunludur.")]
        public string Tel { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Email alanının doldurulması zorunludur.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Adres alanının doldurulması zorunludur.")]
        public string Adres { get; set; }

    }
    public class MusterilerFirmaYeni
    {
       
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Lütfen VergiNo alanına sadece harfle yazınız.")]
        [Required(ErrorMessage = "VergiNo alanının doldurulması zorunludur.")]
        public string VergiNo { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Ad alanının doldurulması zorunludur.")]
        public string Ad { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Soyad alanının doldurulması zorunludur.")]
        public string Soyad { get; set; }

        [MaxLength(256)]
        [Required(ErrorMessage = "FirmaAd alanının doldurulması zorunludur.")]
        public string FirmaAdi { get; set; }


        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Lütfen Telefon alanına sadece rakamla yazınız.")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Telefon alanının doldurulması zorunludur.")]
        public string Tel { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Email alanının doldurulması zorunludur.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Adres alanının doldurulması zorunludur.")]
        public string Adres { get; set; }
    }
    public class MusterilerDuzenle
    {
        
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Lütfen Tc alanına sadece rakamla yazınız.")]
        public string Tc { get; set; }
        
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Lütfen VergiNo alanına sadece sayı yazınız.")]
        public string VergiNo { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Ad alanının doldurulması zorunludur.")]
        public string Ad { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Soyad alanının doldurulması zorunludur.")]
        public string Soyad { get; set; }
        [MaxLength(256)]
        public string FirmaAdi { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Lütfen Telefon alanına sadece rakamla yazınız.")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Telefon alanının doldurulması zorunludur.")]
        public string Tel { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Email alanının doldurulması zorunludur.")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required(ErrorMessage = "Adres alanının doldurulması zorunludur.")]
        public string Adres { get; set; }

    }

    public class KasaIndex
    {

        public PagedData<CariHesap> CariHesap { get; set; }
        public double GirenMiktar { get; set; }
        public double CikanMiktar { get; set; }
        public double KalanMiktar { get; set; }
        public int MusteriID { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriSoyadi { get; set; }
        public double DolarKurİslem()
        {

            XmlDocument xmlVerisi = new XmlDocument();
            xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");
            double dolar = Convert.ToDouble(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
            return dolar;
        }
        public double EuroKurİslem()
        {
            XmlDocument xmlVerisi = new XmlDocument();
            xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");
            double Euro = Convert.ToDouble(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
            return Euro;
        }
     
    }
    public class KasaYeni
    {
        [DataType(DataType.DateTime, ErrorMessage = "Lütfen tarih alanına doğru formatta giriniz")]
        [Required(ErrorMessage = "Tarih alanını doldurmak zorunludur.")]
        public DateTime Tarih { get; set; }
        [Required(ErrorMessage = "Açıklama alanını doldurmak zorunludur.")]
        public string Aciklama { get; set; }

        [Required(ErrorMessage = "Açıklama alanını doldurmak zorunludur.")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Lütfen Verilen alanına sadece rakamla yazınız.")]
        public double Verilen { get; set; }
        [Required(ErrorMessage = "Alınan alanını doldurmak zorunludur.")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Lütfen Alınan alanına sadece rakamla yazınız.")]
        public double Alinan { get; set; }

        public int MusteriID { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriSoyadi { get; set; }

       
    
    }
    public class KasaDuzenle
    {
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Tarih alanını doldurmak zorunludur.")]
        public DateTime Tarih { get; set; }
        [Required(ErrorMessage = "Açıklam alanını doldurmak zorunludur.")]
        public string Aciklama { get; set; }
        [Required(ErrorMessage = "Verilen alanını doldurmak zorunludur.")]
        public double Verilen { get; set; }
        [Required(ErrorMessage = "Alınan alanını doldurmak zorunludur.")]
        public double Alinan { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriSoyadi { get; set; }
        public int MusteriID { get; set; }

       
    }
}