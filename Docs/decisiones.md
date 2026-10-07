# TaskFlow — Registro de decisiones

Este archivo registra las decisiones de diseño tomadas y **por qué** se tomaron.
Cuando una decisión cambie, no se borra: se marca como reemplazada y se añade la nueva.

Formato de cada entrada:

- **Decisión:** qué se decidió.
- **Motivo:** por qué.
- **Consecuencias / pendiente:** qué implica o qué queda por resolver.

---

## Convenciones generales

### D-01 — Todo el código en inglés
- **Decisión:** clases, propiedades, métodos y variables en inglés. Los textos visibles para el usuario pueden ir en español.
- **Motivo:** convención profesional; evita mezclar idiomas en el código.
- **Consecuencias / pendiente:** los estados de la tarea también tendrán nombres en inglés en el código.

### D-02 — La entidad de tarea se llama `TaskItem`
- **Decisión:** usar `TaskItem` en lugar de `Task`.
- **Motivo:** `Task` choca con `System.Threading.Tasks.Task`, que se usará con `async`/`await`, EF Core y ASP.NET Core.

### D-03 — Estructura de carpetas mínima
- **Decisión:** por ahora solo existe `Domain/Entities`. Namespace: `TaskFlow.Domain.Entities`.
- **Motivo:** crear carpetas solo cuando haya algo real que poner dentro (YAGNI).
- **Consecuencias / pendiente:** `Application`, `Infrastructure`, etc. se crearán cuando se necesiten.

### D-04 — Cada commit deja el proyecto compilando
- **Decisión:** no hacer commits parciales que rompan la compilación. Los movimientos de archivos van en commits separados de los cambios de lógica.
- **Motivo:** cualquier versión del repositorio debe poder clonarse y ejecutarse.

---

## Dominio — TaskItem

### D-05 — Identificadores con `Guid`
- **Decisión:** `TaskItem` se identifica con un `Guid` generado por la propia entidad. Las referencias a proyecto y usuario también son `Guid`.
- **Motivo:** no depende de una base de datos para generar IDs; evita IDs secuenciales predecibles (riesgo de seguridad en una API).

### D-06 — Título y descripción obligatorios
- **Decisión:** no se puede crear una tarea sin título ni sin descripción.
- **Motivo:** una tarea con solo título no aporta información suficiente para trabajar.

### D-07 — Las violaciones de reglas del dominio lanzan excepción
- **Decisión:** si un dato viola una regla, se lanza `ArgumentException`. Nunca se corrige ni se ignora en silencio.
- **Motivo:** un fallo silencioso hace creer a quien llama que la operación funcionó. Devolver `bool` permite ignorar el error.
- **Consecuencias / pendiente:** quien llama (hoy `Program.cs`, después la capa de presentación) captura la excepción y decide cómo mostrarla. Más adelante se separarán los mensajes técnicos de los mensajes para el usuario.

### D-08 — Cada regla tiene un solo camino
- **Decisión:** el constructor no asigna directamente los datos que tienen reglas o acciones asociadas; llama a los mismos métodos que se usan después de la creación.
- **Motivo:** la regla vive en un solo lugar. Cuando se añada el historial, se registrará en un único punto.

### D-09 — Fecha límite opcional al crear, pero no se puede quitar
- **Decisión:** una tarea puede crearse sin fecha límite. Una vez asignada, puede cambiarse pero no eliminarse. Debe ser igual o posterior a la fecha de creación.
- **Motivo:** exigir una fecha que el usuario no conoce produce datos inventados. Quitar una fecha comprometida podría ocultar un incumplimiento sin que nadie lo note.
- **Consecuencias / pendiente:** idea futura: filtro o reporte de tareas sin fecha límite para que no se pierdan.

### D-10 — Fechas con `DateOnly` y hora local de la máquina
- **Decisión:** `CreationDate` y `TargetCompletionDate` usan `DateOnly`. La fecha de creación se toma con `DateTime.Now`.
- **Motivo:** del negocio solo importa el día. En la etapa de consola, la máquina y el usuario están en la misma zona horaria.
- **Consecuencias / pendiente:** **deuda técnica.** Cuando exista la API, la hora del servidor no será la del usuario. Probablemente la fecha de creación pasará a ser un instante en UTC y se convertirá según la zona horaria de la organización. Se revisará en la Fase 5.

### D-11 — Máximo un usuario asignado y máximo un proyecto
- **Decisión:** `UserId` y `ProjectId` son opcionales (`Guid?`) y únicos.
- **Motivo:** múltiples asignados o múltiples proyectos no son necesarios todavía (YAGNI).
- **Consecuencias / pendiente:** la asignación se hace mediante métodos con intención de negocio, de modo que un cambio futuro a varios asignados afecte sobre todo al interior de la clase.

### D-12 — La entidad no valida la existencia de usuarios ni proyectos
- **Decisión:** `TaskItem` no consulta si un usuario o proyecto existe.
- **Motivo:** el dominio no depende de bases de datos ni de servicios externos.
- **Consecuencias / pendiente:** esa verificación será responsabilidad de la capa Application.

### D-13 — Una tarea que ya fue asignada a un usuario no puede dejar de tener un usuario asignado
- **Decisión:** `TaskItem` no puede dejar de tener un usuario asignado si ya se le asignó uno anteriormente.
- **Motivo:** una tarea que ya pasó por el manejo de un usuario tiene que tener un responsable, no puede haber el caso que ya nadie responda por esa tarea.
- **Consecuencias / pendiente:** Si el usuario dejara de pertenecer a la organización podria quedar huerfana la tarea si no lo revisan.

### D-14 — Una tarea no puede cambiar de proyecto ni dejar de tener un proyecto asignado
- **Decisión:** `TaskItem` no puede cambiar de proyecto si ya se le asignó uno, así como tampoco puede cambiarlo después de asignado.
- **Motivo:** si una tarea quiere cambiarse de proyecto debe crearse una nueva o duplicarse (más adelante a considerar)
- **Consecuencias / pendiente:** Si el proyecto quiere cambiar su enfoque deberia crearse uno nuevo. (O permitir cambiar el proyecto)

### D-15 — Para cualquier string que un usuario digite se va a hacer la sanatización de los datos eliminando espacios en blanco al inicio y al final de la cadena.
- **Decisión:** String que sea digitado por el usuario va a pasar por un TRIM
- **Motivo:** No tener espacios en blancos al inicio o final de las cadenas de texto que puedan afectar los datos.
- **Consecuencias / pendiente:** Se borrarán espacios en blanco al inicio o final que se digiten intencionalmente.

---

## Pendientes de decidir

- [ ] Representación del estado sin atarse permanentemente a un `enum`.
