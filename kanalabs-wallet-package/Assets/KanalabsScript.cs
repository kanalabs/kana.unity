using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Client.Abstractions.Websocket;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;


public class KanalabsScript : MonoBehaviour
{

    [SerializeField]
    Chain chain;

    [SerializeField]
    LoginProviders loginProvider;
     
    public Button LoginButton;
    public InputField WelcomeText;
    public InputField PublicAddressText;
    public InputField SmartAddressText;
    public InputField AccountBalanceText;
    public string TestPrivateKey;
    public string Tokens;

    Web3Auth web3Auth;
    KanalabsGraphQL graphQL;

    private static readonly string etherspotEndpoint = "https://qa-etherspot.pillarproject.io/";
    

    void Awake()
    {
        graphQL = new KanalabsGraphQL(etherspotEndpoint);

        LoginButton.onClick.AddListener(InitializeWeb3Auth);
         
        LoadAccountDetails(TestPrivateKey);

        Debug.Log("Cubescript Initialized"); 
    }

    public void InitializeWeb3Auth()
    {
        Debug.Log("Web3Auth calling login");

        web3Auth = GetComponent<Web3Auth>();        

        web3Auth.setOptions(new Web3AuthOptions()
        {
            redirectUrl = new Uri(web3Auth.redirectUri),
            clientId = web3Auth.clientId,
            network = web3Auth.network,
        });
        web3Auth.onLogin += onLogin;
        web3Auth.onLogout += onLogout;

        web3Auth.login(Helpers.GetLoginProvider(loginProvider));

        Debug.Log("Web3Auth logged in");
    }

    private void onLogin(Web3AuthResponse response)
    {
        var privateKey = response.privKey;
        Debug.Log($"Private key:{privateKey}");
        LoadAccountDetails(privateKey);

        var userInfo = JsonConvert.SerializeObject(response.userInfo, Formatting.Indented);        
        WelcomeText.text = $"Welcome {response.userInfo.name}!";
        Debug.Log(userInfo);
    }


    public void logout()
    {
        web3Auth.logout();
    }

    private void onLogout()
    {
        Debug.Log("Logged out!");
    }

    private async void LoadAccountDetails(string privateKey)
    {
        var chainId = (int)chain;
        var address = SmartContract.GetPublicAddress(privateKey, chainId);
        PublicAddressText.text = address;
        var smartAddress = SmartContract.GetSmartAddress(address);
        SmartAddressText.text = smartAddress;
        var balanceAmount = await graphQL.GetAccountBalance(smartAddress, chainId, GetTokens()); 
        AccountBalanceText.text = balanceAmount.ToString();
    }

    private List<string> GetTokens()
    {
        var tokensList = new List<string>();
        if (!string.IsNullOrEmpty(Tokens))
        {
            tokensList = Tokens.Split(",").ToList();
        }

        return tokensList;
    }
 

    // Start is called before the first frame update
    void Start()
    { 
        //Debug.Log("Started script");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Updating script");
    }
}
