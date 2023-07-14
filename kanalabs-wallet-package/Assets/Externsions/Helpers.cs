using Nethereum.Web3;
using System;
using System.Globalization;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

public static class Helpers 
{
    public static decimal ConvertHexToDecimal(string hex)
    {
        if (hex.StartsWith("0x") || hex.StartsWith("0X"))
        {
            hex = hex.Remove(0, 2);
        }
        var bigIntBalance = BigInteger.Parse(HexToDecimal(hex));
        var amount = Web3.Convert.FromWei(bigIntBalance);
        return amount;
    }

    static string HexToDecimal(string hex)
    {
        List<int> dec = new(); // { 0 };   // decimal result

        foreach (char c in hex)
        {
            int carry = Convert.ToInt32(c.ToString(), 16);
            // initially holds decimal value of current hex digit;
            // subsequently holds carry-over for multiplication

            for (int i = 0; i < dec.Count; ++i)
            {
                int val = dec[i] * 16 + carry;
                dec[i] = val % 10;
                carry = val / 10;
            }

            while (carry > 0)
            {
                dec.Add(carry % 10);
                carry /= 10;
            }
        }

        var chars = dec.Select(d => (char)('0' + d));
        var cArr = chars.Reverse().ToArray();
        return new string(cArr);
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
