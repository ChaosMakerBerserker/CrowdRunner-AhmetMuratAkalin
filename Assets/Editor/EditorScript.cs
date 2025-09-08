#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Doors))]
public class DoorsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Doors door = (Doors)target;

        // Orijinal Inspector çiz
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Door Preview", EditorStyles.boldLabel);

        // Buton ile kapıları rastgele ayarlama
        if (GUILayout.Button("Randomize Doors"))
        {
            Undo.RecordObject(door, "Randomize Doors");

            // Random bonus atama
            door.RandomizeDoor();

            // Kapı metinlerini ve renklerini güncelle
            door.UpdateDoorTexts();
            door.UpdateDoorColors();

            // Inspector'ı güncelle
            EditorUtility.SetDirty(door);
        }
    }
}
#endif
