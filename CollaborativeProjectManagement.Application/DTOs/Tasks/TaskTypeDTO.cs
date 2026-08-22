using CollaborativeProjectManagement.Domain.Entities.Tasks;

namespace CollaborativeProjectManagement.Application.DTOs.Tasks
{
    public class TaskTypeDTO
    {
        public required int Id { get; set; }
        public required string Title { get; set; }

        public static TaskTypeDTO FromEntity(TaskType type) => new()
        {
            Id = type.Id,
            Title = type.Title
        };
    }
}
