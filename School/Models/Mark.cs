using System;
using System.Collections.Generic;

#nullable disable

namespace School.Models
{
    public partial class Mark
    {
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public double? Marks { get; set; }
    }
}
