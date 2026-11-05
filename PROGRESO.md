# Estado verificable

Actualización: 5 de noviembre de 2026. Proyecto independiente del trabajo anterior.

## Auditoría final

| Requisito | Estado y evidencia |
|---|---|
| Documento y rúbrica | Leído completo. MongoDB 3%; controladores 3%; LINQ 2%; Angular 7% (1+2+3+1). Matriz en REQUISITOS.md. |
| Backend .NET y controladores | Compilado; cinco endpoints CRUD, DTO, validación, ObjectId, errores y campos internos protegidos. |
| MongoDB real | Instancia local aislada 8.0.34 para pruebas; Atlas en base nueva ParcialITareas, colección Tareas. |
| Atlas | 5 comprobaciones reales: listado, POST/BSON, PUT/persistencia, LINQ combinado, DELETE/directo. Tres documentos demo al finalizar. Carga repetida conserva 3. evidencias/atlas.json. |
| LINQ | AsQueryable en controlador; filtro por estado, orden por fecha/prioridad y búsqueda por título/descripción; combinación probada. |
| Angular | AppComponent, TaskListComponent, formulario reactivo y servicio HTTP. Build de producción aprobado. |
| CRUD desde navegador | Creación, edición, completar, cancelar/confirmar eliminación, recargas; lecturas directas de MongoDB. |
| Persistencia integral | Cada tamaño de pantalla crea, edita, completa, reinicia backend, recarga y elimina; verifica MongoDB directamente. |
| Validaciones y fallos | API valida título, prioridad, fecha, ObjectId y campos desconocidos. Frontend muestra validación, error real de red y recuperación. Una actualización fallida conserva la casilla en su estado persistido. |
| Scalar | Cinco operaciones documentadas y modelos OpenAPI; interfaz visible, solicitud GET enviada desde cliente Scalar y HTTP 200 comprobado. |
| Pruebas | 6 unitarias aprobadas. 5 escenarios integración/E2E aprobados, 0 fallos/omitidos; evidencias/resultados.json. Informe detallado local playwright.json y TRX excluidos de Git por rutas del equipo. |
| Responsive | Flujos completos a 375x812, 768x1024 y 1366x768. Sin desbordamiento horizontal; capturas inspeccionadas. |
| Evidencias | Tres capturas exigidas y adicionales de responsive/Scalar, todas obtenidas con Chromium real. |
| Desde cero | Dependencias reinstaladas con npm ci, build y unitarias correctos; batería final arranca sus propios servicios, limpia solo ParcialI_Automated y los detiene. |
| Documentación | README con ejecución independiente de secretos de sesión, endpoints, reglas, pruebas y capturas. |
| GitHub | Publicado y verificado público: https://github.com/Matias2710/parcial-i-programacion-vi-tareas. Código de referencia bb6f0d250ff369fd44370cd75bc69900d54f8c99; remoto comprobado. |
| Presentación académica | No enviada a Moodle. El documento no confirma permiso de IA y contiene fecha contradictoria; no se presume autorización para presentar el parcial. |

## Correcciones verificadas

- Actualizado OpenAPI de la plantilla para eliminar la advertencia de vulnerabilidad de Microsoft.OpenApi 2.0.0.
- Normalizadas fechas internas a milisegundos, precisión que BSON realmente conserva; POST y GET mantienen el mismo valor.
- Casilla de completar muestra el nuevo estado únicamente después de confirmación del servidor.
- Arnés de pruebas espera MongoDB y lo apaga explícitamente. Detener un proceso ya terminado por señal no queda esperando indefinidamente.
- Corregido el patrón del test de fallo de red para incluir rutas por ID; no se relajaron expectativas para ocultar fallos.
- La primera instalación npm buscó accidentalmente el manifiesto del directorio personal; las dos dependencias agregadas allí fueron retiradas. Las dependencias del proyecto están en sus manifiestos locales y no se requieren instalaciones globales.

## Límites y condiciones

Portada con III cuatrimestre 2026 y fecha 26/06/2025; fecha vigente no confirmada. No consta prohibición explícita de IA en el DOCX, pero tampoco permiso específico para rendir el parcial con ella. Desarrollo con asistencia declarada; presentación académica fuera de esta entrega automática.

Aplicación personal en loopback. No incorpora cuentas multiusuario ni despliegue público. No son requisitos del enunciado. Los datos son ficticios; secretos de Atlas fuera del repositorio. No se modificó la colección Ventas del proyecto anterior.

## Cierre

Aplicación disponible en http://127.0.0.1:4205, API en 5095 conectada a Atlas. Se comprobaron las tres tareas demo desde Angular (05-lista-atlas.png). Servicios de prueba detenidos y base temporal limpiada. Quedan activos los servicios de demostración para revisión; detenerlos antes de ejecutar Verificar.ps1 en los mismos puertos.

No quedan requisitos técnicos o de publicación pendientes. La autorización docente y fecha vigente de presentación no están confirmadas; no se realizó entrega académica.

