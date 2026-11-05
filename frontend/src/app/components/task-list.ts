import {Component,inject,signal} from '@angular/core';
import {DatePipe} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {finalize} from 'rxjs';
import {Tarea,Entrada} from '../models/tarea';
import {TareasService} from '../services/tareas';
import {TaskFormComponent} from './task-form';
@Component({selector:'app-task-list',imports:[DatePipe,FormsModule,TaskFormComponent],template:`
<header class="page-heading"><div><p class="eyebrow">TU ESPACIO PERSONAL</p><h1>Mis tareas</h1><p class="subtitle">Organiza tus pendientes. Avanza a tu ritmo.</p></div><button class="primary" (click)="abrir()" [disabled]="ocupado()">+ Nueva tarea</button></header>
@if(mensaje()){<p class="notice" role="status">{{mensaje()}}</p>}
@if(error()){<div class="error-banner" role="alert">{{error()}} <button (click)="cargar()">Reintentar</button></div>}
@if(formVisible()){<app-task-form [tarea]="seleccionada()" [ocupado]="ocupado()" (guardar)="guardar($event)" (cancelar)="formVisible.set(false)"/>}
<section class="toolbar" aria-label="Filtros"><div class="search"><label for="buscar">Buscar tareas</label><input id="buscar" [(ngModel)]="buscar" (ngModelChange)="cargar()" placeholder="Título o descripción"></div><div><label for="estado">Estado</label><select id="estado" [(ngModel)]="estado" (change)="cargar()"><option value="">Todas</option><option value="pendiente">Pendientes</option><option value="completada">Completadas</option></select></div><div><label for="orden">Ordenar por</label><select id="orden" [(ngModel)]="orden" (change)="cargar()"><option value="fechaLimite">Fecha límite</option><option value="prioridad">Prioridad</option></select></div><button (click)="limpiar()">Limpiar filtros</button></section>
<div class="list-heading"><h2>Tus actividades</h2><span>{{tareas().length}} resultados</span></div>
@if(cargando()){<p role="status">Cargando tareas…</p>}
@if(!cargando() && !error() && !tareas().length){<section class="empty"><h2>No hay tareas para mostrar</h2><p>Crea una nueva tarea o ajusta los filtros.</p></section>}
<section class="task-list" aria-label="Lista de tareas">@for(t of tareas();track t.id){<article class="task" [class.done]="t.completada" [attr.data-id]="t.id">
<div class="task-main"><input type="checkbox" [checked]="t.completada" (change)="completar(t,$event)" [disabled]="ocupado()" [attr.aria-label]="'Completar '+t.titulo"><div><h3>{{t.titulo}}</h3><p class="description">{{t.descripcion}}</p><div class="meta"><span class="badge" [class.high]="t.prioridad===3">{{prioridades[t.prioridad]}}</span><span>{{t.fechaLimite ? (t.fechaLimite|date:'dd/MM/yyyy':'UTC'):'Sin fecha límite'}}</span><span>{{t.completada?'Completada':'Pendiente'}}</span></div></div></div>
<div class="actions"><button (click)="abrir(t)" [disabled]="ocupado()" [attr.aria-label]="'Editar '+t.titulo">Editar</button><button class="danger" (click)="borrar(t)" [disabled]="ocupado()" [attr.aria-label]="'Eliminar '+t.titulo">Eliminar</button></div></article>}</section>
`})
export class TaskListComponent {
 private api=inject(TareasService); tareas=signal<Tarea[]>([]);cargando=signal(false);ocupado=signal(false);error=signal('');mensaje=signal('');formVisible=signal(false);seleccionada=signal<Tarea|null>(null);buscar='';estado='';orden='fechaLimite';prioridades=['','Baja','Media','Alta'];private request=0;
 constructor(){this.cargar();}
 cargar(){const id=++this.request;this.cargando.set(true);this.error.set('');this.api.listar(this.estado,this.buscar,this.orden).subscribe({next:t=>{if(id===this.request){this.tareas.set(t);this.cargando.set(false);}},error:()=>{if(id===this.request){this.error.set('No se pudo conectar con la API. Intenta de nuevo.');this.cargando.set(false);this.tareas.set([]);}}});}
 limpiar(){this.buscar='';this.estado='';this.orden='fechaLimite';this.cargar();}
 abrir(t:Tarea|null=null){this.formVisible.set(false);this.seleccionada.set(t);setTimeout(()=>this.formVisible.set(true));this.mensaje.set('');}
 guardar(d:Entrada){this.ocupado.set(true);this.error.set('');this.api.guardar(d,this.seleccionada()?.id).pipe(finalize(()=>this.ocupado.set(false))).subscribe({next:()=>{this.formVisible.set(false);this.mensaje.set('Tarea guardada correctamente.');this.cargar();},error:()=>this.error.set('No se pudo guardar. Revisa los datos y vuelve a intentar.')});}
 completar(t:Tarea,event:Event){(event.target as HTMLInputElement).checked=t.completada;this.ocupado.set(true);const d:Entrada={titulo:t.titulo,descripcion:t.descripcion,prioridad:t.prioridad,fechaLimite:t.fechaLimite?.slice(0,10)??null,completada:!t.completada};this.api.guardar(d,t.id).pipe(finalize(()=>this.ocupado.set(false))).subscribe({next:()=>{this.mensaje.set('Estado actualizado.');this.cargar();},error:()=>{this.error.set('No se pudo actualizar el estado.');this.tareas.update(ts=>ts.map(x=>({...x})));}});}
 borrar(t:Tarea){if(!confirm('¿Eliminar la tarea «'+t.titulo+'»?'))return;this.ocupado.set(true);this.api.eliminar(t.id).pipe(finalize(()=>this.ocupado.set(false))).subscribe({next:()=>{this.mensaje.set('Tarea eliminada.');this.cargar();},error:()=>this.error.set('No se pudo eliminar la tarea.')});}
}

