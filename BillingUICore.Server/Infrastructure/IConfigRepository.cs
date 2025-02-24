namespace BillingUICore.Server.Infrastructure
{
    public interface IConfigRepository
    {
        IDictionary<string, string> GlobalSettings { get; }
        IDictionary<string, string> GetSettings(string groupCode);
        string GetValue(string groupCode, string settingCode);
    }
}
