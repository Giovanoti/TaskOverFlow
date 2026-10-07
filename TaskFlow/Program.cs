using System.Reflection;
using TaskFlow.Domain.Entities;
#region test
//Mostrar que si se guardan todas las propiedades bien
try
{
    TaskItem firstTask = new TaskItem("Title 1", " Description 1", DateOnly.FromDateTime(DateTime.Today.AddDays(2)),Guid.NewGuid(), Guid.NewGuid());
    PropertyInfo[] properties = firstTask.GetType().GetProperties();

    foreach (PropertyInfo prop in properties)
    {
        // Extract the name and value of each property
        string name = prop.Name;
        object value = prop.GetValue(firstTask, null) ?? "null";

        Console.WriteLine($"{name}: {value}");
    }
}
catch(Exception e) when (e is ArgumentException or InvalidOperationException)
{
    Console.WriteLine(e.Message);
}
// Test de cuando no tiene titulo
try
{
    TaskItem secondTask = new TaskItem("", "Description 2", DateOnly.FromDateTime(DateTime.Today.AddDays(2)), Guid.NewGuid(), Guid.NewGuid());
    Console.WriteLine("Funciona");
}
catch (Exception e) when (e is ArgumentException or InvalidOperationException)
{
    Console.WriteLine(e.Message);
}
//Test de una fecha de finalización anterior a la fecha de creación (al crearse apenas la tarea)
try
{
    TaskItem thirdTask = new TaskItem("Title 3", "Description 3", DateOnly.FromDateTime(DateTime.Today.AddDays(-2)), Guid.NewGuid(), Guid.NewGuid());
    Console.WriteLine("Funciona");
}
catch (Exception e) when (e is ArgumentException or InvalidOperationException)
{
    Console.WriteLine(e.Message);
}
//Test de modificar una fecha de finalización anterior a la fecha de creación (con la tarea ya existente)
try
{
    TaskItem fourthTask = new TaskItem("Title 4", "Description 4", DateOnly.FromDateTime(DateTime.Today.AddDays(5)), Guid.NewGuid(), Guid.NewGuid());
    fourthTask.ChangeTargetCompletionDate(DateOnly.FromDateTime(DateTime.Today.AddDays(-5)));
    Console.WriteLine(fourthTask.TargetCompletionDate);
}
catch (Exception e) when (e is ArgumentException or InvalidOperationException)
{
    Console.WriteLine(e.Message);
}
// Test para comparar que si permite cambiar de usuario
try
{
    var firstUserId = Guid.NewGuid();
    TaskItem fifthTask = new TaskItem("Title 5", "Description 5", DateOnly.FromDateTime(DateTime.Today.AddDays(5)), Guid.NewGuid(), firstUserId);
    fifthTask.AssignUser(Guid.NewGuid());
    Console.WriteLine("Comparar Ids de users: "+fifthTask.UserId.ToString()+" "+ firstUserId.ToString());
}
catch (Exception e) when (e is ArgumentException or InvalidOperationException)
{
    Console.WriteLine(e.Message);
}
// Test para verificar que no permite cambiar el proyecto
try
{
    var firstProjectId = Guid.NewGuid();
    TaskItem sixthTask = new TaskItem("Title 5", "Description 5", DateOnly.FromDateTime(DateTime.Today.AddDays(5)), firstProjectId);
    sixthTask.AssignProject(Guid.NewGuid());
    Console.WriteLine(sixthTask.ProjectId.ToString() + " " + firstProjectId.ToString());
}
catch (Exception e) when (e is ArgumentException or InvalidOperationException)
{
    Console.WriteLine(e.Message);
}
// Test para verificar que si permite añadir un proyecto aunque la tarea ya haya sido creada
try
{
    TaskItem sixthTask = new TaskItem("Title 5", "Description 5", DateOnly.FromDateTime(DateTime.Today.AddDays(5)));
    sixthTask.AssignProject(Guid.NewGuid());
    Console.WriteLine(sixthTask.ProjectId.ToString());
}
catch (Exception e) when (e is ArgumentException or InvalidOperationException)
{
    Console.WriteLine(e.Message);
}
// Test para verificar que no permite insertar identificadores vacios
try
{
    TaskItem sixthTask = new TaskItem("Title 5", "Description 5", DateOnly.FromDateTime(DateTime.Today.AddDays(5)), Guid.Empty);
    sixthTask.AssignProject(Guid.NewGuid());
    Console.WriteLine(sixthTask.ProjectId.ToString());
}
catch (Exception e) when (e is ArgumentException or InvalidOperationException)
{
    Console.WriteLine(e.Message);
}
#endregion