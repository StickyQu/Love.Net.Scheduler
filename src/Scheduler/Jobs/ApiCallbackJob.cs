using System;
using System.Net.Http;

namespace Love.Net.Scheduler.Jobs {
    public static class ApiCallbackJob {
        /// <exception cref="ApiCallbackJobException">Condition.</exception>
        public static void ApiCallback(string callbackUrl) {
            try {
                using (var client = new HttpClient()) {
                    var response = client.GetAsync(callbackUrl).Result;
                    if (!response.IsSuccessStatusCode)
                        throw new ApiCallbackJobException(callbackUrl);
                }
            }
            catch (Exception ex) {
                throw new ApiCallbackJobException(callbackUrl, ex);
            }
        }
    }
}