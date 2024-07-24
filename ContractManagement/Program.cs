using ContractManagement.Services;
using ContractManagement.Controllers;
using Microsoft.Extensions.Azure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(new QueueStorageService(builder.Configuration["ConnectionStrings:queue"]));

builder.Services.AddSingleton(new BlobStorageService(builder.Configuration["ConnectionStrings:blob"]));
builder.Services.AddSingleton(new TableStorageService(builder.Configuration["ConnectionStrings:table"],"btcrud"));
//builder.Services.AddAzureClients(clientBuilder =>
//{
//    //clientBuilder.AddQueueServiceClient(builder.Configuration["ConnectionStrings:queuecs"]);
//    clientBuilder.AddTableServiceClient(builder.Configuration["ConnectionStrings:table"]);
//    clientBuilder.AddBlobServiceClient(builder.Configuration["ConnectionStrings:blob"]);

//});
//builder.Services.AddScoped<TableStorageService>();
//builder.Services.AddScoped<BlobStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Contract",
    pattern: "{controller=Contract}/{action=Index}/{partitionKey?}/{rowKey?}",
    defaults: new { controller = "Contract" });

app.Run();
