using System.Net;
using Domain;
using Microsoft.AspNetCore.Http.HttpResults;

var websites = new List<WebsiteDto>();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEnumerable<WebsiteDto>>(_ => websites);
builder.Services.AddScoped<ICollection<WebsiteDto>>(_ => websites);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(_ => _.AddPolicy("All", p => p
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseWebsitesAPI();
app.MapHub<WebsitesHub>("/api/notifications");
app.UseCors("All");

app.Run();
