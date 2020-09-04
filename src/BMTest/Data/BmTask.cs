using System;

namespace BMTest.Data
{
    public class BmTask
    {
        public Guid Id { get; set; }
        public BmTaskStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
