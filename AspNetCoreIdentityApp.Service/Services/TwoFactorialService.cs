using System.Text.Encodings.Web;

namespace AspNetCoreIdentityApp.Service.Services;

public class TwoFactorialService
{
    private readonly UrlEncoder _urlEncoder;

    public TwoFactorialService(UrlEncoder urlEncoder)
    {
        _urlEncoder = urlEncoder;
    }

    public string GenerateQrCodeUri(string email, string unformattedKey)
    {
        var issuer = _urlEncoder.Encode("www.localhost:7114.com");

        return $"otpauth://totp/{issuer}:{_urlEncoder.Encode(email)}" +
               $"?secret={unformattedKey}&issuer={issuer}&digits=6";
    }
}
