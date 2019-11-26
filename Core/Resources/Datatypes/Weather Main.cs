using System;
using System.Collections.Generic;
using System.Text;

namespace Duck_Bot_.Net_Core.Core.Resources.Datatypes
{
    public class System
    {
        public string Country { get; set; }
    }
    public class MainW
    {
        public float Temp { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
    }
    public class WindW
    {
        public float Speed { get; set; }
    }
    public class WeatherMain
    {
        public System Sys { get; set; }
        public MainW Main { get; set; }
        public WindW Wind { get; set; }
        public string Name { get; set; }
        public double Dt { get; set; }

    }
}
