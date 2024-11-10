using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PixelMapProcessor
{
    // This File has Functions on PixelMap.cs

    PixelMap pixelMap;

    public PixelMapProcessor(PixelMap pixelMap)
    {
        this.pixelMap = pixelMap;
    }

    public Color32 ReadColorData(Vector2 pos, int dictionaryIndex)
    {
        return pixelMap.colorData[dictionaryIndex][pos];
    }

    public void AddColorData(Vector2 pos, Color32 color, int dictionaryIndex)
    {
        if (dictionaryIndex < 0 || dictionaryIndex >= pixelMap.colorData.Count)
        {
            Debug.LogError("Invalid dictionary index.");
            return;
        }
        pixelMap.colorData[dictionaryIndex][pos] = color;
    }

    public void RemoveColorData(Vector2 pos, Color32 color, int dictionaryIndex)
    {
        if (dictionaryIndex < 0 || dictionaryIndex >= pixelMap.colorData.Count)
        {
            Debug.LogError("Invalid dictionary index.");
            return;
        }

        if (pixelMap.colorData[dictionaryIndex].ContainsKey(pos))
        {
            pixelMap.colorData[dictionaryIndex].Remove(pos);
            Debug.Log($"Element at position {pos} removed from dictionary {dictionaryIndex}.");
        }
        else
        {
            Debug.LogWarning($"Element at position {pos} not found in dictionary {dictionaryIndex}.");
        }
    }

    public void AddDictionary()
    {
        pixelMap.colorData.Add(new Dictionary<Vector2, Color32>());
    }
}
