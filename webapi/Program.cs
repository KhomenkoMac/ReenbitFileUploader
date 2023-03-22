using api;
using Azure.Identity;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(_ => new BlobServiceClient(new Uri("https://uploaderapp1storage.blob.core.windows.net"), new DefaultAzureCredential(new DefaultAzureCredentialOptions
{
    ManagedIdentityClientId = "b9b369dd-f686-4e50-b073-dd5cd6648aa2"
})));
builder.Services.AddScoped<AzureDocxUploaderService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicySetup => corsPolicySetup.AllowAnyMethod()
                                              .AllowAnyHeader()
                                              .AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
