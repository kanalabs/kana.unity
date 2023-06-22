using Nethereum.Web3;
using System.Globalization;
using System.Numerics;

public static class Helpers 
{
    public static decimal ConvertHexToDecimal(string hex)
    {
        if (hex.StartsWith("0x") || hex.StartsWith("0X"))
        {
            hex = hex.Remove(0, 2);
        }
        var bigIntBalance = BigInteger.Parse(hex, NumberStyles.HexNumber);
        var amount = Web3.Convert.FromWei(bigIntBalance);
        return amount;
    }


    public static LoginParams GetLoginProvider(LoginProviders loginProvider)
    {
        var options = new LoginParams();
        switch (loginProvider)
        {
            case (LoginProviders.Facebook):
                options.loginProvider = Provider.FACEBOOK;
                break;
            case (LoginProviders.Google):
                options.loginProvider = Provider.GOOGLE;
                break;
            case (LoginProviders.Twitter):
                options.loginProvider = Provider.TWITTER;
                break;
        }

        return options;
    }
}
