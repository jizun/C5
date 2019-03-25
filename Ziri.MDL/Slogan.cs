using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("Slogan")]
    public class Slogan
    {
        [Key]
        public int ID { get; set; }
        public string Text { get; set; }
    }
    
}
