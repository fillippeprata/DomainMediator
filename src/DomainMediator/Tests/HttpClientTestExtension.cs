using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace DomainMediator.Tests;

public static class HttpClientTestExtension
{
    public static HttpClient Init(this HttpClient client)
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("DomainWebApi_Tests");
        return client;
    }

    public static void SetAccessToken(this HttpClient client, string accessToken)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    public static async Task<JsonTestModel?> Send(this HttpClient client, string requestUri,
        WebAppTestModel? model = null)
    {
        model ??= new WebAppTestModel();

        StringContent? httpContent = null;
        if (model.Content != null)
            httpContent = new StringContent(JsonConvert.SerializeObject(model.Content), Encoding.UTF8,
                "application/json");

        var response = model.HttpMethod switch
        {
            TestHttpMethod.Get => await client.GetAsync(requestUri),
            TestHttpMethod.Post => await client.PostAsync(requestUri, httpContent),
            TestHttpMethod.Put => await client.PutAsync(requestUri, httpContent),
            TestHttpMethod.Patch => await client.PatchAsync(requestUri, httpContent),
            TestHttpMethod.Delete => await client.DeleteAsync(requestUri),
            _ => null
        };

        if (model.EnsureSuccess)
            response!.EnsureSuccessStatusCode();

        return await JsonTestModel.Parse(response!);
    }
}