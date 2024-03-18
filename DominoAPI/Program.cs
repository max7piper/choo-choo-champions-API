using MongoDB.Driver;
using MongoDB.Bson;
using DominoAPI.UserObjects;
using DominoAPI.Services;
using DominoAPI.UserRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<UserDatabaseSettings>(
    builder.Configuration.GetSection("UserDatabase"));

builder.Services.AddSingleton<UserService>();
builder.Services.AddScoped<IUserRepository,UserRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
