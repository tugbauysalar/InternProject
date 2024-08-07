using InternProject.Application.DTOs;

namespace InternProject.Application.Services
{
    public interface IEducationService
    {
        Task<CreateEducationDto> CreateEducation(CreateEducationDto dto);
        Task<UpdateEducationDto> UpdateEducation(int id, UpdateEducationDto dto);
        List<EducationAssignmentDto> GetUserEducationAssignments(string userId);
        Task AssignEducationToUser(string userId, int educationId, int day);
        Task DeleteEducation(int id);
    }
}
