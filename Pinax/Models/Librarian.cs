namespace Pinax.Models;

public class Librarian
{
    private readonly PinaxConfiguration _pinaxConfiguration;
    private readonly string _userSecretsToken;

    public Librarian(PinaxConfiguration pinaxConfiguration, string userSecretsToken)
    {
        _pinaxConfiguration = pinaxConfiguration;
        _userSecretsToken = userSecretsToken;
    }
}