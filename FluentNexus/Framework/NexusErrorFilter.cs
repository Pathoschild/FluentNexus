using Pathoschild.FluentNexus.Models;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;

namespace Pathoschild.FluentNexus.Framework
{
    /// <summary>An HTTP filter which parses Nexus API errors.</summary>
    internal class NexusErrorFilter : IHttpFilter
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
        /// <param name="request">The HTTP request.</param>
        public void OnRequest(IRequest request) { }

        /// <summary>Method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.</summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="httpErrorAsException">Whether HTTP error responses (e.g. HTTP 404) should be raised as exceptions.</param>
        public void OnResponse(IResponse response, bool httpErrorAsException)
        {
            if (!httpErrorAsException || response.Message.IsSuccessStatusCode)
                return;

            GenericError error = this.GetError(response);
            throw !string.IsNullOrWhiteSpace(error?.Message)
                ? new ApiException(response, error.Message)
                : new ApiException(response, $"The Nexus API returned status code {response.Message.StatusCode}: {response.Message.ReasonPhrase}");
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the generic Nexus error from an HTTP response, if available.</summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>Returns the error if applicable, else <c>null</c>.</returns>
        private GenericError GetError(IResponse response)
        {
            try
            {
                return response.As<GenericError>().Result;
            }
            catch
            {
                return null;
            }
        }
    }
}
