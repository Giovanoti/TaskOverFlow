# TaskFlow --- Plan de desarrollo y aprendizaje

## Objetivo general

Construiremos una aplicación en C#/.NET que vaya creciendo
progresivamente hasta convertirse en un proyecto con características
cercanas a una aplicación profesional.

La idea no es crear funcionalidades aisladas. Todo lo que agreguemos
deberá tener sentido dentro del mismo dominio y, al mismo tiempo, servir
para aprender conceptos importantes de C#, .NET, arquitectura, SOLID,
testing y seguridad.

El proyecto inicial será una **Console App con .NET 10.0 LTS**.

------------------------------------------------------------------------

# 1. Plantilla inicial

La plantilla seleccionada es:

-   **Console App**
-   **C#**
-   **.NET 10.0 (Long Term Support)**

No utilizaremos `Console App (.NET Framework)` para este proyecto.

La razón es que queremos trabajar sobre el .NET moderno y posteriormente
evolucionar hacia ASP.NET Core, Entity Framework Core y una API REST.

La evolución prevista será aproximadamente:

``` text
C# / .NET
    ↓
Console App
    ↓
Domain
    ↓
Application
    ↓
Infrastructure
    ↓
SQL Server / EF Core
    ↓
ASP.NET Core API
    ↓
Authentication / Authorization
    ↓
Testing / Security / Quality
```

------------------------------------------------------------------------

# 2. Aplicación: TaskFlow

La aplicación se llamará provisionalmente **TaskFlow**.

No será simplemente una lista de tareas.

Será un pequeño sistema de gestión de trabajo para una organización.

El dominio irá creciendo de forma coherente:

``` text
Organización
    │
    ├── Usuarios
    │
    └── Proyectos
            │
            └── Tareas
```

Una tarea podrá evolucionar para incluir:

-   título
-   descripción
-   prioridad
-   fecha de creación
-   fecha límite
-   estado
-   usuario asignado
-   proyecto al que pertenece

Posteriormente podremos incorporar:

-   comentarios
-   etiquetas
-   historial de cambios
-   auditoría
-   notificaciones
-   archivos
-   búsqueda
-   filtros
-   paginación
-   reportes
-   permisos

Cada nueva funcionalidad deberá estar relacionada con el dominio de
TaskFlow.

------------------------------------------------------------------------

# 3. Filosofía del proyecto

El objetivo no será simplemente "hacer que funcione".

Buscaremos simultáneamente:

1.  Código correcto.
2.  Código comprensible.
3.  Código mantenible.
4.  Arquitectura razonable.
5.  Buenas prácticas de seguridad.
6.  Capacidad de evolución.
7.  Tests adecuados.
8.  Comprensión de las decisiones tomadas.

La meta no es aplicar SOLID por obligación.

La meta es aprender a diseñar software mantenible y utilizar SOLID
cuando realmente aporte valor.

------------------------------------------------------------------------

# 4. Cómo voy a ayudarte

El proyecto será principalmente desarrollado por ti.

Yo actuaré como mentor y revisor.

No te entregaré directamente la solución cuando estemos implementando
una funcionalidad.

En lugar de decir:

``` text
Copia este código.
```

te plantearé objetivos, preguntas y restricciones.

Por ejemplo:

> Objetivo: crear la entidad Task.

Podría indicarte que determines:

1.  Qué información necesita representar.
2.  Qué propiedades son necesarias.
3.  Qué propiedades deberían poder cambiarse.
4.  Qué propiedad identifica una tarea.
5.  Qué reglas deberían cumplirse.
6.  Qué datos no deberían aceptarse.

Después tú escribirás el código.

Cuando me lo muestres, lo revisaré buscando:

-   Correcto
-   Mejorable
-   Problema de diseño
-   Problema de arquitectura
-   Posible vulnerabilidad
-   Código difícil de mantener
-   Concepto que conviene investigar

Si existe un error, primero intentaré darte una pista para que lo
encuentres tú.

Si necesitas una segunda pista, será más específica.

La solución completa no será el primer recurso.

------------------------------------------------------------------------

# 5. Cómo evitaremos grandes refactorizaciones

No intentaremos diseñar todo el sistema desde el primer día.

Eso podría llevarnos a una arquitectura innecesariamente compleja.

La regla será:

> Diseñar desde el principio las fronteras importantes, pero implementar
> únicamente lo que necesitamos en cada etapa.

La estructura conceptual que iremos construyendo será:

``` text
TaskFlow
│
├── Domain
│
├── Application
│
├── Infrastructure
│
└── Presentation
```

Inicialmente algunas partes serán pequeñas.

Posteriormente podrán crecer.

Una posible evolución será:

``` text
Presentation
├── Console
└── API

Application
├── UseCases
├── DTOs
└── Abstractions

Domain
├── Entities
├── ValueObjects
├── Enums
└── DomainRules

Infrastructure
├── Persistence
├── Repositories
└── ExternalServices
```

