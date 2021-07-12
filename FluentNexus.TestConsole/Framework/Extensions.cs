using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FluentNexus.TestConsole.Framework
{
    /// <summary>Provides extensions for internal use.</summary>
    internal static class Extensions
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Dump a serialized representation of the data to the console.</summary>
        /// <param name="data">The data to dump.</param>
        /// <param name="label">A human-readable label for the data, if any.</param>
        /// <returns>Returns the input value for chaining.</returns>
        public static async Task<T> DumpAsync<T>(this Task<T> data, string label = null)
        {
            T result = await data;
            return result.Dump(label);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Dump a serialized representation of the data to the console.</summary>
        /// <param name="data">The data to dump.</param>
        /// <param name="label">A human-readable label for the data, if any.</param>
        /// <returns>Returns the input value for chaining.</returns>
        private static T Dump<T>(this T data, string label = null)
        {
            if (!string.IsNullOrWhiteSpace(label))
                Console.WriteLine(label);

            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;

                switch (data)
                {
                    case null:
                        Console.WriteLine("<null>");
                        break;

                    case string _:
                        Console.WriteLine(data);
                        break;

                    default:
                        Console.WriteLine(data.ToDisplayJson());
                        break;
                }
            }
            finally
            {
                Console.ResetColor();
            }

            Console.WriteLine();
            return data;
        }

        /// <summary>Get a JSON representation of the given data for display to the user.</summary>
        /// <param name="data">The data to format.</param>
        private static string ToDisplayJson(this object data)
        {
            // Use System.Text.Json instead of Json.NET so we can see the actual model shape.
            // Json.NET is too smart for our purposes here; e.g. it'll show extension data fields
            // as their own properties, making it harder to spot unmapped fields.
            return JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });
        }
    }
}
