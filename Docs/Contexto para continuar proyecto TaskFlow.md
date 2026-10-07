Quiero continuar en este chat un proyecto de aprendizaje en C#/.NET llamado **TaskFlow**. Ya tengo el plan general del proyecto en un archivo Markdown, por lo que no necesito que vuelvas a crear el plan completo. Necesito que uses el siguiente contexto para continuar exactamente desde donde quedamos.

## 1. Objetivo del proyecto

Quiero construir progresivamente una aplicación de gestión de tareas/proyectos llamada **TaskFlow**, empezando como una **Console App** y evolucionándola posteriormente hacia una aplicación más profesional.

La intención principal no es solamente terminar una aplicación, sino **aprender y practicar**:

- C#
- POO
- SOLID
- Clean Code
- arquitectura mantenible
- separación de responsabilidades
- patrones de diseño cuando estén justificados
- persistencia
- SQL Server
- Entity Framework Core
- ASP.NET Core Web API
- autenticación/autorización
- seguridad
- testing
- evolución arquitectónica

El proyecto debe crecer progresivamente. **No quiero sobreingeniería desde el principio.**

### Filosofía arquitectónica

Quiero diseñar pensando en futuras necesidades, pero implementar solamente lo necesario en cada etapa.

La idea es:

> "Diseñar los límites importantes desde el principio, pero no implementar toda la arquitectura final desde el día uno."

No quiero una arquitectura que sea imposible de modificar. Quiero una arquitectura que pueda evolucionar sin tener que rehacer todo.

SOLID es una herramienta, no el objetivo. No quiero crear interfaces/clases innecesarias solamente para decir que se está aplicando SOLID.

La seguridad también debe considerarse desde el diseño, pero sin implementar autenticación, JWT, etc. antes de que corresponda.

---

## 2. Entorno actual

Estoy usando:

- Microsoft Visual Studio
- **Console App**
- **.NET 10.0 LTS**
- C#
- No estoy usando "Console App (.NET Framework)"

El proyecto ya fue creado correctamente.

---

## 3. Metodología de aprendizaje

Quiero que actúes como **mentor**, no como alguien que simplemente escribe el proyecto por mí.

Reglas importantes:

- Explícame paso a paso.
- Hazme razonar sobre las decisiones.
- Cuando estemos haciendo ejercicios, **no me des directamente la solución ni el código**.
- Primero déjame intentarlo.
- Puedes darme pistas y revisar mi código.
- Si cometo un error, explícame por qué y ayúdame a corregirlo.
- Evita avanzar demasiado rápido.
- Pero tampoco quiero que la fase conceptual se vuelva interminable: ya hicimos bastante análisis y quiero empezar a producir código.
- Cada etapa debe terminar en algo tangible: código, prueba, funcionalidad, etc.
- Antes de pasar a una etapa importante, podemos hacer un checkpoint/revisión.

---

# 4. Conceptos funcionales definidos

La aplicación será inicialmente un sistema de gestión de trabajo/tareas.

## Task

Una tarea tiene:

- título
- descripción
- fecha de inicio
- fecha límite/deadline
- usuario asignado opcional
- estado
- proyecto opcional

Además:

- puede tener comentarios
- tiene historial de cambios/actividad

Una tarea puede existir sin proyecto.

Una tarea puede existir sin usuario asignado.

Inicialmente una tarea puede tener **como máximo un usuario asignado**.

No queremos implementar múltiples asignados todavía.

Importante:

"Fecha de finalización" significa **fecha límite para completar la tarea**, no el momento real en que fue completada.

El momento real de finalización puede quedar registrado cuando cambia a un estado como "Finalizada", mediante el historial.

---

## Project

Un proyecto:

- tiene identidad
- tiene título
- tiene descripción
- puede contener muchas tareas

Inicialmente:

> Una tarea pertenece a 0 o 1 proyecto.

No queremos múltiples proyectos por tarea en esta primera versión.

Posteriormente podrían existir conceptos como tags/labels si fueran necesarios, pero no ahora.

---

## Person

`Person` representa a una persona.

No toda persona tiene necesariamente una cuenta del sistema.

Relación conceptual:

> Person → 0..1 User

---

## User

`User` representa la cuenta/acceso al sistema de una persona.

