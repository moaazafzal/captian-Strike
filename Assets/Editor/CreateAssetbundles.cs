using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

class CreateAssetbundles
{
    [MenuItem("Character Generator/Create Assetbundles")]
    static void Execute()
    {
        bool createdBundle = false;
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(o is GameObject)) continue;
            if (o.name.Contains("@")) continue;

            GameObject characterFBX = (GameObject)o;
            string name = characterFBX.name.ToLower();

            Debug.Log("******* Creating assetbundles for: " + name + " *******");

            // Create a directory to store the generated assetbundles.
            if (!Directory.Exists(AssetbundlePath))
                Directory.CreateDirectory(AssetbundlePath);

            // Delete existing assetbundles for the current character.
            string[] existingAssetbundles = Directory.GetFiles(AssetbundlePath);
            foreach (string bundle in existingAssetbundles)
            {
                if (bundle.EndsWith(".assetbundle") && bundle.Contains("/assetbundles/" + name))
                    File.Delete(bundle);
            }

            // Save bones and animations to a separate assetbundle.
            GameObject characterClone = (GameObject)Object.Instantiate(characterFBX);

            // Postprocess animations: we need them animating even offscreen.
            foreach (Animation anim in characterClone.GetComponentsInChildren<Animation>())
                anim.animateOnlyIfVisible = false;

            foreach (SkinnedMeshRenderer smr in characterClone.GetComponentsInChildren<SkinnedMeshRenderer>())
                Object.DestroyImmediate(smr.gameObject);

            characterClone.AddComponent<SkinnedMeshRenderer>();
            Object characterBasePrefab = GetPrefab(characterClone, "characterbase");
            string path = AssetbundlePath + name + "_characterbase";
            BuildAssetBundle(new[] { characterBasePrefab }, path);

            // Collect materials.
            List<Material> materials = EditorHelpers.CollectAll<Material>(GenerateMaterials.MaterialsPath(characterFBX));

            // Create assetbundles for each SkinnedMeshRenderer.
            foreach (SkinnedMeshRenderer smr in characterFBX.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                List<Object> toinclude = new List<Object>();

                // Save the current SkinnedMeshRenderer as a prefab so it can be included
                // in the assetbundle.
                GameObject rendererClone = (GameObject)EditorUtility.InstantiatePrefab(smr.gameObject);
                GameObject rendererParent = rendererClone.transform.parent.gameObject;
                rendererClone.transform.parent = null;
                Object.DestroyImmediate(rendererParent);
                Object rendererPrefab = GetPrefab(rendererClone, "rendererobject");
                toinclude.Add(rendererPrefab);

                // Collect applicable materials.
                foreach (Material m in materials)
                    if (m.name.Contains(smr.name.ToLower())) toinclude.Add(m);

                // Save the assetbundle.
                List<string> boneNames = new List<string>();
                foreach (Transform t in smr.bones)
                    boneNames.Add(t.name);
                string stringholderpath = "Assets/bonenames.asset";

                StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
                holder.content = boneNames.ToArray();
                AssetDatabase.CreateAsset(holder, stringholderpath);
                toinclude.Add(AssetDatabase.LoadAssetAtPath<StringHolder>(stringholderpath));

                string bundleName = name + "_" + smr.name.ToLower();
                path = AssetbundlePath + bundleName;
                BuildAssetBundle(toinclude.ToArray(), path);
                Debug.Log("Saved " + bundleName + " with " + (toinclude.Count - 2) + " materials");

                // Delete temp assets.
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rendererPrefab));
                AssetDatabase.DeleteAsset(stringholderpath);
                createdBundle = true;
            }
        }

        if (createdBundle)
            UpdateCharacterElementDatabase.Execute();
        else
            EditorUtility.DisplayDialog("Character Generator", "No Asset Bundles created. Select the characters folder in the Project pane to process all characters. Select subfolders to process specific characters.", "Ok");
    }

    static Object GetPrefab(GameObject go, string name)
    {
        string prefabPath = "Assets/" + name + ".prefab";
        Object tempPrefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        Object.DestroyImmediate(go);
        return tempPrefab;
    }

    static void BuildAssetBundle(Object[] assets, string path)
    {
        // Ensure the asset is saved and ready for bundling.
        AssetDatabase.SaveAssets();

        // Build the AssetBundle.
        AssetBundleBuild build = new AssetBundleBuild
        {
            assetBundleName = Path.GetFileNameWithoutExtension(path),
            assetNames = new string[assets.Length]
        };

        for (int i = 0; i < assets.Length; i++)
        {
            build.assetNames[i] = AssetDatabase.GetAssetPath(assets[i]);
        }

        // Create a temporary folder to store the build.
        string tempPath = "Assets/TempAssetBundles";
        if (!Directory.Exists(tempPath))
            Directory.CreateDirectory(tempPath);

        BuildPipeline.BuildAssetBundles(tempPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        // Move the built AssetBundle to the desired location.
        string tempBundlePath = Path.Combine(tempPath, build.assetBundleName);
        File.Move(tempBundlePath, path);

        // Clean up the temporary folder.
        Directory.Delete(tempPath, true);
    }

    public static string AssetbundlePath
    {
        get { return "Assets/AssetBundles" + Path.DirectorySeparatorChar; }
    }
}
