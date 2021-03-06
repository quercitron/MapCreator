﻿namespace Triangulation.MapPainter
{
    public class DrawSettings
    {
        public bool DisplayPolygons { get; set; }
        public bool DisplayCoast { get; set; }
        public bool DisplayRivers { get; set; }
        public bool DisplayElevation { get; set; }
        public bool DisplayMoisture { get; set; }
        public bool DisplayLinealBorders { get; set; }
        public bool DisplayNoiseBorders { get; set; }
        public bool ApplyNoise { get; set; }

        public bool DisplayLakes { get; set; }

        public bool DisplayCoastline { get; set; }
    }
}
