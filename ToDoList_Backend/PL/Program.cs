using AutoMapper;
using BLL.Services;
using BLL.Services.TaskServices;
using BLL.Validations;
using DAL.FileHandler;
using DAL.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.Helpers;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services.TaskServices;
using Domain.Interfaces.Validations;
using Domain.Profiles;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllers();

//FluentValidation - Validators
builder.Services.AddScoped<IValidator<TaskItem>, TaskItemValidator>();
builder.Services.AddScoped<IValidator<TaskItemDTO>, TaskItemDTOValidator>();
builder.Services.AddScoped<IValidator<int>, TaskIdValidation>();

//Validators
builder.Services.AddScoped<ITaskValidation, TaskValidations>();

//Logging   
builder.Services.AddLogging();

//AutoMapper
var config = new MapperConfiguration(cfg => 
    cfg.AddMaps(typeof(TaskItemProfile).Assembly));
builder.Services.AddSingleton(config.CreateMapper());

// Helpers
builder.Services.AddScoped<IFileHandler, JsonFileHandler>();

//Repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

//Services
builder.Services.AddScoped<ITaskCommandService, TaskCommandService>();
builder.Services.AddScoped<ITaskQueryService, TaskQueryService>();
builder.Services.AddScoped<ITaskFilterService, TaskFilterService>();
builder.Services.AddScoped<ITaskStatusService, TaskStatusService>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
