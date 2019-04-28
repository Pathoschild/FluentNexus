using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Pathoschild.Http.Client;

namespace Pathoschild.FluentNexus.Framework
{
    /// <summary>Provides extension methods for internal client code.</summary>
    internal static class InternalExtensions
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Configure an async task so it's safe to use in a synchronous context with <see cref="Task{TResult}.Result"/>.</summary>
        /// <typeparam name="T">The task return type.</typeparam>
        /// <param name="task">The task to extend.</param>
        public static ConfiguredTaskAwaitable<T> MakeSyncSafe<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }

        /// <summary>Configure an async task so it's safe to use in a synchronous context with <see cref="Task.Wait()"/>.</summary>
        /// <param name="request">The request to extend.</param>
        public static ConfiguredTaskAwaitable<IResponse> MakeSyncSafe(this IRequest request)
        {
            return request.AsResponse().ConfigureAwait(false);
        }
    }
}
