#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (LanesManager))]
public class LanesManagerEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        LanesManager myScript = (LanesManager) target;

        if (GUILayout.Button("Initial setup")) {
            myScript.InitialSetup();
        }
    }
}
#endif
