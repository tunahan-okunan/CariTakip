using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CariTakip.Models
{
    public class Kullanici
    {

        public virtual int Id { get; set; }
        public virtual string Adi { get; set; }
        public virtual string Sifre { get; set; }
        public virtual  Rol Rol { get; set; }
        public virtual void PasswordSet(string sifre)
        {
            Sifre = BCrypt.Net.BCrypt.HashPassword(sifre, 13);
        }
        public virtual bool CheckPassword(string sifre)
        {
            return BCrypt.Net.BCrypt.Verify(sifre, Sifre);
        }
        public static void FakeHash()
        {

            BCrypt.Net.BCrypt.HashPassword("", 13);
        }
        public Kullanici()
        {
            Rol = new Rol();
        }
        
    }
    public class KullaniciMap : ClassMapping<Kullanici>
    {
        public KullaniciMap()
        {
            Table("kullanicilar");
            Id(x => x.Id, x => x.Generator(Generators.Identity));
            Property(x => x.Adi, x => x.NotNullable(true));
            Property(x => x.Sifre, x => x.NotNullable(true));
            ManyToOne(x => x.Rol, x =>
            {
                x.Column("rol_id");
                x.NotNullable(true);

            });
        }

    }
}