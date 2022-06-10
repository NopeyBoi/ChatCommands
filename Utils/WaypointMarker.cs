using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;

namespace ChatCommands.Utils
{
    public class WaypointMarker
    {
        public string Name { get; }
        public float X { get; }
        public float Y { get; }
        public string User { get; }
        public bool IsGlobal { get; }
        public WaypointMarker(string name, float x, float y, string user, bool isGlobal = false)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.User = user;
            this.IsGlobal = isGlobal;
        }
    }
}
