using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces.Validations
{
    public interface ITaskValidation
    {
        Task ValidateIdForSearch(int id);
        Task ValidateIdForCreate(int id);
        Task ValidateNotEmptyAsync();
        Task ValidateDTOAsync(TaskItemDTO dto);
        Task ValidateIdAndDTOAsync(int id, TaskItemDTO dto);
    }
}
