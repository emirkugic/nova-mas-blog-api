using MaxMind.GeoIP2;
using System.Net;
using MaxMind.GeoIP2.Exceptions;

namespace nova_mas_blog_api.Middleware
{
    public class CountryBlockingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DatabaseReader _dbReader;
        private readonly HashSet<string> _blockedCountries;

        public CountryBlockingMiddleware(RequestDelegate next, string dbPath)
        {
            _next = next;
            _dbReader = new DatabaseReader(dbPath);
            _blockedCountries = new HashSet<string>
            {
                "AF", // Afghanistan
                "DZ", // Algeria
                "AO", // Angola
                "AM", // Armenia
                "AZ", // Azerbaijan
                "BH", // Bahrain
                "BD", // Bangladesh
                "BY", // Belarus
                "BJ", // Benin
                "BT", // Bhutan
                "BO", // Bolivia
                "BW", // Botswana
                "BN", // Brunei
                "BF", // Burkina Faso
                "BI", // Burundi
                "KH", // Cambodia
                "CM", // Cameroon
                "CV", // Cape Verde
                "CF", // Central African Republic
                "TD", // Chad
                "CL", // Chile
                "CN", // China
                "CO", // Colombia
                "KM", // Comoros
                "CG", // Congo - Brazzaville
                "CD", // Congo - Kinshasa
                "CR", // Costa Rica
                "CI", // Côte d’Ivoire
                "CU", // Cuba
                "DJ", // Djibouti
                "DO", // Dominican Republic
                "EC", // Ecuador
                "EG", // Egypt
                "SV", // El Salvador
                "GQ", // Equatorial Guinea
                "ER", // Eritrea
                "ET", // Ethiopia
                "FJ", // Fiji
                "GA", // Gabon
                "GM", // Gambia
                "GE", // Georgia
                "GH", // Ghana
                "GT", // Guatemala
                "GN", // Guinea
                "GW", // Guinea-Bissau
                "GY", // Guyana
                "HT", // Haiti
                "HN", // Honduras
                "IR", // Iran
                "IQ", // Iraq
                "JM", // Jamaica
                "JO", // Jordan
                "KZ", // Kazakhstan
                "KE", // Kenya
                "KP", // North Korea
                "KW", // Kuwait
                "KG", // Kyrgyzstan
                "LA", // Laos
                "LB", // Lebanon
                "LS", // Lesotho
                "LR", // Liberia
                "LY", // Libya
                "MG", // Madagascar
                "MW", // Malawi
                "MY", // Malaysia
                "MV", // Maldives
                "ML", // Mali
                "MR", // Mauritania
                "MU", // Mauritius
                "MX", // Mexico
                "MD", // Moldova
                "MN", // Mongolia
                "MA", // Morocco
                "MZ", // Mozambique
                "MM", // Myanmar (Burma)
                "NA", // Namibia
                "NP", // Nepal
                "NI", // Nicaragua
                "NE", // Niger
                "NG", // Nigeria
                "OM", // Oman
                "PK", // Pakistan
                "PA", // Panama
                "PG", // Papua New Guinea
                "PY", // Paraguay
                "PE", // Peru
                "PH", // Philippines
                "QA", // Qatar
                "RO", // Romania
                "RU", // Russia
                "RW", // Rwanda
                "SA", // Saudi Arabia
                "SN", // Senegal
                "SL", // Sierra Leone
                "SG", // Singapore
                "SO", // Somalia
                "ZA", // South Africa
                "SS", // South Sudan
                "SD", // Sudan
                "SR", // Suriname
                "SZ", // Swaziland
                "SY", // Syria
                "TJ", // Tajikistan
                "TZ", // Tanzania
                "TH", // Thailand
                // "TG", // Togo
                "TN", // Tunisia
                "TR", // Turkey
                "TM", // Turkmenistan
                "UG", // Uganda
                "UA", // Ukraine
                "AE", // United Arab Emirates
                "UZ", // Uzbekistan
                "VE", // Venezuela
                "VN", // Vietnam
                "YE", // Yemen
                "ZM", // Zambia
                "ZW"  // Zimbabwe
            };
            //TODO: Test if it actually works with Bosnia
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            if (ipAddress!.IsIPv4MappedToIPv6)
            {
                ipAddress = ipAddress.MapToIPv4();
            }

            string countryIsoCode = null!;

            if (IsPublicIP(ipAddress))
            {
                try
                {
                    countryIsoCode = _dbReader.Country(ipAddress.ToString()).Country.IsoCode!;
                }
                catch (AddressNotFoundException)
                {
                    await _next(context);
                    return;
                }

                if (_blockedCountries.Contains(countryIsoCode!))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Service not yet available in your country.");
                    return;
                }
            }

            await _next(context);
        }

        private bool IsPublicIP(IPAddress ipAddress)
        {
            if (IPAddress.IsLoopback(ipAddress) || ipAddress.Equals(IPAddress.IPv6Loopback))
            {
                return false;
            }

            var bytes = ipAddress.GetAddressBytes();
            return !(
                (bytes[0] == 10) ||
                (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
                (bytes[0] == 192 && bytes[1] == 168)
            );
        }
    }
}
