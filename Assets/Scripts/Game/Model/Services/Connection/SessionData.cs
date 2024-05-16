namespace Game.Model.Services.Connection
{
    public class SessionData
    {
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public string ServiceId { get; set; }
    }
}