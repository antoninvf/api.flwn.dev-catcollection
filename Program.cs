var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "flwn",
        policy =>
        {
            policy.WithOrigins
            ("https://api.flwn.dev",
                "https://flwn.dev",
                "https://cat.flwn.dev",
                "https://basil.florist",
                "https://cat.basil.florist");
        });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();