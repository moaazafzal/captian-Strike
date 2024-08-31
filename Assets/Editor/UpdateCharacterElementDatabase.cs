using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

class UpdateCharacterElementDatabase
{
    // This method collects information about all available
    // CharacterElements stores it in the CharacterElementDatabase
    // assetbundle. Which CharacterElements are available is 
    // determined by checking the generated materials.
    [MenuItem("Character Generator/Update Character Element Database")]
    public static void Execute()
    {
        List<CharacterElement> characterElements = new List<CharacterElement>();

        // As a CharacterElement needs the name of the assetbundle
        // that contains its assets, we go through all assetbundles
        // to match them to the materials we find.
        string[] assetbundles = Directory.GetFiles(CreateAssetbundles.AssetbundlePath);
        string[] materials = Directory.GetFiles("Assets/characters", "*.mat", SearchOption.AllDirectories);
        foreach (string material in materials)
        {
            foreach (string bundle in assetbundles)
            {
                FileInfo bundleFI = new FileInfo(bundle);
                FileInfo materialFI = new FileInfo(material);
                string bundleName = bundleFI.Name.Replace(".assetbundle", "");
                if (!materialFI.Name.StartsWith(bundleName)) continue;
                if (!material.Contains("Per Texture Materials")) continue;
                characterElements.Add(new CharacterElement(materialFI.Name.Replace(".mat", ""), bundleFI.Name));
                break;
            }
        }

        // After collecting all CharacterElements we store them in an
        // assetbundle using a ScriptableObject.

        // Create a ScriptableObject that contains the list of CharacterElements.
        CharacterElementHolder t = ScriptableObject.CreateInstance<CharacterElementHolder>();
        t.content = characterElements;

        // Save the ScriptableObject and load the resulting asset so it can 
        // be added to an assetbundle.
        string assetPath = "Assets/CharacterElementDatabase.asset";
        AssetDatabase.CreateAsset(t, assetPath);
        AssetDatabase.SaveAssets();
        Object o = AssetDatabase.LoadAssetAtPath<CharacterElementHolder>(assetPath);

        // Build the CharacterElementDatabase assetbundle.
        BuildAssetBundles();

        // Delete the ScriptableObject.
        AssetDatabase.DeleteAsset(assetPath);

        Debug.Log("******* Updated Character Element Database, added " + characterElements.Count + " elements *******");
    }

    private static void BuildAssetBundles()
    {
        BuildPipeline.BuildAssetBundles(CreateAssetbundles.AssetbundlePath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
