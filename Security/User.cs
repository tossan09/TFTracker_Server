namespace TFTDataTrackerApi.Security
{
    public class User
    {
        public required string AdmUser { get; set; }
        public required List<string> Roles { get; set; }
    }
}
