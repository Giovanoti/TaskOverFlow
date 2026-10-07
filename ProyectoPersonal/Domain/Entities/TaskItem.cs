namespace TaskFlow.Domain.Entities
{
    internal class TaskItem
    {
        public Guid Id {  get; } = Guid.NewGuid();
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateOnly CreationDate { get; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? TargetCompletionDate { get; private set; } = null;
        public Guid? ProjectId { get; private set; }
        public Guid? UserId { get; private set; }

        public TaskItem(string title, string description, DateOnly? targetCompletionDate = null,Guid? projectId = null, Guid? userId = null)
        {
            Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Tiene que tener un titulo la tarea", nameof(title)) : title.Trim();
            Description = string.IsNullOrWhiteSpace(description)? throw new ArgumentException("Tiene que tener una descripción la tarea", nameof(description)) : description.Trim();
            if(targetCompletionDate.HasValue)  ChangeTargetCompletionDate(targetCompletionDate.Value);
            if(projectId.HasValue) AssignProject(projectId.Value);
            if(userId.HasValue) AssignUser(userId.Value);
        }

        public void ChangeTargetCompletionDate(DateOnly targetCompletionDate) 
        {
            if (targetCompletionDate >= CreationDate)
                TargetCompletionDate = targetCompletionDate;
            else
                throw new ArgumentException("La fecha de completado debe ser mayor o igual a la fecha de creación", nameof(targetCompletionDate));
        }

        public void AssignProject(Guid projectId)
        {
            if (projectId.Equals(Guid.Empty)) throw new ArgumentException("Identificador de proyecto vacio");
            if (!ProjectId.HasValue)
                ProjectId = projectId;
            else
                throw new InvalidOperationException("La tarea ya tiene un proyecto asignado, no puede cambiarse");
        }

        public void AssignUser(Guid userId)
        {
            if (userId.Equals(Guid.Empty)) throw new ArgumentException("Identificador de usuario vacio");
            UserId = userId;
        }
    }
}
