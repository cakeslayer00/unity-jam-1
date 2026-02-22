#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Track))]
public class TrackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Track track = (Track)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Load Beats From File"))
        {
            track.LoadFromFile();
            EditorUtility.SetDirty(track);
        }
    }
}
#endif