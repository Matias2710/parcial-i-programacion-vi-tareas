using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tareas.Api.DTOs;
using Tareas.Api.Models;
using Tareas.Api.Services;
namespace Tareas.Api.Controllers;
[ApiController, Route("api/tareas")]
public sealed class TareasController(TareasStore store):ControllerBase {
 [HttpGet]
 [ProducesResponseType<List<Tarea>>(200)]
 public async Task<IActionResult> Listar(string? estado=null,string? buscar=null,string ordenarPor="fechaLimite",CancellationToken ct=default){
  if(estado is not null and not "" and not "pendiente" and not "completada" || ordenarPor is not "fechaLimite" and not "prioridad" || buscar?.Length>200) return Problem(statusCode:400,detail:"Filtro u orden inválido.");
  var q=store.Coleccion.AsQueryable();
  if(!string.IsNullOrEmpty(estado)) q=q.Where(t=>t.Completada==(estado=="completada"));
  if(!string.IsNullOrWhiteSpace(buscar)){var b=buscar.Trim().ToLowerInvariant();q=q.Where(t=>t.Titulo.ToLower().Contains(b)||t.Descripcion.ToLower().Contains(b));}
  q=ordenarPor=="prioridad"?q.OrderByDescending(t=>t.Prioridad).ThenBy(t=>t.Id):q.OrderBy(t=>t.FechaLimite==null).ThenBy(t=>t.FechaLimite).ThenBy(t=>t.Id);
  return Ok(await q.ToListAsync(ct));
 }
 [HttpGet("{id}")]
 [ProducesResponseType<Tarea>(200),ProducesResponseType(404),ProducesResponseType(400)]
 public async Task<IActionResult> Obtener(string id,CancellationToken ct){
  if(!ObjectId.TryParse(id,out _))return Problem(statusCode:400,detail:"Identificador inválido.");
  var t=await store.Coleccion.Find(t=>t.Id==id).FirstOrDefaultAsync(ct);return t is null?NotFound():Ok(t);
 }
 [HttpPost]
 [ProducesResponseType<Tarea>(201),ProducesResponseType(400)]
 public async Task<IActionResult> Crear(TareaEntrada d,CancellationToken ct){
  if(string.IsNullOrWhiteSpace(d.Titulo))return Problem(statusCode:400,detail:"El título es obligatorio.");
  var t=new Tarea();Aplicar(t,d);await store.Coleccion.InsertOneAsync(t,cancellationToken:ct);return CreatedAtAction(nameof(Obtener),new{id=t.Id},t);
 }
 [HttpPut("{id}")]
 [ProducesResponseType<Tarea>(200),ProducesResponseType(404),ProducesResponseType(400)]
 public async Task<IActionResult> Actualizar(string id,TareaEntrada d,CancellationToken ct){
  if(!ObjectId.TryParse(id,out _) || string.IsNullOrWhiteSpace(d.Titulo))return Problem(statusCode:400,detail:"Identificador o título inválido.");
  var update=Builders<Tarea>.Update.Set(t=>t.Titulo,d.Titulo.Trim()).Set(t=>t.Descripcion,d.Descripcion.Trim()).Set(t=>t.FechaLimite,Fecha(d.FechaLimite)).Set(t=>t.Prioridad,d.Prioridad).Set(t=>t.Completada,d.Completada).Set(t=>t.FechaActualizacion,Tarea.Ahora());
  var t=await store.Coleccion.FindOneAndUpdateAsync(v=>v.Id==id,update,new FindOneAndUpdateOptions<Tarea>{ReturnDocument=ReturnDocument.After},ct);
  return t is null?NotFound():Ok(t);
 }
 [HttpDelete("{id}")]
 [ProducesResponseType(204),ProducesResponseType(404),ProducesResponseType(400)]
 public async Task<IActionResult> Eliminar(string id,CancellationToken ct){
  if(!ObjectId.TryParse(id,out _))return Problem(statusCode:400,detail:"Identificador inválido.");
  return (await store.Coleccion.DeleteOneAsync(t=>t.Id==id,ct)).DeletedCount==0?NotFound():NoContent();
 }
 public static DateTime? Fecha(DateOnly? date)=>date?.ToDateTime(TimeOnly.MinValue,DateTimeKind.Utc);
 public static void Aplicar(Tarea t,TareaEntrada d){t.Titulo=d.Titulo.Trim();t.Descripcion=d.Descripcion.Trim();t.FechaLimite=Fecha(d.FechaLimite);t.Prioridad=d.Prioridad;t.Completada=d.Completada;t.FechaActualizacion=Tarea.Ahora();}
}

