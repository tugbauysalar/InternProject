using AutoMapper;
using InternProject.Application;
using InternProject.Application.DTOs;
using InternProject.Application.Services;
using InternProject.Domain.Entities;
using Microsoft.AspNetCore.SignalR;


namespace InternProject.Persistence.Services
{
    public class EducationService : IEducationService
    {
        private readonly IService<Education> _service;
        private readonly IMapper _mapper;
        private readonly IUnitofWork _unitofwork;
        private readonly AppDbContext _context;
        private readonly IHubContext<MyHub> _hubContext;

        public EducationService(IService<Education> service, IMapper mapper, IUnitofWork unitofWork, 
            AppDbContext context, IHubContext<MyHub> hubContext)
        {
            _service = service;
            _mapper = mapper;
            _unitofwork = unitofWork;
            _context = context;
            _hubContext = hubContext;
        }

        public async Task AssignEducationToUser(string userId, int educationId, int day)
        {
            var education = await _context.Educations.FindAsync(educationId);
            var assignedDate = DateTime.UtcNow;
            var completionDate = assignedDate.AddDays(day);
            var educationAssignment = new EducationAssignment
            {
                UserId = userId,
                EducationId = educationId,
                Education = education,
                AssignedDate = assignedDate,
                CompletionDate = completionDate
            };
            _context.EducationAssignments.Add(educationAssignment);
            await _unitofwork.CommitAsync();

            var message = $"Atanan eğitim: {education.Name}, bitirme tarihi: {completionDate.ToShortDateString()}";

            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
        }

        public async Task<CreateEducationDto> CreateEducation(CreateEducationDto dto)
        {
            await _service.AddAsync(_mapper.Map<Education>(dto));
            await _unitofwork.CommitAsync();
            return dto; 
        }

        public async Task DeleteEducation(int id)
        {
            var education = await _service.GetByIdAsync(id);
            education.DeletedDate = DateTime.UtcNow;
            education.IsDeleted = true;
            await _unitofwork.CommitAsync();
        }

        public async Task<UpdateEducationDto> UpdateEducation(int id, UpdateEducationDto dto)
        {
            var education = await _service.GetByIdAsync(id);
            education.Name = dto.Name;
            education.Description = dto.Description;
            education.CategoryId = dto.CategoryId;
            education.UpdatedDate = DateTime.UtcNow;
            await _unitofwork.CommitAsync();
            return dto;
        }
    }
}
