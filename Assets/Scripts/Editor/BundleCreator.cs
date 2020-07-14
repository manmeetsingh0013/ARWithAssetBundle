using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BundleCreator : Editor
{
    [MenuItem("Assets/Build AssetBundles")]

    static void BuildAssetBundle()
    {
        string path = "/Users/manmeetsingh/Desktop/Bundle/IOS";

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.iOS);

    }
}
