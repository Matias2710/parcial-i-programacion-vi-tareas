import {inject,Injectable} from '@angular/core';
import {HttpClient,HttpParams} from '@angular/common/http';
import {Tarea,Entrada} from '../models/tarea';
@Injectable({providedIn:'root'}) export class TareasService {
 private http=inject(HttpClient);
 listar(estado:string,buscar:string,ordenarPor:string){return this.http.get<Tarea[]>('/api/tareas',{params:new HttpParams().set('estado',estado).set('buscar',buscar).set('ordenarPor',ordenarPor)});}
 guardar(d:Entrada,id?:string){return id?this.http.put<Tarea>('/api/tareas/'+id,d):this.http.post<Tarea>('/api/tareas',d);}
 eliminar(id:string){return this.http.delete('/api/tareas/'+id);}
}
