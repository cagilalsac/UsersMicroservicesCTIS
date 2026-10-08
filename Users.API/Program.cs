using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC (Inversion of Control) container.
builder.Services.AddDbContext<DbContext, UsersDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString(nameof(UsersDb))
));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
