**FluentNexus** is a modern async HTTP client for the Nexus Mods API. It gives you simple
strongly-typed methods to access all of the Nexus API endpoints, handling the gritty details like
deserialisation, content negotiation, URL encoding, and error-handling.

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
(or an [SSO key](https://github.com/Nexus-Mods/sso-integration-demo)), and an arbitrary app
name/version which is just stored by Nexus for tracking:
```c#
var nexus = new NexusClient("api key here", "My App Name", "1.0.0");
```

### Basic examples
Now just call its methods to interact with the Nexus API! 

For example...

* Start tracking a mod:
  ```c#
  await nexus.Users.TrackMod("stardewvalley", 2400);
  ```
* Get a list of files for a mod:
  ```c#
  ModFileList files = await nexus.ModFiles.GetModFiles("stardewvalley", 2400);
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
   ModFileDownloadLink[] links = await nexus.ModFiles.GetDownloadLinks("stardewvalley", 2400, 9622);
}
catch (ApiException ex)
{
   if (ex.Status == HttpStatusCode.Forbidden && ex.Message.Contains("this is for premium users only"))
      // user needs a premium account
}
```

## Advanced
### Rate limits
The Nexus API sets some rate limits (see [API docs](https://app.swaggerhub.com/apis-docs/NexusMods/nexus-mods_public_api_params_in_form_data/1.0)),
and will return HTTP 429 if you exceed them.

You can check your rate limit usage anytime. Since this is cached on each response, this will only
ping the Nexus API if you haven't sent a request recently. (Even if it does ping the API, this
won't be counted against your limits.) To check the rate limit values:

```c#
IRateLimitManager limits = await nexus.GetRateLimits();
Console.WriteLine($"Daily usage: {limits.DailyRemaining}/{limits.DailyLimit} requests left, will reset at {limits.DailyReset}.");
Console.WriteLine($"Hourly usage: {limits.HourlyRemaining}/{limits.HourlyLimit} requests left, will reset at {limits.HourlyReset}.");
```

The client can also help you automate rate limit handling. For example, let's say you want
to pause the script when you exceed the rate limits but resume when they renew:
```c#
IRateLimitManager rateLimits = await nexus.GetRateLimits();
if (rateLimits.IsBlocked())
{
   TimeSpan renewDelay = rateLimits.GetTimeUntilRenewal();
   Console.WriteLine($"Exceeded rate limits, will resume at {DateTime.Now + renewDelay} local time.");
   Thread.Sleep(renewDelay);
}
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
nexus.Users.TrackMod("stardewvalley", 2400).Wait();
```

[.NET Standard]: https://docs.microsoft.com/en-us/dotnet/articles/standard/library
[Parallel Programming with .NET: Await, and UI, and deadlocks! Oh my!]: https://blogs.msdn.microsoft.com/pfxteam/2011/01/13/await-and-ui-and-deadlocks-oh-my/
[Don't Block on Async Code]: https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
[media type formatters]: https://www.nuget.org/packages?q=MediaTypeFormatter

[AggregateException]: https://docs.microsoft.com/en-us/dotnet/api/system.aggregateexception

