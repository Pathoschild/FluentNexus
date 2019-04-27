**FluentNexus** is a modern async HTTP client for the Nexus Mods API. It provides simplified
methods for querying and submitting requests to Nexus Mods, hiding away the gritty details like
deserialisation, content negotiation, and URL encoding.

Designed with discoverability and extensibility as core principles, just autocomplete to see which
methods are available at each step.

## Install
Install it [from NuGet](https://nuget.org/packages/Pathoschild.FluentNexus):
> Install-Package Pathoschild.FluentNexus

The client works on any modern platform (including Linux, Mac, and Windows):

| platform                    | min version |
| :-------------------------- | :---------- |
| .NET Core                   | 1.0         |
| .NET Framework              | 4.5         |
| [.NET Standard][]           | 1.3         |
| Mono                        | 4.6         |
| Unity                       | 2018.1      |
| Universal Windows Platform  | 10.0        |
| Xamarin.Android             | 7.0         |
| Xamarin.iOS                 | 10.0        |
| Xamarin.Mac                 | 3.0         |

## Use
### Init a client
Create the client with your [personal API key](https://www.nexusmods.com/users/myaccount?tab=api)
(or an [SSO key](https://github.com/Nexus-Mods/sso-integration-demo)), and set the [user
agent](https://en.wikipedia.org/wiki/User_agent#Format_for_automated_agents_(bots)) you want to use:
```c#
var nexus = new FluentNexus("api key here").SetUserAgent("YourProjectName/1.0.0 (+url)");
```

(The user agent will default to something like `FluentNexus/1.0.0 (+http://github.com/Pathoschild/FluentNexus)`
if you don't set it.)

### Basic examples
Now just call its methods to interact with the Nexus API! 

For example...

* Start tracking a mod:
  ```c#
  await nexus.Users.TrackMod("stardewvalley", 2400);
  ```
* Get a list of files for a mod:
  ```c#
  ModFileList[] files = await nexus.ModFiles.GetModFiles("stardewvalley", 2400);
  ```

* Get download links for a mod file:
   ```c#
   ModFileDownloadLink[] downloadLinks = await nexus.ModFiles.GetDownloadLinks("stardewvalley", 2400, 9622);
   ```
* Which mods are being tracked by the user?
  ```c#
  UserTrackedMod[] mods = await nexus.Users.GetTrackedMods();
  ```

See the [Nexus API docs](https://app.swaggerhub.com/apis-docs/NexusMods/nexus-mods_public_api_params_in_form_data/1.0)
for a list of endpoints and fields supported by this client.

### Error-handling
The Nexus API returns errors in this format:
```js
{
  "code": 403,
  "message": "You don't have permission to get download links from the API without visting nexusmods.com - this is for premium users only."
}
```

The client will throw an `ApiException` when that happens, which contains the HTTP status code,
Nexus error message, and response:
```c#
try
{
   string downloadLink = await nexus.ModFiles.GetDownloadLink(...);
}
catch (ApiException ex)
{
   if (ex.Status == 403 && ex.Message.Contains("this is for premium users only"))
      // user needs a premium account
}
```

## Advanced
### Rate limits
The Nexus Mods API imposes some rate limits (see [API docs](https://app.swaggerhub.com/apis-docs/NexusMods/nexus-mods_public_api_params_in_form_data/1.0)).
They're high enough that you don't need to worry about them in most cases, but you can check your
current rate limit usage after making a request:

```c#
RequestMetadata meta = nexus.GetLastRequestMetadata();
Console.WriteLine($"Daily usage: {meta.DailyRemaining}/{meta.DailyLimit} requests left, will reset at {meta.DailyReset}.");
Console.WriteLine($"Hourly usage: {meta.HourlyRemaining}/{meta.HourlyLimit} requests left, will reset at {meta.HourlyReset}.");
Console.WriteLine($"The last request took {meta.LastRequestRuntime} seconds of server time.");
```

### Underlying HTTP client
If needed, you can use the underlying fluent HTTP client directly (e.g. for an unsupported endpoint):

```c#
Mod mod = await nexus.HttpClient
   .GetAsync($"v1/games/{domainName}/mods/{modID}.json")
   .As<Mod>();
```

See [FluentHttpClient](https://github.com/Pathoschild/FluentHttpClient/) for documentation.

### Synchronous use
The client is built around the `async` and `await` keywords, but you can use the client
synchronously. That's not recommended — it complicates error-handling (e.g. errors get wrapped
into [AggregateException][]), and it's very easy to cause thread deadlocks when you do this (see
_[Parallel Programming with .NET: Await, and UI, and deadlocks! Oh my!][]_ and
_[Don't Block on Async Code][])._

If you really need to use it synchronously, you can just call the `Result` property:
```c#
UserTrackedMod[] mods = nexus.Users.GetTrackedMods().Result;
```

Or if you don't need the response:

```c#
nexus.Users.TrackMod("stardewvalley", 2400).AsResponse().Wait();
```

[.NET Standard]: https://docs.microsoft.com/en-us/dotnet/articles/standard/library
[Parallel Programming with .NET: Await, and UI, and deadlocks! Oh my!]: https://blogs.msdn.microsoft.com/pfxteam/2011/01/13/await-and-ui-and-deadlocks-oh-my/
[Don't Block on Async Code]: https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
[media type formatters]: https://www.nuget.org/packages?q=MediaTypeFormatter

[AggregateException]: https://docs.microsoft.com/en-us/dotnet/api/system.aggregateexception

