using DomainMediator.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DomainMediator.Tests;

public class JsonTestModel(bool success, bool containsUserNotification, object[] notifications, object? response)
{
    public HttpResponseMessage? HttpResponse { get; private set; }
    public bool Success { get; private set; } = success;
    public bool ContainsUserNotification { get; private set; } = containsUserNotification;
    private object? ObjectResponse { get; } = response;
    public object[] Notifications { get; } = notifications;

    public T? ResponseData<T>() where T : class
    {
        if (ObjectResponse == null)
            return null;

        var resposta = ((JToken)ObjectResponse).ToString();
        return JsonConvert.DeserializeObject<T>(resposta);
    }

    public DomainNotification[]? GetNotifications()
    {
        return Notifications.Length == 0 ? null : JToken.FromObject(Notifications).ToObject<DomainNotification[]>();
    }

    public static async Task<JsonTestModel?> Parse(HttpResponseMessage response)
    {
        var result = JsonConvert.DeserializeObject<JsonTestModel>(await response.Content.ReadAsStringAsync());
        if (result == null) return new JsonTestModel(false, false, [], null) { HttpResponse = response };

        result.HttpResponse = response;
        return result;
    }
}
