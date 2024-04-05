using System.Net;
using Newtonsoft.Json;
using SteamAuthentication.Exceptions;

#pragma warning disable CS0168

namespace SteamAuthentication.LogicModels;

public class TimeDeferenceRestClient : SteamRestClient
{
    public TimeDeferenceRestClient(IWebProxy? proxy) : base(proxy)
    {
    }

    public async Task<long> GetSteamTimeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response =
                await ExecutePostRequestWithoutHeadersAsync(Endpoints.TwoFactorTimeQuery, null, "steamid=0", cancellationToken);

            if (!response.IsSuccessful)
                throw new RequestException("响应不成功", response.StatusCode, response.Content, null);

            if (response.Content == null)
                throw new RequestException("响应内容为空", response.StatusCode, null, null);

            var timeQuery = JsonConvert.DeserializeObject<TimeQuery>(response.Content);

            if (timeQuery == null)
                throw new RequestException("反序列化的时间查询值为空", response.StatusCode,
                    response.Content, null);

            return timeQuery.Response.ServerTime;
        }
        catch (RequestException e)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new RequestException("获取时差时出现异常", null, null, e);
        }
    }
}

internal class TimeQuery
{
    [JsonProperty("response")] internal TimeQueryResponse Response { get; }

    public TimeQuery(TimeQueryResponse response)
    {
        Response = response;
    }

    internal class TimeQueryResponse
    {
        [JsonProperty("server_time")] public long ServerTime { get; }

        public TimeQueryResponse(long serverTime)
        {
            ServerTime = serverTime;
        }
    }
}