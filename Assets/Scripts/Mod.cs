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
      StructureListChange.AddStructure("oldHouse2", "Assets/Content/Models/oldHouse2.ModPrefab");
      StructureListChange.AddStructure("oldHouse3", "Assets/Content/Models/oldHouse3.ModPrefab");
      StructureListChange.AddStructure("treeTypeA", "Assets/Content/Models/treeTypeA.ModPrefab");
      StructureListChange.AddStructure("heliBody", "Assets/Content/Models/heliBody.ModPrefab");
      StructureListChange.AddStructure("heliBladeMain", "Assets/Content/Models/heliBladeMain.ModPrefab");
      StructureListChange.AddStructure("heliBladeVice", "Assets/Content/Models/heliBladeVice.ModPrefab");
      StructureListChange.AddStructure("Bus", "Assets/Content/Models/Bus.ModPrefab");
      StructureListChange.AddStructure("carCivi", "Assets/Content/Models/carCivi.ModPrefab");
      StructureListChange.AddStructure("carMili", "Assets/Content/Models/carMili.ModPrefab");
      StructureListChange.StartAdd();
    }
    }
}