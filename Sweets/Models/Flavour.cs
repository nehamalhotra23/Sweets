using System.Collections.Generic;

namespace Sweets.Models
{
    public class Flavour
    {
        public Flavour()
        {
            this.Treats = new HashSet<FlavourTreat>();
            
        }

        public int FlavourId { get; set; }
        public string FlavourName { get; set; }
        public virtual ApplicationUser User { get; set; }

    
        public virtual ICollection<FlavourTreat> Treats { get;}
        

    }
}