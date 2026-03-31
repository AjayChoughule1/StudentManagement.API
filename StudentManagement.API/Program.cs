using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Infrastructure.Data;
using StudentManagement.API.Domain.Services;
using StudentManagement.API.Domain.Interfaces;
using StudentManagement.API.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (?? remove duplicate)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});


// ? ADD THESE (IMPORTANT ??)

// Service Registration
builder.Services.AddScoped<TeacherService>();

// If using UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// If using repositories (example)
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();


var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();