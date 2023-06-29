using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt.Models
{
    public class Blind
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int ActionLevel { get; set; }
        public bool IsClosed { get; set; }
    }
}
