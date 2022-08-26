# Introduction

Shortly - a url shortner fit for Twitter in 2006!

# Getting up and running

Requires .NET 6

- Clone the repository
- Call ```dotnet test src/Shortly.Tests``` to ensure all tests are passing
- Call ```dotnet run --project src/Shortly```
- Development mode is entirely in-memory, so shorten away!

# Features and assumptions

Features:
- Client side and server side validation that URLs are well formed
- Hashing library used will start with smaller hash codes
- Hashing library is salted so that next hash values are unpredictable
- If you attempt to go to an invalid shortened url, you are directed back to the shortly page
- Shortening the same url multiple times will result in different shortened urls
- Only the hash is persisted and not the full redirect url, so the redirect url can change if Shortly is a bad name

URL Rules:
- URLs must be well formed
- Only absolute URLs are allowed

URL Rules for consideration in the future:
- Any domain is allowed (we could blacklist)
- Any protocol is allowed (and doesn't only accept https for example)

Risks
- The shortening endpoint can be protected by CORS, but still could be called by third parties. We can put a secret that is POSTed from the client to protect it.
- You could shorten two urls and cause a loop. You could blacklist url shortening domains. And/or add a rate limiter.

Nice to haves:
- A little button to copy the link to the clipboard

# Implementation

I wanted to go for a simple static website with a couple of endpoints for shortening and redirecting. I decided to go for the ASP.NET Core 6 Minimal Hosting model. (see ```Program.cs```)

Everytime a url is shortened, a counter is incremented which is hashed, resulting in a unique url.

## Persistence

Persistence involves storing a long for the counter (see ```ICounter.cs```) and a key/value lookup (see ```IUrlMapper.cs```).

They are currently hardcoded to use appropriate C# structures. It would be very easy to decide on a real database such as Redis to permanently persist, so long as you inherit the interfaces and map them for production.

## The Future

I could see this static page hosted on a CDN, and the shortening and redirecting endpoints as two serverless functions.

## Testing

This implementation doesn't have logic that requires unit testing as it's all orchestration code.

I have created Integration Tests instead that confirm the parts of the backend work together as intended.

I would like to put full end-to-end browser tests covering the critical path to ensure the client code is working as expected.