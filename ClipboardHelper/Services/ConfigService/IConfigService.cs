namespace ClipboardHelper.Services.ConfigService;

public interface IConfigService
{
    T? GetConfigSetting<T>(string key, T defaultValue);
    bool SetConfigProperty<T>(string key, T value);
}