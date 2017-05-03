using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSAHandset.Models
{
    public class Handset
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public float WidthInMM { get; set; }
        public float HeightInMM { get; set; }
        public float ThicknessInMM { get; set; }
        public float ScreenSizeInIn { get; set; }
        public byte MemoryInGB { get; set; }
        public byte StorageInGB { get; set; }
        public float CameraInMP { get; set; }
        public short BatteryInMAH { get; set; }
        public string Processor { get; set; }
        public string OS { get; set; }
        public string ImageUrl { get; set; }

    }
}