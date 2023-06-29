using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt.Models
{
    public class Temperature
    {
        public string Id { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //  Temperatura
        public decimal TemperatureDeg { get; set; }

        //  Wilgotność
        public decimal Humidity { get; set; }

        public bool IsConnected { get; set; }

    }
}