Un usuario:

- tiene identidad propia
- pertenece a una persona
- tiene credenciales/acceso
- tiene exactamente un rol inicialmente

Una persona asignada a una tarea debe tener un usuario.

---

## Organization

Queremos introducir el concepto de **Organization** porque la aplicación puede ser utilizada por múltiples clientes/organizaciones.

Esto es importante porque no queremos repetir un problema visto en otro proyecto donde **todo estaba estructurado exclusivamente por cliente**, y cuando posteriormente se necesitaba algo global era muy difícil hacerlo.

Por eso queremos diseñar pensando desde temprano en el concepto de **scope/alcance**:

```text
Sistema
├── elementos globales
│
└── Organization
    ├── usuarios
    ├── roles
    ├── proyectos
    ├── tareas
    └── configuración
```

Pero **todavía no quiero llenar todas las entidades con `OrganizationId` ni implementar multi-tenancy completo**.

Primero queremos determinar qué conceptos realmente deben ser globales y cuáles pertenecen a una organización.

La idea es que la arquitectura pueda soportar posteriormente ambos casos sin tener que rehacer todo.

---

# 5. Roles

Un usuario tiene exactamente un `Role` inicialmente.

Los roles tienen permisos.

Muy importante:

`Role` **sí tiene identidad propia y tiene alcance por organización**.

Por ejemplo:

```text
Organization A
    Role: Vendedor
        ├── Crear tarea
        ├── Editar tarea
        └── Asignar tarea

Organization B
    Role: Vendedor
        ├── Consultar tarea
        └── Agregar comentario
```

Aunque ambos se llamen "Vendedor", son roles diferentes porque pertenecen a organizaciones diferentes y pueden tener permisos diferentes.

Por tanto:

```text
Organization A
 └── Vendedor A

Organization B
 └── Vendedor B
```

No son el mismo Role.

Un rol puede ser reutilizado por múltiples usuarios dentro de la misma organización:

```text
Usuario 1 ──┐
Usuario 2 ──┼──→ Role Vendedor
Usuario 3 ──┘
```

Si se modifica el rol, el cambio afecta a los usuarios que lo tienen.

---

# 6. Permission

`Permission` representa conceptos de permisos definidos por el sistema.

Ejemplos conceptuales:

```text
Task.Create
Task.Edit
Task.Assign
Task.Delete
Comment.Create
Project.Create
```

Los permisos son reutilizables.

No queremos crear un permiso diferente para cada usuario.

Por ahora los permisos son conceptos definidos por el sistema; no queremos que cada organización pueda inventar arbitrariamente nuevos permisos.

Los roles pueden tener diferentes permisos.

Todavía no vamos a implementar el sistema completo de RBAC; eso llegará cuando corresponda.

---

# 7. Status

El estado de una tarea debe ser controlado.

Ejemplos iniciales:

```text
Pendiente
En progreso
En revisión
Bloqueada
Finalizada
Cancelada
```

Pero hay una decisión importante:

**No queremos asumir que los estados serán permanentemente un enum/fijos.**

Inicialmente podemos trabajar con un conjunto controlado porque es más sencillo y nos permite avanzar.

Sin embargo, la arquitectura debe dejar abierta la posibilidad de que posteriormente los estados sean configurables por organización.

Esto será una futura evolución arquitectónica.

Por tanto, no quiero diseñar ahora algo que nos obligue para siempre a tener un `enum Status`.

---

# 8. Comments

Una tarea puede tener múltiples comentarios.

Un comentario:

- tiene identidad/representa una ocurrencia independiente
- tiene contenido
- tiene autor
- pertenece a una tarea
- registra cuándo fue creado

Dos comentarios con exactamente el mismo texto siguen siendo dos comentarios diferentes.

Los archivos adjuntos a comentarios son una funcionalidad futura.

No implementarlos ahora debido a temas de almacenamiento, seguridad, validación de archivos, etc.

---

# 9. History / historial

El historial es independiente de los comentarios.

Esto es MUY IMPORTANTE:

> Un cambio en una tarea debe generar historial aunque nadie haya escrito un comentario.

Por ejemplo:

