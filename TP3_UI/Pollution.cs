using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_UI
{
    public class Components
    {
        public double co { get; set; }
        public double no { get; set; }
        public double no2 { get; set; }
        public double o3 { get; set; }
        public double so2 { get; set; }
        public double pm2_5 { get; set; }
        public double pm10 { get; set; }
        public double nh3 { get; set; }
    }

    public class List2
    {
        public Main2 main { get; set; }
        public Components components { get; set; }
        public int dt { get; set; }
    }

    public class Main2
    {
        public int aqi { get; set; }
    }

    public class RootPollution
    {
        public Coord coord { get; set; }
        public List<List2> list { get; set; }
    }
}
