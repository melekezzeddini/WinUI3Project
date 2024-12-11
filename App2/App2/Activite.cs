using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public class Activite { 
            public int IdActivite { get; set; }
            public string Nom { get; set; }
            public double CoutOrganisation { get; set; }
            public double PrixParticipation { get; set; }
            public int IdType { get; set; }
            public string TypeName { get; set; }  
        public string FCoutOrganisation => $"{CoutOrganisation:0.00} $";
        public string FPrixParticipation => $"{PrixParticipation:0.00} $";

    }
}
