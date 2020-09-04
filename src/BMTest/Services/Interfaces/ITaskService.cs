using BMTest.Data;
using System;
using System.Threading.Tasks;

namespace BMTest.Services.Interfaces
{
    public interface ITaskService
    {
        Task<Guid> CreateAsync();
        Task<BmTask> GetAsync(Guid taskId);
    }
}