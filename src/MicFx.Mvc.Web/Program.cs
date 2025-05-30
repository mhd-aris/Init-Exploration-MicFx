var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOrchardCore()
    .AddMvc();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseOrchardCore();

app.Run();

// Make the implicit Program class public for testing
public partial class Program { }
