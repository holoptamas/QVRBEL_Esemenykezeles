using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QVRBEL_Esemenykezeles
{
    internal class Program
    {
        class Etel
        {
            public string megnevezes;
            public string[] hozzavalok;

            public Etel(string megnevezes, string[] hozzavalok)
            {
                this.megnevezes = megnevezes;
                this.hozzavalok = hozzavalok;
            }
        }

        public delegate void RendelesTelesitesKezelo(string etelNeve);
        public delegate void HozzavaloSzuksegesKezelo(string hozzavalo);
        public delegate void HozzavaloElkeszultKezelo(string hozzavalo);

        class Sef
        {
            Etel cel;
            int szuksegesHozzavaloSzam;

            public Etel[] receptek = new Etel[]
            {
                new Etel("poharviz", new string[] {"viz"}),
                new Etel("leves", new string[] { "repa", "hus", "krumpli", "viz" } ),
                new Etel("rantothus", new string[] { "hus", "krumpli" } ),
                new Etel("fozelek", new string[] { "viz", "repa" } )
            };

            public RendelesTelesitesKezelo RendelesTeljesitve;
            public RendelesTelesitesKezelo RendelesNemTeljesitve;
            public HozzavaloSzuksegesKezelo HozzavaloSzukseges;

            public void Megrendeles(string etelNeve)
            {
                int i = 0;
                while (i < receptek.Length && receptek[i].megnevezes != etelNeve)
                {
                    i++;
                }
                if (i < receptek.Length)
                {
                    Elkeszites(receptek[i]);
                }
                else
                {
                    RendelesNemTeljesitve(etelNeve);
                }
            }

            public void Elkeszites(Etel recept)
            {
                this.cel = recept;
                szuksegesHozzavaloSzam = cel.hozzavalok.Length;
                for (int i = 0; i < cel.hozzavalok.Length; i++)
                {
                    HozzavaloSzukseges(cel.hozzavalok[i]);
                }
            }

            public void SzakacsElkeszult(string hozzavalo)
            {
                szuksegesHozzavaloSzam--;
                if (szuksegesHozzavaloSzam == 0)
                {
                    RendelesTeljesitve(cel.megnevezes);
                }
            }

            public void Felvesz(Szakacs szakacs)
            {
                HozzavaloSzukseges += szakacs.SefKerValmait;
                szakacs.HozzavaloElkeszult += SzakacsElkeszult;
            }
        }

        class Szakacs
        {
            public string nev { get; }
            public string specialitas;

            public Szakacs(string nev, string specialitas)
            {
                this.nev = nev;
                this.specialitas = specialitas;
            }

            public HozzavaloElkeszultKezelo HozzavaloElkeszult;

            public void SefKerValmait(string hozzavalo)
            {
                if (hozzavalo == specialitas)
                {
                    Foz();
                }
            }

            public void Foz()
            {
                HozzavaloElkeszult(specialitas);
            }
        }

        static void Main(string[] args)
        {
            Sef s = new Sef();
            s.RendelesTeljesitve += Sikeres;
            s.RendelesNemTeljesitve += Sikertelen;
            Szakacs sz = new Szakacs("Pista", "viz");
            s.Felvesz(sz);
            s.Megrendeles("poharviz");
            s.Megrendeles("kecskesajt");

            Console.ReadLine();
        }

        public static void Sikeres(string etelNeve)
        {
            Console.WriteLine($"* Sikeres rendelés '{etelNeve}'");
        }

        public static void Sikertelen(string etelNeve)
        {
            Console.WriteLine($"* Sikertelen rendelés '{etelNeve}'");
        }
    }
}
