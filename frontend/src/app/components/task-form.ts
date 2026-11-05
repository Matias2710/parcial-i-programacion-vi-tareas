import {Component, input,output,OnInit,inject} from '@angular/core';
import {FormBuilder,ReactiveFormsModule,Validators} from '@angular/forms';
import {Entrada,Tarea} from '../models/tarea';
@Component({selector:'app-task-form',imports:[ReactiveFormsModule],template:`
<section class="form-panel" aria-labelledby="form-title"><h2 id="form-title">{{tarea()?'Editar tarea':'Nueva tarea'}}</h2>
<form [formGroup]="form" (ngSubmit)="submit()">
<label for="titulo">Título <span aria-hidden="true">*</span></label><input id="titulo" formControlName="titulo" maxlength="120" autofocus>
@if(form.controls.titulo.touched && form.controls.titulo.invalid){<p class="error" role="alert">Escribe un título de 1 a 120 caracteres.</p>}
<label for="descripcion">Descripción</label><textarea id="descripcion" formControlName="descripcion" maxlength="2000" rows="3"></textarea>
<div class="form-grid"><div><label for="fecha">Fecha límite</label><input id="fecha" type="date" formControlName="fechaLimite"></div>
<div><label for="prioridad">Prioridad</label><select id="prioridad" formControlName="prioridad"><option [ngValue]="1">Baja</option><option [ngValue]="2">Media</option><option [ngValue]="3">Alta</option></select></div></div>
<div class="actions"><button class="primary" type="submit" [disabled]="ocupado()">{{ocupado()?'Guardando…':'Guardar tarea'}}</button><button type="button" (click)="cancelar.emit()" [disabled]="ocupado()">Cancelar</button></div></form></section>`})
export class TaskFormComponent implements OnInit {
 tarea=input<Tarea|null>(null);ocupado=input(false);guardar=output<Entrada>();cancelar=output<void>();private fb=inject(FormBuilder);
 form=this.fb.nonNullable.group({titulo:['',[Validators.required,Validators.maxLength(120),Validators.pattern(/.*\S.*/)]],descripcion:['',Validators.maxLength(2000)],fechaLimite:[''],prioridad:[2,[Validators.min(1),Validators.max(3)]]});
 ngOnInit(){const t=this.tarea();if(t)this.form.patchValue({...t,fechaLimite:t.fechaLimite?.slice(0,10)??''});}
 submit(){this.form.markAllAsTouched();if(this.form.invalid||this.ocupado())return;const d=this.form.getRawValue();this.guardar.emit({...d,titulo:d.titulo.trim(),fechaLimite:d.fechaLimite||null,completada:this.tarea()?.completada??false});}
}
