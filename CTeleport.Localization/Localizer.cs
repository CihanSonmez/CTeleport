using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace CTeleport.Localization
{
    public class Localizer
    {
        private readonly ResourceManager resourceManager;
        private static readonly List<string> supportedCultures = new List<string> { "en", "nl", "de" };

        public Localizer()
        {
            this.resourceManager = new ResourceManager(typeof(Resource));
        }

        #region Prep
        private string GetValue([CallerMemberName] string caller = null) => resourceManager.GetString(caller);
        #endregion

        #region Resource Compatibility Check
        public static void ResourceCompatibilityCheck()
        {
#if DEBUG
            ResourceManager rm = new ResourceManager(typeof(Resource));

            var props = typeof(Localizer).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList().Where(p => p.PropertyType == typeof(string));
            var methods = typeof(Localizer).GetMethods(BindingFlags.Public | BindingFlags.Instance).ToList().Where(m => !m.IsSpecialName && m.ReturnType == typeof(string));

            var resourceKeys = new List<string>();
            resourceKeys.AddRange(props.Select(p => p.Name));
            resourceKeys.AddRange(methods.Select(m => m.Name));

            resourceKeys.Remove("ToString");

            resourceKeys = resourceKeys.Distinct().ToList();

            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalUICulture = Thread.CurrentThread.CurrentUICulture;

            foreach (var culture in supportedCultures)
            {
                var cultureInfo = new CultureInfo(culture);

                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                foreach (var key in resourceKeys)
                {
                    try
                    {
                        rm.GetString(key);
                    }
                    catch (Exception)
                    {
                        throw new Exception($"Key: {key} not found in for {culture} culture");
                    }
                }
            }

            Thread.CurrentThread.CurrentCulture = originalCulture;
            Thread.CurrentThread.CurrentUICulture = originalUICulture;
#endif
        }
        #endregion

        public string AnErrorOccured => GetValue();
        public string FirstIataCodeCannotBeEmpty => GetValue();
        public string FirstIataCodeShouldBe3Letter => GetValue();
        public string SecondIataCodeCannotBeEmpty => GetValue();
        public string SecondIataCodeShouldBe3Letter => GetValue();
        public string InvalidLatitude(double latitude, string iataCode) => GetValue().Format(latitude, iataCode);
        public string InvalidLongitude(double longitude, string iataCode) => GetValue().Format(longitude, iataCode);
    }
}