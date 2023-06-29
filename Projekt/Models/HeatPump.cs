using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt.Models
{
    public class HeatPump
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public decimal CompressorPower { get; set; }
        public decimal HeaterPower { get; set; }
        public int CO_HeatingTemperature { get; set; }
        public int CWU_HeatingTemperature { get; set; }

    }
}
