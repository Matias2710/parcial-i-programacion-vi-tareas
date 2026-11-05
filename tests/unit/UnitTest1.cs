using System.ComponentModel.DataAnnotations;using MongoDB.Bson;using Tareas.Api.DTOs;using Tareas.Api.Models;using Tareas.Api.Controllers;
namespace Unit;
public class Reglas {
 [Theory][InlineData("")][InlineData(" ")]public void Titulo_requerido(string titulo){var d=new TareaEntrada{Titulo=titulo};Assert.False(Validator.TryValidateObject(d,new ValidationContext(d),new List<ValidationResult>(),true));}
 [Theory][InlineData(0)][InlineData(4)]public void Prioridad_invalida(int p){var d=new TareaEntrada{Titulo="Tarea",Prioridad=p};Assert.False(Validator.TryValidateObject(d,new ValidationContext(d),new List<ValidationResult>(),true));}
 [Fact]public void Bson_y_id(){var t=new Tarea{Titulo="Prueba"};Assert.True(ObjectId.TryParse(t.Id,out _));var b=t.ToBsonDocument();Assert.True(b["_id"].IsObjectId);Assert.Equal(t.Id,MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Tarea>(b).Id);}
 [Fact]public void Fecha_y_actualizacion(){var t=new Tarea();var fecha=t.FechaCreacion;var id=t.Id;TareasController.Aplicar(t,new TareaEntrada{Titulo="  Nuevo  ",Descripcion=" x ",Completada=true,FechaLimite=new DateOnly(2028,2,29),Prioridad=3});Assert.Equal("Nuevo",t.Titulo);Assert.True(t.Completada);Assert.Equal(DateTimeKind.Utc,t.FechaLimite!.Value.Kind);Assert.Equal(fecha,t.FechaCreacion);Assert.Equal(id,t.Id);Assert.Null(TareasController.Fecha(null));}
}
