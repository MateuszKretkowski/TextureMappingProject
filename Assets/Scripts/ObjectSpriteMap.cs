using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ObjectSpriteMap", menuName = "Custom/pixel Dictionary")]
public class ObjectSpriteMap : ScriptableObject
{
    public List<Vector2> keys = new List<Vector2>();
    public List<Color> values = new List<Color>();
    private string filePath => Path.Combine(Application.persistentDataPath, "ObjectSpriteMap.json");

    public void SetColor(Vector2 pos, Color color)
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

    public Color? GetColor(Vector2 pos)
    {
        if (keys.Contains(pos))
        {
            return values[keys.IndexOf(pos)];
        }
        return null;
    }

    public void Remove(Vector2 pos)
    {
        int index = keys.IndexOf(pos);
            keys.RemoveAt(index);
            values.RemoveAt(index);
    }
}
