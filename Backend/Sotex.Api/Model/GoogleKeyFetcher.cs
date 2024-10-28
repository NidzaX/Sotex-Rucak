using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Sotex.Api.Model;

public class GoogleKeyFetcher
{
    private static readonly HttpClient HttpClient = new HttpClient();

    public async Task<IEnumerable<SecurityKey>> GetGooglePublicKeysAsync()
    {
        const string jwksUrl = "https://www.googleapis.com/oauth2/v3/certs";

        var response = await HttpClient.GetStringAsync(jwksUrl);
        var googleJwks = JsonConvert.DeserializeObject<GoogleJwks>(response);

        var securityKeys = new List<SecurityKey>();

        foreach (var key in googleJwks.Keys)
        {
            var rsaParameters = new RSAParameters
            {
                Modulus = Base64UrlEncoder.DecodeBytes(key.N),
                Exponent = Base64UrlEncoder.DecodeBytes(key.E)
            };

            var rsa = RSA.Create();
            rsa.ImportParameters(rsaParameters);
            var securityKey = new RsaSecurityKey(rsa) { KeyId = key.Kid };
            securityKeys.Add(securityKey);
        }

        return securityKeys;
    }
}
