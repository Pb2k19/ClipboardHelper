using System.Configuration;

namespace ClipboardHelper.Services.ConfigService;

public class ConfigService : IConfigService
{
    public T? GetConfigSetting<T>(string key, T defaultValue)
    {
        try
        {
            object value = Properties.Settings.Default[key];

            if (value is not null)
                return (T)value;
            else
                return defaultValue;
        }
        catch (Exception ex)
        {
            if (ex is InvalidCastException or SettingsPropertyNotFoundException)
                return defaultValue;

            throw;
        }
    }

    public bool SetConfigProperty<T>(string key, T value)
    {
        try
        {
            if (value.Equals(GetConfigSetting(key, string.Empty)))
                return true;

            Properties.Settings.Default[key] = value;
            Properties.Settings.Default.Save();
            return true;
        }
        catch (Exception ex)
        {
            if (ex is SettingsPropertyIsReadOnlyException or SettingsPropertyNotFoundException or SettingsPropertyWrongTypeException)
                return false;

            throw;
        }
    }
}
