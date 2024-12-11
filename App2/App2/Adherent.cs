using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
   public class Adherent
    {
        public string Num_identification { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Date_naissance { get; set; }
        public int Age { get; set; }
        public string Nom_complet { get { return Nom+" "+Prenom; } } 
    }
}
