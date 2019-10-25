using System.Collections.Generic;

namespace Sweets.Models
{
  public class Treat
    {
        public Treat()
        {
            this.Flavours = new HashSet<FlavourTreat>();
        }

        public int TreatId { get; set; }
        public string TreatName { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<FlavourTreat> Flavours { get; set; }
    }
}
