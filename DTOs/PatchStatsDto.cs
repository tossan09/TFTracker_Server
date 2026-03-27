namespace TFTDataTrackerApi.DTOs
{
    public class PatchStatsDto
    {
        public required string PatchNumber { get; set; }
        public decimal AvgPlacement { get; set; }
        public decimal WinratePercent { get; set; }
        public decimal Top4RatePercent { get; set; }
        public decimal Bot8RatePercent { get; set; }
        public decimal AvgConsistency { get; set; }
        public int TotalMatches { get; set; }
    }
}
