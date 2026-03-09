using System.ComponentModel.DataAnnotations;

namespace TFTDataTrackerApi.Models
{
    public class Patches
    {
        public int id { get; set; }
        [Required]
        public string patch_number { get; set; } = string.Empty;
        public int Set_id { get; set; }
    }
}
