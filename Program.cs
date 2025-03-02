using Microsoft.EntityFrameworkCore; //Entity framwork for database access 
using BookReviewApi.Models; 
using BookReviewApi.Context;
using Microsoft.AspNetCore.Identity; // Identity frame work for user management and authentication
using BookReviewApi.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOpenApi(); // Open API for swagger documentation

builder.Services.AddControllers();// Registering controllers for API end points 

// Registers db context to use SQL Lite for EF to be able to interact with the databse - 
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Connection"))); 

// Registering identify framework for user authentication,roles and JWT tokens.
builder.Services.AddIdentity<IdentityUser, IdentityRole>() // EF stores user information
    .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders(); // Genration tokens for 

// Registering email settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings")); 
builder.Services.AddScoped<EmailService>(); // Email service used as a dependency 

builder.Services.AddScoped<RolesController>(); // Registering roles controller for do roles management as a dependency injection 

//Registering jwt authenticiation - ensure suers are authenticated befoe accessing 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token is from a valid user 
            ValidateAudience = true, // Token is from a valid user 
            ValidateLifetime = true, // Making sure token is not expired 
            ValidateIssuerSigningKey = true, // Making sure token has a valid signing key
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Validate issuer 
            ValidAudience = builder.Configuration["Jwt:Issuer"], 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) //using secret key from appsettings.json to validate token
        };
    });

//Registering services - dependency injections 
//Inject services whenever interfaces are required 
builder.Services.AddScoped<IGenreService, GenreService>(); 
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IBookGenreService, BookGenreService>();

//Swagger configuration 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BookReview API",
        Description = "An ASP.NET Core Web API for managing reviews for books by users of the application",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Zara Khan",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

//Include XML Documentation

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build(); // Build the application

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapControllers(); //map api endpoints controllers
app.UseAuthentication(); 
app.UseAuthorization();

app.Run(); // Start application

