using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment()) {
    builder.Services.AddSingleton<ICounter, InMemoryCounter>();
    builder.Services.AddSingleton<IUrlMapper, InMemoryUrlMapper>();
}
else {
    // add configurations for real ICounter and IUrlMapper persistence mechanisms here
}

builder.Services.AddSingleton<IHasher, HashIdsDotNet>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/shorten", async ([FromBody]string url, ICounter counter, IHasher hasher, IUrlMapper urlMapper) => {
    if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        return Results.BadRequest($"The url {url} provided is not valid");

    var count = await counter.GetNext();
    var hash = hasher.GenerateHash(count);
    await urlMapper.Save(hash, url);

    return Results.Ok(hash);
});

app.MapGet("/{hash}", async (string hash, IUrlMapper urlMapper) => {
    var url = await urlMapper.Get(hash);

    if (url == null)
        return Results.Redirect("/", true);

    return Results.Redirect(url, true);
});

app.Run();