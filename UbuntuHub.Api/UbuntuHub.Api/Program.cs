using Microsoft.EntityFrameworkCore;
using UbuntuHub.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UbuntuHubDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("UbuntuHubDatabase")
    )
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UbuntuHubDbContext>();

    if (!db.Posts.Any())
    {
        db.Posts.AddRange(
            new UbuntuHub.Api.Models.Post
            {
                UserId = 1,
                Title = "Community Clean-Up",
                Description = "Join us this Saturday to help clean our local park.",
                Location = "Johannesburg, Gauteng",
                Category = "Community"
            },
            new UbuntuHub.Api.Models.Post
            {
                UserId = 1,
                Title = "Food Donation Drive",
                Description = "We are collecting non-perishable food items for local families.",
                Location = "Soweto, Gauteng",
                Category = "Donations"
            },
            new UbuntuHub.Api.Models.Post
            {
                UserId = 1,
                Title = "Free Tutoring",
                Description = "Volunteer tutors are available to help school learners.",
                Location = "Pretoria, Gauteng",
                Category = "Education"
            }
        );

        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
