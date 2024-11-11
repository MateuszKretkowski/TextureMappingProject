using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class SpriteMapperScript
{
    public static List<Vector2> MapColorToVector2(List<Color> colors, string mapName)
    {
        List<Vector2> colorPositions = new List<Vector2>();
        Sprite map = Resources.Load<Sprite>($"Maps/{mapName}.map");

        for (int x=0; x<= map.texture.width; x++)
        {
            for (int y=0; y<= map.texture.height; y++)
            {
                Vector2 pixelPosition = new Vector2(x, y);

                float u = pixelPosition.x / map.texture.width;
                float v = pixelPosition.y / map.texture.height;

                Color mappedColor = map.texture.GetPixel(x, y);

                if (colors.Contains(mappedColor) && mappedColor.a != 0)
                {
                    Debug.Log("we have a matched color!");
                    colorPositions.Add(pixelPosition); 
                }
            }
        }
        Debug.Log("Lista: " + string.Join(", ", colorPositions));
        return colorPositions;
    }
    // list color32
    public static List<Color> MapVector2ToColor(List<Vector2> positions, string mapName)
    {
        List<Color> colors = new List<Color>();
        Sprite normalMap = Resources.Load<Sprite>($"Maps/{mapName}_normal.map");

        for (int x=0; x<=normalMap.texture.width; x++)
        {
            for (int y=0; y<=normalMap.texture.height; y++)
            {
                Vector2 pixelPosition = new Vector2(x, y);

                float u = pixelPosition.x / normalMap.texture.width;
                float v = pixelPosition.y / normalMap.texture.height;

                Color mappedColor = normalMap.texture.GetPixel(x, y);

                if (positions.Contains(pixelPosition) && mappedColor.a != 0)
                {
                    Debug.Log("We Have Succesfully Mapped a color");
                    colors.Add(mappedColor);
                }
            }
        }
        return colors;
    }

    public static void ColorApplier(List<Color> colors, List<Vector2> positions, Sprite objectSprite)
    {
        for (int i=0; i<positions.Count && i<colors.Count; i++)
        {
            Debug.Log(colors);
            Debug.Log(positions);
            int x = Mathf.RoundToInt(positions[i].x);
            int y = Mathf.RoundToInt(positions[i].y);
            if (colors[i].a != 0)
            {
                objectSprite.texture.SetPixel(x, y, colors[i]);
            }
        }
        objectSprite.texture.Apply();
    }
}
