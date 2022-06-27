using System;

namespace TaskManager.Model
{
    internal interface ITimestamp
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DetetedAt { get; set; }
    }
}
