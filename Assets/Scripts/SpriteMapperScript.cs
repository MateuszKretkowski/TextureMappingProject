using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class SpriteMapperScript
{
    public List<Vector2> MapColorToVector2(List<Color> colors, string mapName)
    {
        List<Vector2> colorPositions = new List<Vector2>();
        Sprite map = Resources.Load<Sprite>($"Maps/{mapName}_normal.map");

        for (int x=0; x< map.texture.width; x++)
        {
            for (int y=0; y< map.texture.height; y++)
            {
                Vector2 pixelPosition = new Vector2(x, y);

                float u = pixelPosition.x / map.texture.width;
                float v = pixelPosition.y / map.texture.height;

                Color mappedColor = map.texture.GetPixelBilinear(u, v);

                if (colors.Contains(mappedColor))
                {
                    Debug.Log("we have a matched color!");
                    colorPositions.Add(pixelPosition); 
                }
            }
        }
        return colorPositions;
    }
    // list color32
    public List<Color> MapVector2ToColor(List<Vector2> positions, string mapName)
    {
        List<Color> colors = new List<Color>();
        Sprite normalMap = Resources.Load<Sprite>($"Maps/{mapName}.map");

        for (int x=0; x<normalMap.texture.width; x++)
        {
            for (int y=0; y<normalMap.texture.height; y++)
            {
                Vector2 pixelPosition = new Vector2(x, y);

                float u = pixelPosition.x / normalMap.texture.width;
                float v = pixelPosition.y / normalMap.texture.height;

                Color mappedColor = normalMap.texture.GetPixelBilinear(u, v);

                if (positions.Contains(pixelPosition))
                {
                    if (mappedColor.a != 0)
                    {
                        Debug.LogError("there is a bug with MapColor32ToVector2 void");
                    }
                    Debug.Log("We Have Succesfully Mapped a color");
                    colors.Add(mappedColor);
                }
            }
        }
        return colors;
    }

    public void ColorApplier(List<Color> colors, List<Vector2> positions, Sprite objectSprite)
    {
        for (int i=0; i<positions.Count; i++)
        {
            int x = Mathf.RoundToInt(positions[i].x);
            int y = Mathf.RoundToInt(positions[i].y);
            objectSprite.texture.SetPixel(x, y, colors[i]);
        }
        objectSprite.texture.Apply();
    }
}
