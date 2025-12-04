using System.Text.Encodings.Web;

namespace AspNetCoreIdentityApp.Service.Services
{
    public class TwoFactorialService
    {
        private readonly UrlEncoder _urlEncoder;

        public TwoFactorialService(UrlEncoder urlEncoder)
        {
            _urlEncoder = urlEncoder;
        }
        
        public string GenerateQrCodeUri(string email,string unformattedKey)
        {
            const string format = "otpauth://totp/{0}?secret={1}&issuer={2}&digits=6";
            return string.Format(format, _urlEncoder.Encode("www.localhost:7114,com"), _urlEncoder.Encode(email), unformattedKey);
        }
    }
}
