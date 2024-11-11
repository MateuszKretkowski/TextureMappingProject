using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelMap : ScriptableObject
{
    // List of Dictionaries which uses the {player}_normal.map Vector2's, and colors
    public List<Dictionary<Vector2, Color>> colorData;
}
