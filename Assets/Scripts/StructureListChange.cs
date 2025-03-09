using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using Assets.Scripts.Menu.ListView;
using System.Linq;
using System.Reflection;
using Jundroo.ModTools;
using Assets.Scripts;
using Assets.Scripts.Flight.Sim;
using ModApi.Planet;
using ModApi.Common.Extensions;
using ModApi.Settings;
using System;
using ModApi;


[Harmony]
public static class StructureListChange
{
    [HarmonyTranspiler]
    [HarmonyPatch("AddStructureViewModel", "LoadItems", MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    { 
        var codes = instructions.ToList();
        List<CodeInstruction> codes2 = new List<CodeInstruction>();
        var strcodetmp1 = codes[1065];
        var strcodetmp2 = codes[1066];
        for (int i = 1068; i <= 1076; i++)
        {
            codes2.Add(codes[i]);
        }
        int startIndex = 1166;
        List<CodeInstruction> codes3 = new List<CodeInstruction>();
        for (int i = 1; i <= 47; i++)
        {
            codes3.Add(codes[startIndex + i]);
        }
        
        codes.RemoveRange(startIndex+1, codes3.Count);
        for (int i = 0; i < names.Count; i++)
        {
            codes.Add(strcodetmp1);
            //var tmp = strcodetmp2;
            var tmp = new CodeInstruction(strcodetmp2)
            {
                operand = names[i]
            };
            codes.Add(tmp);
            var tmp2 = new CodeInstruction(strcodetmp2)
            {
                operand = paths[i]
            };
            codes.Add(tmp2);
            foreach(var code in codes2) 
            {
                codes.Add(code);
            }
        }
        foreach (var code in codes3)
        {
            codes.Add(code);
        }
        Debug.Log("Code Process completed.");
        return codes.AsEnumerable();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(StructureNode), "InstantiateSubStructures")]
    private static bool InitSubStrucuturePrefix(Transform parent, IEnumerable<SubStructure> subStructures, ref int lod, ref bool insideRigidBody, StructureNode __instance)
    {
        TerrainQualitySettings.StructureDetailQuality structureDetailQuality = Game.InPlanetStudioScene ? TerrainQualitySettings.StructureDetailQuality.High : Game.Instance.QualitySettings.Terrain.StructureDetail.Value;
        foreach (SubStructure subStructure in subStructures)
        {
            try
            {
                if (structureDetailQuality >= subStructure.RequiredQuality)
                {
                    if (subStructure.LevelOfDetail <= lod || insideRigidBody)
                    {
                        bool flag = false;
                        if (subStructure.LoadedGameObject == null)
                        {
                            GameObject gameObject = Game.Instance.ResourceLoader.InstantiatePrefab(subStructure.PrefabPath);
                            StructureGameObjectScript structureGameObjectScript = gameObject.AddMissingComponent<StructureGameObjectScript>();
                            structureGameObjectScript.StructureNode = null;
                            structureGameObjectScript.SubStructure = subStructure;
                            subStructure.OnGameObjectLoaded(gameObject);
                            Transform transform = gameObject.transform;
                            transform.SetParent(parent, false);
                            transform.localPosition = subStructure.LocalPosition;
                            transform.localRotation = Quaternion.Euler(subStructure.LocalRotation);
                            transform.localScale = subStructure.LocalScale;
                            subStructure.UpdateDynamicMaterials();
                            StructureNode.FixNegativeBoxColliderScales(subStructure.LoadedGameObject);
                            if (Game.InFlightScene && subStructure.AngularVelocity != null)
                            {
                                gameObject.AddComponent<SubStructureRotateScript>().Initialize(subStructure.AngularVelocity.Value);
                            }
                            if (subStructure.CameraCollision == SubStructure.CameraCollisionType.Collide)
                            {
                                Utilities.ChangeLayersOfGameObjectAndChildrenRecursive(gameObject, 29, Array.Empty<int>());
                            }
                            else if (subStructure.CameraCollision == SubStructure.CameraCollisionType.NoCollide)
                            {
                                Utilities.ChangeLayersOfGameObjectAndChildrenRecursive(gameObject, 26, Array.Empty<int>());
                            }
                            flag = true;
                        }
                        bool insideRigidBody2 = insideRigidBody || subStructure.Mass > 0.0;
                        Traverse.Create(__instance).Method("InstantiateSubStructures").GetValue(subStructure.LoadedGameObject.transform, subStructure.SubStructures, lod, insideRigidBody2);
                        if (Game.InFlightScene && flag && !insideRigidBody && subStructure.Mass > 0.0)
                        {
                            subStructure.LoadedGameObject.AddComponent<SubStructureRigidBodyScript>().Initialize(subStructure);
                        }
                    }
                    else if (subStructure.LoadedGameObject != null)
                    {
                        Traverse.Create<StructureNode>().Method("UnloadSubstructureGameObjects").GetValue(subStructure, true);
                    }
                }
            }
            catch 
            {
            }
        }
        return false;
    }

    public static void StartAdd()
    {
        var harmony = new Harmony("com.TL0SR2.StructurePatch");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    public static void AddStructure(string name,string path)
    {
        if (!path.EndsWith(".ModPrefab"))
            path += ".ModPrefab";
        names.Add(name);
        paths.Add(path);
    }

    public static List<string> names = new List<string>();
    public static List<string> paths = new List<string>();
}

[HarmonyPatch]
 class ResourceLoaderPatch
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.GetTypesFromAssembly(typeof(ResourceLoader).Assembly)
            .SelectMany(type => type.GetMethods())
            .Where(method => method.Name.StartsWith("InstantiatePrefab") && !method.IsGenericMethod)
            .Cast<MethodBase>();
    }


    private static bool Prefix(ref string path,ref bool logErrors,ref object __result)
    {
        if (!path.EndsWith(".ModPrefab"))
        {
            return true;
        }
        else
        {
            string pathtmp = path.Substring(0, path.Length - 10) + ".prefab";
            GameObject gameObject = Mod.Instance.ResourceLoader.LoadAsset<GameObject>(pathtmp);
            if (gameObject == null)
            {
                if (logErrors)
                {
                    Debug.LogErrorFormat($"The prefab at path '{pathtmp}' could not be found.");
                }
                __result = null;
            }
            else
            {
                __result = UnityEngine.Object.Instantiate<GameObject>(gameObject);
            }
            return false;
        }
    }
}