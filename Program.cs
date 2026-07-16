using Cargo_Management_Project.Data;
using Cargo_Management_Project.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register service layer dependencies
builder.Services.AddScoped<IShipmentBookingService, ShipmentBookingService>();
builder.Services.AddScoped<IFreightInvoiceService, FreightInvoiceService>();
builder.Services.AddScoped<ITrackingService, TrackingService>();

// Configure database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
