using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Cấu hình Controller và chống lỗi vòng lặp JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Cấu hình Swagger cho cả Dev và Production
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";
});

// 4. TỰ ĐỘNG CHUYỂN HƯỚNG VỀ SWAGGER KHI VÀO TRANG CHỦ
app.MapGet("/", context => {
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// 5. QUAN TRỌNG: Tắt HttpsRedirection vì Somee Free thường chỉ chạy HTTP
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();