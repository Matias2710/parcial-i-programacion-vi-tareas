using MongoDB.Driver;
using Tareas.Api.Models;
namespace Tareas.Api.Services;
public sealed class TareasStore {
 public IMongoCollection<Tarea> Coleccion {get;}
 public TareasStore(IConfiguration c) {
  var client = new MongoClient(c["Mongo:Uri"] ?? "mongodb://127.0.0.1:27027");
  Coleccion = client.GetDatabase(c["Mongo:Database"] ?? "ParcialITareas").GetCollection<Tarea>("Tareas");
 }
 public async Task Sembrar() {
  var rows = new[]{("Preparar presentación", "Revisar los ejemplos de LINQ",3,false), ("Comprar materiales", "Cuaderno y marcadores",1,false), ("Repasar Angular", "Formularios reactivos de universidad",2,true)};
  for(var i=0;i<rows.Length;i++){
   var x=rows[i];var t=new Tarea{Id=$"00000000000000000000000{i+1}",Titulo=x.Item1,Descripcion=x.Item2,Prioridad=x.Item3,Completada=x.Item4,FechaLimite=i==1?null:DateTime.UtcNow.Date.AddDays(i+1)};
   await Coleccion.UpdateOneAsync(v=>v.Id==t.Id,Builders<Tarea>.Update.SetOnInsert(v=>v.Titulo,t.Titulo).SetOnInsert(v=>v.Descripcion,t.Descripcion).SetOnInsert(v=>v.Prioridad,t.Prioridad).SetOnInsert(v=>v.Completada,t.Completada).SetOnInsert(v=>v.FechaLimite,t.FechaLimite).SetOnInsert(v=>v.FechaCreacion,t.FechaCreacion).SetOnInsert(v=>v.FechaActualizacion,t.FechaActualizacion),new UpdateOptions{IsUpsert=true});
  }
 }
}
