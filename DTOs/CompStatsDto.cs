namespace TFTDataTrackerApi.DTOs
{
    public class CompStatsDto
    {
        public required string CompId { get; set; }
        public string? PatchId { get; set; }
        public decimal AvgPlacement { get; set; }
        public decimal WinratePercent { get; set; }
        public decimal Top4RatePercent { get; set; }
        public decimal Bot8RatePercent { get; set; }
        public decimal TotalMatches { get; set; }
    }
}
