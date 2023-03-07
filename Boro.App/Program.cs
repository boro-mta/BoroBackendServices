using Boro.Logging.DependencyInjection;
using ItemService.Controller.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.AddBoroLogging(Path.Combine(Environment.CurrentDirectory, "Logs"));
// Add services to the container.
builder.Services.AddItemService();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseBoroLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