No crearemos todas estas carpetas y clases inmediatamente.

Las incorporaremos cuando exista una razón para hacerlo.

------------------------------------------------------------------------

# 6. Principios de diseño

Durante todo el proyecto prestaremos atención a:

## SOLID

### Single Responsibility Principle

Cada componente debe tener una responsabilidad coherente y una razón
clara para cambiar.

### Open/Closed Principle

Buscaremos poder extender determinados comportamientos sin modificar
innecesariamente código estable.

### Liskov Substitution Principle

Cuando utilicemos abstracciones y herencia, verificaremos que las
implementaciones respeten los contratos esperados.

### Interface Segregation Principle

Evitaremos interfaces gigantescas que obliguen a las implementaciones a
depender de operaciones que no necesitan.

### Dependency Inversion Principle

Las partes de alto nivel no deberían quedar acopladas innecesariamente a
detalles concretos.

------------------------------------------------------------------------

# 7. Otros principios

También utilizaremos:

-   KISS
-   DRY
-   YAGNI
-   composición sobre herencia cuando sea apropiado
-   encapsulación
-   separación de responsabilidades
-   bajo acoplamiento
-   alta cohesión

Pero ninguno se aplicará mecánicamente.

Cada decisión deberá tener una razón.

------------------------------------------------------------------------

# 8. Seguridad desde el comienzo

La seguridad será una preocupación transversal.

No esperaremos hasta crear la API para comenzar a pensar en ella.

Practicaremos una mentalidad de **Secure by Design**.

Entre los temas que iremos incorporando:

-   Validación de entradas.
-   No confiar en datos proporcionados por usuarios.
-   Manejo adecuado de excepciones.
-   No revelar información sensible en errores.
-   No almacenar secretos en el código.
-   Gestión segura de configuración.
-   Principio de mínimo privilegio.
-   Separación de responsabilidades.
-   Logging responsable.
-   Gestión de dependencias.
-   Validación de autorización.
-   Protección contra inyección SQL cuando llegue la base de datos.
-   Almacenamiento seguro de contraseñas cuando aparezca autenticación.
-   Protección de endpoints.
-   CORS.
-   Rate limiting.
-   Gestión de tokens.

También aprenderemos a distinguir entre:

> "Esto funciona"

y

> "Esto funciona y está diseñado de forma razonablemente segura."

------------------------------------------------------------------------

# 9. Fases del proyecto

## Fase 0 --- Preparación

Objetivo:

Tener correctamente preparado el entorno.

Temas:

-   Visual Studio
-   .NET SDK
-   solución
-   proyecto
-   Git
-   estructura inicial
-   configuración básica

No comenzaremos implementando funcionalidades sin entender primero qué
creó Visual Studio.

------------------------------------------------------------------------

## Fase 1 --- Domain

Construiremos el núcleo del sistema.

Posible estructura:

``` text
Domain
├── Entities
├── ValueObjects
├── Enums
└── DomainRules
```

Aprenderemos:

-   clases
-   objetos
-   propiedades
-   métodos
-   encapsulación
-   invariantes
-   composición
-   enums
-   diseño de entidades
-   reglas de negocio

Todavía no dependeremos de SQL Server ni de HTTP.

------------------------------------------------------------------------

## Fase 2 --- Application

Definiremos las operaciones que el sistema puede realizar.

Ejemplos conceptuales:

``` text
CreateTask
UpdateTask
CompleteTask
DeleteTask
AssignTask
```

Aprenderemos:

-   casos de uso
-   servicios de aplicación
-   interfaces
-   Dependency Inversion
-   DTOs cuando sean necesarios
-   separación entre lógica de negocio y mecanismos externos

------------------------------------------------------------------------

## Fase 3 --- Infrastructure

Comenzaremos con una persistencia sencilla.

Por ejemplo:

``` text
Application
    ↓
Repository abstraction
    ↓
JSON
```

Después evolucionaremos hacia:

``` text
Application
    ↓
Repository abstraction
    ↓
Entity Framework Core
    ↓
SQL Server
```

Aprenderemos:

-   persistencia
-   repositorios
-   Entity Framework Core
-   DbContext
-   migraciones
-   relaciones
-   consultas
-   seguridad de acceso a datos

------------------------------------------------------------------------

## Fase 4 --- Testing

Introduciremos pruebas antes de que el sistema se vuelva demasiado
grande.

Trabajaremos con:

-   xUnit
-   Arrange / Act / Assert
-   pruebas unitarias
-   pruebas de integración
-   casos límite
-   pruebas de reglas de negocio

La intención será aprender a diseñar código que sea fácil de probar.

------------------------------------------------------------------------

## Fase 5 --- ASP.NET Core

La aplicación evolucionará para exponer una API.

