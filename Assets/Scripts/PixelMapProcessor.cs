using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelMapProcessor
{
    PixelMap pixelMap;

    public PixelMapProcessor(PixelMap pixelMap)
    {
        this.pixelMap = pixelMap;
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

    public void AddDictionary()
    {
        pixelMap.colorData.Add(new Dictionary<Vector2, Color32>());
    }
}
