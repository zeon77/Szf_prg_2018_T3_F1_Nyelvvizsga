using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nyelvvizsga
{
    class Program
    {
        static void Main(string[] args)
        {
            //1. Beolvasás
            List<Nyelvvizsga> nyelvvizsgak = new List<Nyelvvizsga>();

            foreach (var sor in File.ReadAllLines(@"sikeres.csv", Encoding.Default).Skip(1))
            {
                nyelvvizsgak.Add(new Nyelvvizsga(sor, true));
            }

            foreach (var sor in File.ReadAllLines(@"sikertelen.csv", Encoding.Default).Skip(1))
            {
                nyelvvizsgak.Add(new Nyelvvizsga(sor, false));
            }

            //foreach (var v in nyelvvizsgak)
            //{
            //    Console.WriteLine($"{v.Sikeres}, {v.Nyelv}, {v.ÖsszesViszga}");
            //}

            //2. feladat
            Console.WriteLine("2. feladat: A legnépszerűbb nyelvek: ");
            var result2 = nyelvvizsgak
                .GroupBy(v => v.Nyelv)
                .Select(g => new { Nyelv = g.Key, Összes = g.Sum(v => v.ÖsszesVizsga) })
                .OrderByDescending(x => x.Összes)
                .Take(3);

            foreach (var item in result2)
            {
                Console.WriteLine($"\t{item.Nyelv}");
            }

            //3. feladat: Bekérés
            Console.WriteLine("3. feladat:");
            Console.Write("\tVizsgálandó év (2009-2018): ");
            int Évszám = int.Parse(Console.ReadLine());
            while (!(Évszám >= 2009 && Évszám <= 2018))
            {
                Console.Write("\tHiba. Vizsgálandó év (2009-2018): ");
                Évszám = int.Parse(Console.ReadLine());
            }

            //4. feladat
            Console.WriteLine("4. feladat: ");

            var result4 = nyelvvizsgak
                .GroupBy(v => v.Nyelv)
                .Select(g => new {
                    Nyelv = g.Key,
                    Arány = g.Sum(v => v.Vizsgák[Évszám]) == 0 ? 0 : g.Single(x => x.Sikeres == false).Vizsgák[Évszám] / (double)g.Sum(v => v.Vizsgák[Évszám]) * 100
                })
                .OrderByDescending(x => x.Arány)
                .First();

            Console.WriteLine($"\t{Évszám}-ben {result4.Nyelv} nyelvből a sikertelen vizsgák aránya: {result4.Arány:0.00}%");

            //5. feladat
            Console.WriteLine("5. feladat");
            var result5 = nyelvvizsgak
                .GroupBy(v => v.Nyelv)
                .Select(g => new {
                    Nyelv = g.Key,
                    ÖsszesVizsgaAdottÉvben = g.Sum(v => v.Vizsgák[Évszám]),
                })
                .Where(v => v.ÖsszesVizsgaAdottÉvben == 0);


            if (result5.Count() == 0)
            {
                Console.WriteLine("\tMinden nyelvből volt vizsgázó");
            }
            else
            {
                foreach (var item in result5)
                {
                    Console.WriteLine($"\t{item.Nyelv}: {item.ÖsszesVizsgaAdottÉvben}");
                }
            }

            //6. feladat
            Console.WriteLine("6. feladat");
            StreamWriter sw = new StreamWriter(@" osszesites.csv");
            var result6 = nyelvvizsgak
                .GroupBy(v => v.Nyelv)
                .Select(g => new {
                    Nyelv = g.Key,
                    Összes = g.Sum(v => v.ÖsszesVizsga),
                    Arány = g.Sum(v => v.ÖsszesVizsga) == 0 ? 0 : g.Single(x => x.Sikeres == true).ÖsszesVizsga / (double)g.Sum(v => v.ÖsszesVizsga) * 100
                });

            foreach (var item in result6)
            {
                sw.WriteLine($"{item.Nyelv};{item.Összes};{item.Arány:0.00}%");
                Console.WriteLine($"{item.Nyelv};{item.Összes};{item.Arány:0.00}%");
            }

            sw.Close();

            Console.ReadKey();
        }
    }
}
