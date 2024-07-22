using System.Net.Http.Headers;
using System.Text;
using System.Web;
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
            TestHttpMethod.Get => await GetAsync(),
            TestHttpMethod.Post => await client.PostAsync(requestUri, httpContent),
            TestHttpMethod.Put => await client.PutAsync(requestUri, httpContent),
            TestHttpMethod.Patch => await client.PatchAsync(requestUri, httpContent),
            TestHttpMethod.Delete => await client.DeleteAsync(requestUri),
            _ => null
        };

        if (!model.EnsureSuccess || response!.IsSuccessStatusCode) return await JsonTestModel.Parse(response!);

        var errorMessage = string.Empty;
        try
        {
            var jsonErrorModel = await JsonTestModel.Parse(response);
            errorMessage = jsonErrorModel!.GetNotifications()!.Aggregate("Error testing http request. | ", (current, notification) => current + $"{notification.Message} | ");
        }
        catch
        {
            response.EnsureSuccessStatusCode();
        }

        throw new Exception(errorMessage);

        #region Local methods

        async Task<HttpResponseMessage> GetAsync()
        {
            var queryParams = "";
            if (model.QueryParameters != null)
                queryParams = $"?{GetQueryString()}";
            return await client.GetAsync(requestUri + queryParams);
        }

        string GetQueryString()
        {
            var obj = model.QueryParameters;
            var properties = from p in obj.GetType().GetProperties()
                where p.GetValue(obj, null) != null
                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        #endregion
    }
}
