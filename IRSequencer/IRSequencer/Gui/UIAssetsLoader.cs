using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace IRSequencer_v3.Gui
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class UIAssetsLoader : MonoBehaviour
    {
        private AssetBundle IRAssetBundle;
        private AssetBundle IRSeqAssetBundle;

        internal static GameObject controlWindowPrefab;
        internal static GameObject sequencerLinePrefab;
        internal static GameObject stateLinePrefab;
        internal static GameObject sequenceLinePrefab;

        internal static GameObject uiSettingsWindowPrefab;

        internal static GameObject editorWindowPrefab;
        internal static GameObject sequenceCommandLinePrefab;

        internal static GameObject basicTooltipPrefab;

        internal static List<Texture2D> iconAssets;
        internal static List<Sprite> spriteAssets;

        public static bool allPrefabsReady = false;

        public IEnumerator LoadBundle(string location)
        {
            while (!Caching.ready)
                yield return null;
            Logger.Log("Loading Bundle");
            using (WWW www = WWW.LoadFromCacheOrDownload(location, 1))
            {
                yield return www;
                IRAssetBundle = www.assetBundle;
                LoadBundleAssets();
            }
        }

        private void LoadBundleAssets()
        {
            Logger.Log("Loading Bundle Assets");

            var prefabs = IRAssetBundle.LoadAllAssets<GameObject>();
            Logger.Log("Looping on prefabs");
            int prefabsLoadedCount = 0;
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (prefabs[i].name == "SequencerMainWindowPrefab")
                {
                    controlWindowPrefab = prefabs[i] as GameObject;
                    prefabsLoadedCount++;
                }

                if (prefabs[i].name == "SequencerLinePrefab")
                {
                    sequencerLinePrefab = prefabs[i] as GameObject;
                    prefabsLoadedCount++;
                }

                if (prefabs[i].name == "SequencerStateLinePrefab")
                {
                    stateLinePrefab = prefabs[i] as GameObject;
                    prefabsLoadedCount++;
                }

                if (prefabs[i].name == "SequenceLinePrefab")
                {
                    sequenceLinePrefab = prefabs[i] as GameObject;
                    prefabsLoadedCount++;
                }

                if (prefabs[i].name == "SequencerUISettingsWindowPrefab")
                {
                    uiSettingsWindowPrefab = prefabs[i] as GameObject;
                    prefabsLoadedCount++;
                }

                if (prefabs[i].name == "SequencerEditorWindowPrefab")
                {
                    editorWindowPrefab = prefabs[i] as GameObject;
                    prefabsLoadedCount++;
                }

                if (prefabs[i].name == "SequenceCommandLine")
                {
                    sequenceCommandLinePrefab = prefabs[i] as GameObject;
                    prefabsLoadedCount++;
                }

                if (prefabs[i].name == "BasicTooltipPrefab")
                {
                    basicTooltipPrefab = prefabs[i] as GameObject;
                    prefabsLoadedCount++;
                }
            }

            allPrefabsReady = (prefabsLoadedCount > 7);

            spriteAssets = new List<Sprite>();
            var sprites = IRAssetBundle.LoadAllAssets<Sprite>();

            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] != null)
                {
                    spriteAssets.Add(sprites[i]);
                    Logger.Log("Successfully loaded Sprite " + sprites[i].name, Logger.Level.Debug);
                }
                else
                {
                    Logger.Log("[IRS]Shit happened during loading Sprite " + sprites[i].name, Logger.Level.Debug);
                }
            }

            iconAssets = new List<Texture2D>();
            var icons = IRAssetBundle.LoadAllAssets<Texture2D>();

            for (int i = 0; i < icons.Length; i++)
            {
                if (icons[i] != null)
                {
                    iconAssets.Add(icons[i]);
                    Logger.Log("[IRS]Successfully loaded texture " + icons[i].name, Logger.Level.Debug);
                }
                else
                {
                    Logger.Log("[IRS]Shit happened during loading texture " + icons[i].name, Logger.Level.Debug);
                }
            }

            if (allPrefabsReady)
                Logger.Log("Successfully loaded all prefabs from AssetBundle");
            else
                Logger.Log("Some prefabs failed to load, bundle = " + IRAssetBundle.name);
        }

        public void Start()
        {
            Logger.Log("Starting loading sprites and stuff");

            //need to clean cache
            Caching.ClearCache();

            var assemblyFile = Assembly.GetExecutingAssembly().Location;
            var bundlePath = "file://" + assemblyFile.Replace(new FileInfo(assemblyFile).Name, "").Replace("\\", "/") + "../AssetBundles/";

            Logger.Log("Loading bundles from BundlePath: " + bundlePath, Logger.Level.Debug);

            Caching.ClearCache();

            StartCoroutine(LoadBundle(bundlePath + "ir_seq_ui_objects_v3.ksp"));
        }

        public void OnDestroy()
        {
            if (IRAssetBundle)
            {
                Logger.Log("Unloading bundle", Logger.Level.Debug);
                IRAssetBundle.Unload(false);
            }
        }
    }
}

