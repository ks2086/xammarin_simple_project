using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt.Models
{
    public class Ventilation
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public bool IsActive { get; set; }
        public int PowerLevel { get; set; }
        public decimal MoistureRecoveryLevel { get; set; }
    }
}
