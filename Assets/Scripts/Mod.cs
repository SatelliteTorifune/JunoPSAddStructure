namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ModApi;
    using ModApi.Common;
    using ModApi.Mods;
    using UnityEngine;
    using Jundroo.ModTools;

    /// <summary>
    /// A singleton object representing this mod that is instantiated and initialize when the mod is loaded.
    /// </summary>
    public class Mod : ModApi.Mods.GameMod
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Mod"/> class from being created.
        /// </summary>
        private Mod() : base()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the mod object.
        /// </summary>
        /// <value>The singleton instance of the mod object.</value>
        public static Mod Instance { get; } = GetModInstance<Mod>();
        public override void OnModLoaded()
    {
      base.OnModLoaded();
      StructureListChange.AddStructure("RocketFactoryInside", "Assets/Content/Models/RocketFactoryInside.ModPrefab");
      StructureListChange.AddStructure("RocketFactoryOut", "Assets/Content/Models/RocketFactOut.ModPrefab");
      StructureListChange.AddStructure("deadPlane", "Assets/Content/Models/deadPlane.ModPrefab");
      StructureListChange.AddStructure("oldHouse1", "Assets/Content/Models/oldHouse1.ModPrefab");
      StructureListChange.StartAdd();
    }
    }
}