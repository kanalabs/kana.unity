using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Client.Abstractions.Websocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class KanalabsGraphQL
{

    private GraphQLHttpClient graphQLClient;

    public KanalabsGraphQL(string baseUrl)
    {
        var httpClientOption = new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(baseUrl)
        };
        graphQLClient = new GraphQLHttpClient(httpClientOption, new NewtonsoftJsonSerializer());
    }


    public async Task<decimal> GetAccountBalance(string account, int chainId, List<string> tokens)
    {
        var request = new GraphQLRequest
        {
            Query = @"query  ( $chainId: Int! , $account: String!, $tokens: [String!]) {
                            accountBalances(chainId: $chainId, account: $account, tokens: $tokens) {
                            items {
                              balance
                              token
                            }
                          }
                        }",
            Variables = new
            {
                chainId,
                account,
                tokens
            }
        }; 

        var graphQLResponse = await graphQLClient.SendQueryAsync<AccountBalanceResponse>(request);
        
        if(graphQLResponse.Data != null && graphQLResponse.Data.AccountBalances.Items.Any())
        {
            var accountBalance = graphQLResponse.Data.AccountBalances.Items.FirstOrDefault();
            Debug.Log("Account Balance : " + accountBalance.Balance);
            var balanceAmount = Helpers.ConvertHexToDecimal(accountBalance.Balance);
            Debug.Log("Balance Amount : " + balanceAmount);
            return balanceAmount;
        }

        return 0;       
    }





}