Arquitectura conceptual:

``` text
HTTP
 ↓
Controller
 ↓
Application
 ↓
Domain
 ↓
Infrastructure
 ↓
Database
```

Aprenderemos:

-   REST
-   HTTP
-   Controllers
-   routing
-   status codes
-   model binding
-   validation
-   Dependency Injection
-   middleware
-   Swagger/OpenAPI

------------------------------------------------------------------------

## Fase 6 --- Seguridad

Introduciremos:

``` text
Authentication
        ↓
JWT
        ↓
Authorization
        ↓
Roles / Permissions
```

También estudiaremos:

-   gestión de contraseñas
-   secretos
-   tokens
-   autorización
-   CORS
-   rate limiting
-   errores
-   logging
-   OWASP
-   amenazas comunes en APIs

------------------------------------------------------------------------

## Fase 7 --- Calidad profesional

Revisaremos el proyecto como si estuviera pasando por un code review
profesional.

Aspectos:

-   Clean Code
-   SOLID
-   naming
-   organización
-   logging
-   documentación
-   análisis estático
-   dependencias
-   mantenibilidad
-   revisión de arquitectura

------------------------------------------------------------------------

## Fase 8 --- Funcionalidades avanzadas

Podremos incorporar progresivamente:

``` text
Comentarios
Auditoría
Notificaciones
Archivos
Reportes
Búsqueda
Filtros
Paginación
Permisos
Historial
```

Cada funcionalidad tendrá que justificar:

-   dónde pertenece
-   qué capa debe conocerla
-   qué dependencias introduce
-   qué riesgos introduce
-   cómo se prueba

------------------------------------------------------------------------

## Fase 9 --- Evolución arquitectónica

Cuando el proyecto tenga suficiente complejidad podremos estudiar:

``` text
Monolito modular
       ↓
Clean Architecture
       ↓
CQRS
       ↓
Messaging
       ↓
Microservices
```

No introduciremos microservicios simplemente porque sean una tecnología
popular.

La decisión deberá surgir de una necesidad real del sistema.

Esto permitirá aprender cuándo una arquitectura más compleja es útil y
cuándo solamente añade complejidad.

------------------------------------------------------------------------

# 10. Checkpoints

Después de cada fase habrá un checkpoint.

Antes de avanzar, revisaremos:

### Diseño

-   ¿Las responsabilidades están bien separadas?
-   ¿Hay acoplamiento innecesario?
-   ¿Las abstracciones tienen sentido?

### C

-   ¿El código utiliza correctamente las características del lenguaje?
-   ¿La encapsulación es adecuada?
-   ¿Hay código innecesariamente complejo?

### SOLID

-   ¿Hay violaciones evidentes?
-   ¿Estamos aplicando SOLID con una razón real?
-   ¿Estamos creando abstracciones innecesarias?

### Seguridad

-   ¿Qué entradas controla el usuario?
-   ¿Qué datos deberían considerarse sensibles?
-   ¿Qué podría ocurrir si alguien manipula las entradas?
-   ¿Estamos exponiendo información que no deberíamos?

### Testing

-   ¿Qué comportamiento debería probarse?
-   ¿Qué casos límite existen?
-   ¿Qué podría romperse al modificar el código?

### Arquitectura

-   ¿La siguiente fase podrá incorporarse sin destruir la anterior?
-   ¿Estamos adelantando decisiones innecesarias?
-   ¿Existe alguna dependencia que no debería existir?

------------------------------------------------------------------------

# 11. La regla de oro

El proyecto tendrá tres objetivos simultáneos:

``` text
             TASKFLOW
                 │
       ┌─────────┼─────────┐
       ↓         ↓         ↓
   APRENDER   CONSTRUIR   DISEÑAR
      C#       SOFTWARE    SEGURO
     .NET        REAL    Y MANTENIBLE
```

No buscamos solamente terminar una aplicación.

Buscamos que puedas explicar:

-   por qué existe cada capa
-   por qué una clase tiene determinada responsabilidad
-   por qué una dependencia está donde está
-   por qué una abstracción es necesaria
-   qué riesgos de seguridad existen
-   cómo probar el comportamiento
-   qué cambiarías si el sistema creciera

------------------------------------------------------------------------

# 12. Punto actual

Actualmente estamos aquí:

``` text
Fase 0
  ↓
Visual Studio
  ↓
Console App
  ↓
.NET 10.0 LTS
  ↓
[ SIGUIENTE CHECKPOINT ]
```

El siguiente paso será realizar el **Checkpoint 0**.

Todavía no necesitamos implementar funcionalidades.

Primero estableceremos las primeras decisiones de diseño de TaskFlow y
tú las tomarás con orientación.

La intención es comenzar a desarrollar desde el principio con mentalidad
profesional, sin convertir el proyecto en un ejercicio donde simplemente
copiamos código.
