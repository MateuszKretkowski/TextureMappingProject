using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SpriteMap", menuName = "Custom/pixel Dictionary")]
public class ObjectSpriteMap : ScriptableObject
{
    public List<Vector2> keys = new List<Vector2>();
    public List<Color> values = new List<Color>();

    public void SetColor(Vector2 pos, Color32 color)
    {
        if (keys.Contains(pos))
        {
            int index = keys.IndexOf(pos);
            values[index] = color;
        }
        else
        {
            keys.Add(pos);
            values.Add(color);
        }
    }

    public Color32? GetColor(Vector2 pos)
    {
        if (keys.Contains(pos))
        {
            return values[keys.IndexOf(pos)];
        }
        return null;
    }

}
