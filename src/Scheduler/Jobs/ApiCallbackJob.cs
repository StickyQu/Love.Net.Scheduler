using System;
using System.Net.Http;
using Hangfire;

namespace Love.Net.Scheduler.Jobs {
    public static class ApiCallbackJob {
        /// <exception cref="ApiCallbackJobException">Condition.</exception>
        public static void ApiCallback(string callbackUrl, string method = "GET") {
            try {
                using (var client = new HttpClient()) {
                    HttpResponseMessage response;
                    bool responseIsSuccessStatusCode;
                    switch (method) {
                        case "GET":
                            using (response = client.GetAsync(callbackUrl).Result) {
                                responseIsSuccessStatusCode = response.IsSuccessStatusCode;
                            }
                            break;
                        case "POST":
                            using (response = client.PostAsJsonAsync(callbackUrl, new { }).Result) {
                                responseIsSuccessStatusCode = response.IsSuccessStatusCode;
                            }
                            break;
                        case "PUT":
                            using (response = client.PutAsJsonAsync(callbackUrl, new { }).Result) {
                                responseIsSuccessStatusCode = response.IsSuccessStatusCode;
                            }
                            break;
                        case "DELETE":
                            using (response = client.DeleteAsync(callbackUrl).Result) {
                                responseIsSuccessStatusCode = response.IsSuccessStatusCode;
                            }
                            break;

                        default:
                            throw new NotSupportedException($"not supported method:{method}");
                    }
                    if (!responseIsSuccessStatusCode)
                        throw new ApiCallbackJobException(callbackUrl);
                }
            }
            catch (Exception ex) {
                throw new ApiCallbackJobException(callbackUrl, ex);
            }
        }
        public static void ApiCallbackAndRemove(string callbackUrl) {
            try {
                using (var client = new HttpClient()) {
                    var response = client.GetAsync(callbackUrl).Result;
                    if (!response.IsSuccessStatusCode)
                        throw new ApiCallbackJobException(callbackUrl);
                    RecurringJob.RemoveIfExists(callbackUrl);
                }
            }
            catch (Exception ex) {
                throw new ApiCallbackJobException(callbackUrl, ex);
            }
        }
    }
}
