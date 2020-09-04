using BMTest.Data;
using System;

namespace BMTest.Models
{
    public class TaskResponseModel
    {
        public BmTaskStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
