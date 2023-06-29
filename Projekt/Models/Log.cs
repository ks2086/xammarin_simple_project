using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt.Models
{
    public class Log
    {
        public string Id { get; set; }
        public string LogStatusId { get; set; }
        public string Module { get; set; }
        public string ModuleUnitId { get; set; }
        public DateTime Created_at { get; set; }
    }
}
