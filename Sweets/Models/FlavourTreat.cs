using System.ComponentModel.DataAnnotations.Schema;
namespace Sweets.Models
{
  public class FlavourTreat
    {       
        public int FlavourTreatId { get; set; }
        public int TreatId { get; set; }
        public int FlavourId { get; set; }

        [ForeignKey("TreatId")]
        public Treat Treat { get; set; }
        [ForeignKey("FlavourId")]
        public Flavour Flavour { get; set; }
    }
}