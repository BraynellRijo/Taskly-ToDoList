using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services.TaskServices
{
    public interface ITaskStatusService
    {
        Task MarkAsCompleted(int id);
    }
}
