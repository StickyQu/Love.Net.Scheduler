using System;

namespace Love.Net.Scheduler.Jobs {
    //[Serializable]
    public class ApiCallbackJobException : Exception {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ApiCallbackJobException() {
        }

        public ApiCallbackJobException(string message) : base(message) {
        }

        public ApiCallbackJobException(string message, Exception inner) : base(message, inner) {
        }

        ///// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        //protected ApiCallbackJobException(SerializationInfo info, StreamingContext context) : base(info, context) {
        //}
    }
}