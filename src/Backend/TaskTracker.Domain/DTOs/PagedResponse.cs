using System.Collections.Generic;

namespace TaskTracker.Domain.DTOs;
public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public int TotalCount { get; set; }
}
