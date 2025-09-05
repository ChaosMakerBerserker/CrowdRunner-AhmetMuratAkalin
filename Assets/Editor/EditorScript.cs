using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class AutoCreateAssets
{
    static AutoCreateAssets()
    {
        // Settings klasörü
        if (!AssetDatabase.IsValidFolder("Assets/Settings"))
            AssetDatabase.CreateFolder("Assets", "Settings");

        // TutorialInfo klasörü
        if (!AssetDatabase.IsValidFolder("Assets/TutorialInfo"))
            AssetDatabase.CreateFolder("Assets", "TutorialInfo");

        // Scripts klasörü
        if (!AssetDatabase.IsValidFolder("Assets/TutorialInfo/Scripts"))
            AssetDatabase.CreateFolder("Assets/TutorialInfo", "Scripts");

        // Readme asset
        if (AssetDatabase.FindAssets("t:Readme").Length == 0)
        {
            Readme readme = ScriptableObject.CreateInstance<Readme>();
            AssetDatabase.CreateAsset(readme, "Assets/Settings/MainReadme.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}