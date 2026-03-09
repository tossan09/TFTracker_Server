using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace TFTDataTrackerApi.Models
{
    public class Matches
    {
        public int id { get; set; }
        public int comp_id { get; set; } 
        public int patch_id { get; set; } 
        public int placement { get; set; }
        public int finallevel {  get; set; }
        public int goldstage32 { get; set; }
        public int goldstage41 { get; set; }
        public int hpstage32 { get; set; }
        public bool forced { get; set; }
        public bool contested { get; set; }
        [Required]
        public string comment { get; set; } = string.Empty;

    }
}
