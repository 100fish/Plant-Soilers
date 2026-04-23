using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomPathGenerator))]
public class customInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Path"))
        {
            RandomPathGenerator.instance.generatePath();
        }
    }
}
