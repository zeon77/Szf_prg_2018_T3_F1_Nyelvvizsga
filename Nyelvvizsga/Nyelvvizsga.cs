using System;
using System.Collections.Generic;
using System.Linq;

namespace Nyelvvizsga
{
    class Nyelvvizsga
    {
        public bool Sikeres { get; set; }
        public string Nyelv { get; set; }
        
        //Vizsgák<Év,VizsgákSzáma>
        public Dictionary<int, int> Vizsgák { get; set; }

        public int ÖsszesVizsga
        {
            get
            {
                return Vizsgák.Sum(v => v.Value);
            }
        }

        public Nyelvvizsga(string sor, bool Sikeres)
        {
            this.Sikeres = Sikeres;
            string[] tmp = sor.Split(';');
            Nyelv = tmp[0];
            Vizsgák = new Dictionary<int, int>();
            int Év = 2009;
            for (int i = 1; i < tmp.Length; i++)
            {
                Vizsgák.Add(Év, int.Parse(tmp[i]));
                Év++;
            }
        }
    }
}
