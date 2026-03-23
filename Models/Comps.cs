using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace TFTDataTrackerApi.Models
{
    public class Comps
    {
        public int id {  get; set; }
        [Required] 
        public string name { get; set; } = string.Empty;
        
        public string? traits { get; set; } = string.Empty; // nova tabela traits?
        
        public string? style { get; set; } = string.Empty; //trocar tipagem certa
        [Required]
        public int setid { get; set; }
    }
}
