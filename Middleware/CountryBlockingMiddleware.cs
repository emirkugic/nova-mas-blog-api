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
            _blockedCountries = new HashSet<string> { "CN", "RU", "KP", "IR", "SY" }; //TODO: Test if it actually works with Bosnia
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
