using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CarPhysics))]
public class EditorScriptTest : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Custom Buttons");
        if(GUILayout.Button("Reset Car"))
        {
            CarPhysics carPhysics = (CarPhysics)target;
            carPhysics.ResetCar();
        }
    }
}
