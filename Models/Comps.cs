using System.ComponentModel.DataAnnotations;

namespace TFTDataTrackerApi.Models
{
    public class Comps
    {
        public int id {  get; set; }
        [Required] 
        public string name { get; set; } = string.Empty;
        [Required]
        public string traits { get; set; } = string.Empty; // nova tabela traits?
        [Required]
        public string style { get; set; } = string.Empty; //trocar tipagem certa
    }
}
