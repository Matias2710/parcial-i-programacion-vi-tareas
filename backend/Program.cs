using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using Tareas.Api.Services;
var b=WebApplication.CreateBuilder(args);
b.Services.AddControllers().AddJsonOptions(o=>o.JsonSerializerOptions.UnmappedMemberHandling=JsonUnmappedMemberHandling.Disallow);
b.Services.AddOpenApi();b.Services.AddProblemDetails();b.Services.AddSingleton<TareasStore>();
var app=b.Build();
app.UseExceptionHandler();
if(app.Environment.IsDevelopment()){app.MapOpenApi();app.MapScalarApiReference();}
app.MapControllers();
if(args.Contains("--seed")){await app.Services.GetRequiredService<TareasStore>().Sembrar();return;}
app.Run();
public partial class Program {}
