using System.Text.Json.Nodes;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SAM.Core.IoT
{
    public static partial class Query
    {
        public static async Task<JsonObject> Response(string uri, TimeSpan timeout)
        {
            if(string.IsNullOrWhiteSpace(uri))
            {
                return null;
            }

            JsonObject result = null;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.ConnectionClose = true;
                httpClient.Timeout = timeout;

                using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(timeout))
                {
                    cancellationTokenSource.CancelAfter(timeout);

                    HttpResponseMessage httpResponseMessage = null;
                    try
                    {
                        httpResponseMessage = await httpClient.GetAsync(uri, cancellationTokenSource.Token);
                    }
                    catch (TaskCanceledException taskCanceledException)
                    {
                        httpResponseMessage = null;

                        httpClient.CancelPendingRequests();
                    }
                    catch (Exception exception)
                    {
                        httpResponseMessage = null;

                        httpClient.CancelPendingRequests();
                    }

                    if (httpResponseMessage != null && httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        try
                        {
                            string json = await httpResponseMessage.Content.ReadAsStringAsync();
                            result = JsonNode.Parse(json) as JsonObject;
                        }
                        catch
                        {
                            result = null;
                        }
                    }
                }
            }

            return result;
        }

        public static async Task<JsonObject> Response(string uri)
        {
            return await Response(uri, TimeSpan.FromSeconds(5));
        }
    }
}