namespace GameShopAPI.Helpers
{
    public class AppSettings
    {
        public required string SecretKey { get; set; } = Environment.GetEnvironmentVariable("APP_SECRET_KEY")!;
        public required int TokenExpirationDays { get; set; }
    }
}
