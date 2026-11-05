using System.ComponentModel.DataAnnotations;
namespace Tareas.Api.DTOs;
public sealed class TareaEntrada {
 [Required, StringLength(120,MinimumLength=1)] public string Titulo {get;set;} = "";
 [StringLength(2000)] public string Descripcion {get;set;} = "";
 public DateOnly? FechaLimite {get;set;}
 [Range(1,3)] public int Prioridad {get;set;} = 2;
 public bool Completada {get;set;}
}
