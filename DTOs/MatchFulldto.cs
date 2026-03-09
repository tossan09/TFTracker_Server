using System.ComponentModel.DataAnnotations;

namespace TFTDataTrackerApi.DTOs
{
    public class MatchFulldto
    {
        public int id { get; set; }
        [Required]
        public string compname { get; set; } = string.Empty;
        [Required]
        public string patchnumber { get; set; } = string.Empty;
        public int placement { get; set; }
        public int finallevel { get; set; }
        public int goldstage32 { get; set; }
        public int goldstage41 { get; set; }
        public int hpstage32 { get; set; }
        public bool forced { get; set; }
        public bool contested { get; set; }
        [Required]
        public string comment { get; set; } = string.Empty;

    }
}
