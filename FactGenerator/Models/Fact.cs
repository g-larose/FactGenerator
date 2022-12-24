using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactGenerator.Models
{
    public class Fact
    {
        public Guid Id { get; set; }
        public string? Category { get; set; }
        public string? Link { get; set; }
        public string? Content { get; set; }
    }
}
