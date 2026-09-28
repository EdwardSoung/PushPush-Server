using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushPushServer.Data;
using PushPushServer.DTO;
using PushPushServer.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var message = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

            return new OkObjectResult(new BaseResponse
            {
                ResultCode = ResultCode.InvalidRequest,
                Message = message,
            });
        };
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connStr = builder.Configuration.GetConnectionString("GameDB");
builder.Services.AddDbContext<GameDBContext>(opt =>
    opt.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

builder.Services.AddSingleton<SessionService>();
builder.Services.AddScoped<UserService>();   // DbContext를 쓰므로 Scoped
builder.Services.AddScoped<ItemService>();

var app = builder.Build();

app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync(new BaseResponse
    {
        ResultCode = ResultCode.ServerError,
        Message = "서버 오류가 발생했습니다.",
    });
}));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
