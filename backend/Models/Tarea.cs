using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Tareas.Api.Models;
public sealed class Tarea {
 [BsonId, BsonRepresentation(BsonType.ObjectId)] public string Id {get;set;} = ObjectId.GenerateNewId().ToString();
 public string Titulo {get;set;} = "";
 public string Descripcion {get;set;} = "";
 public DateTime? FechaLimite {get;set;}
 public int Prioridad {get;set;} = 2;
 public bool Completada {get;set;}
 public static DateTime Ahora() => DateTimeOffset.FromUnixTimeMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()).UtcDateTime;
 public DateTime FechaCreacion {get;set;} = Ahora();
 public DateTime FechaActualizacion {get;set;} = Ahora();
}
