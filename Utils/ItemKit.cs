using System;
using System.Collections.Generic;
using System.Text;

namespace ChatCommands.Utils
{
    public class ItemKit
    {
        public string Name { get; }
        public Dictionary<int, int> PrefabGUIDs { get; }

        public ItemKit(string name, Dictionary<int, int> prefabGuids)
        {
            this.Name = name;
            this.PrefabGUIDs = prefabGuids;
        }
    }
}