```text
Task creada
Task asignada a Juan
Estado cambiado de Pendiente → En progreso
Fecha límite modificada
Comentario agregado
Task marcada como Finalizada
```

Cada evento representa una ocurrencia.

Por eso `History` no debe depender de que exista un `Comment`.

Comentarios e historial son conceptos diferentes.

---

# 10. Modelo conceptual actual

Tenemos aproximadamente:

```text
Organization
    ├── Users
    ├── Roles
    ├── Projects
    └── Tasks

Person
    └── 0..1 User
              └── 1 Role
                    └── * Permissions

Project
    └── 0..* Tasks

Task
    ├── 0..1 Project
    ├── 0..1 assigned User
    ├── 1 Status
    ├── 0..* Comments
    └── 0..* History

Comment
    └── Author/User

History
    └── registra cambios/actividad de Task
```

Esto es conceptual, no significa que ya hayamos decidido exactamente cómo serán las clases, propiedades o relaciones en C#.

---

# 11. Clasificación conceptual que hicimos

Antes de escribir código analizamos si los conceptos necesitaban identidad o eran conceptos reutilizables.

Resultado actual:

```text
Task        → Identidad propia
Project     → Identidad propia
Person      → Identidad propia
User        → Identidad propia
Role        → Identidad propia, por organización
Permission  → Concepto reutilizable
Status      → Concepto reutilizable
Comment     → Hecho/ocurrencia con identidad
History     → Hecho/ocurrencia con identidad
```

Una observación importante:

`Role` inicialmente lo clasifiqué como "concepto reutilizable", pero después corregimos la interpretación:

**Role tiene identidad propia**, aunque puede ser reutilizado por múltiples usuarios dentro de una organización.

---

# 12. OCP y evolución

Me preocupaba que cambiar posteriormente la arquitectura pudiera romper Open/Closed Principle.

La conclusión fue:

> OCP no significa que la arquitectura jamás pueda cambiar.

Estamos haciendo descubrimiento del dominio antes de implementar precisamente para reducir cambios destructivos posteriores.

Queremos que el sistema pueda evolucionar.

Ejemplo:

Inicialmente:

```text
Status
    Pendiente
    En progreso
    Finalizada
```

Posteriormente:

```text
Organization
    ├── Status: Pendiente
    ├── Status: En revisión
    ├── Status: Bloqueada
    └── Status: Finalizada
```

No queremos que la primera implementación haga imposible esa evolución.

---

# 13. Lo que NO queremos hacer todavía

No implementar todavía:

- SQL Server
- Entity Framework Core
- ASP.NET Core
- API
- JWT
- autenticación
- autorización completa
- archivos adjuntos
- microservicios
- CQRS
- mensajería
- Docker
- cloud deployment

Todo eso vendrá posteriormente.

Ahora estamos en **Domain / POO**.

---

# 14. Dónde quedamos EXACTAMENTE

Ya terminamos la fase de descubrimiento conceptual inicial.

El siguiente paso debe ser **tangible y comenzar a escribir código**.

La siguiente tarea acordada era:

> Crear el primer objeto real del dominio: `Task`.

La estructura propuesta inicialmente era:

```text
Domain/
    Entities/
```

Pero todavía **NO he creado la clase `Task`**.

El mentor debe pedirme primero que proponga las propiedades de `Task` y luego ayudarme a diseñarla.

La dinámica debe ser:

1. Yo propongo las propiedades.
2. El mentor revisa mi propuesta.
3. Me hace pensar en identidad, encapsulación, invariantes y comportamiento.
4. Yo intento escribir la clase.
5. El mentor revisa mi código.
6. Corregimos.
7. Ejecutamos/pruebamos.
8. Pasamos al siguiente concepto.

**No quiero que me entregues directamente la clase `Task` completa antes de que yo intente hacerla.**

Quiero aprender construyéndola.

---

## 15. Regla para el nuevo chat

No vuelvas a explicarme todo este contexto desde cero.

Simplemente confirma que entendiste el estado del proyecto y continúa desde:

> **"Vamos a diseñar la primera clase `Task`. Primero dime qué propiedades consideras que debería tener."**

El objetivo ahora es dejar atrás la discusión conceptual excesiva y empezar a producir código, manteniendo las decisiones arquitectónicas importantes que ya definimos.