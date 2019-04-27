using System;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;

namespace Pathoschild.FluentNexus.Framework
{
    /// <summary>An HTTP filter which raises an event when an HTTP response is received.</summary>
    internal class ResponseCallbackFilter : IHttpFilter
    {
        /*********
        ** Fields
        *********/
        /// <summary>The callback to invoke when a response is received.</summary>
        private readonly Action<IResponse> Callback;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="callback">The callback to invoke when a response is received.</param>
        public ResponseCallbackFilter(Action<IResponse> callback)
        {
            this.Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        /// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
        /// <param name="request">The HTTP request.</param>
        public void OnRequest(IRequest request) { }

        /// <summary>Method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.</summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="httpErrorAsException">Whether HTTP error responses (e.g. HTTP 404) should be raised as exceptions.</param>
        public void OnResponse(IResponse response, bool httpErrorAsException)
        {
            this.Callback(response);
        }
    }
}
