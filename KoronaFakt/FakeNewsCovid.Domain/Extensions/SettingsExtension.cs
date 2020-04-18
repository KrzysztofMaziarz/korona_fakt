using Microsoft.Extensions.Configuration;

namespace FakeNewsCovid.Domain.Extensions
{
    public static class SettingsExtension
    {
        public static TClass GetSettings<TClass>(this IConfiguration configuration)
            where TClass : class, new()
        {
            var section = typeof(TClass).Name.Replace("Settings", string.Empty);
            TClass configurationValue = new TClass();
            var result = configuration.GetSection(section);

            if (!result.Exists())
            {
                throw new System.ArgumentException("I don't have such Settings " + section);
            }

            result.Bind(configurationValue);

            return configurationValue;
        }
    }
}
