using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Profiles
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            CreateMap<TaskItem, TaskItemDTO>();
            CreateMap<TaskItemDTO, TaskItem>();
        }
    }
}
