using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BarracudaTrack", menuName = "Rhythm/Track")]
public class Track : ScriptableObject
{
    public TextAsset beatFile;
    public List<float> beatTimings = new();

    public void LoadFromFile()
    {
        beatTimings.Clear();
        var lines = beatFile.text.Split('\n');

        foreach (var line in lines)
        {
            if (float.TryParse(line.Trim(), out float t))
                beatTimings.Add(t);
        }
    }
}