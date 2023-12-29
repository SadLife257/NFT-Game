#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAssetBundle : MonoBehaviour
{
    [MenuItem("Assets/Build Asset Bundles")]
    static void BuildBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";

        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }

        BuildPipeline
            .BuildAssetBundles(assetBundleDirectory,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows);
    }

    [MenuItem("Assets/Get Asset Bundles Name")]
    static void GetName()
    {
        var names = AssetDatabase.GetAllAssetBundleNames();
        foreach (var name in names)
            Debug.Log("AssetBundle: " + name);
    }
}
#endif