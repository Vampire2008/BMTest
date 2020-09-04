using Microsoft.EntityFrameworkCore;

namespace BMTest.Data
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<BmTask> Tasks { get; set; }
    }
}
