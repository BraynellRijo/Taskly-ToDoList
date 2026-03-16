using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class PagedResultDTO<T>
    {
        public IEnumerable<T> items { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
