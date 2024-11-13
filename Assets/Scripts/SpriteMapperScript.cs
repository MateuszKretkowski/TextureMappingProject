using System.Collections.Generic;
using UnityEngine;

public class SpriteMapperScript
{
    public static void MapColorToVector2(List<List<Color>> colors, string mapName, List<List<Vector2>> spriteVector2, List<Sprite> pngTextures)
    {
        List<List<Vector2>> colorPositions = new List<List<Vector2>>();
        Sprite map = Resources.Load<Sprite>($"Maps/{mapName}.map");

        if (map == null || map.texture == null)
        {
            Debug.LogError("Map nie zosta³a znaleziona lub tekstura jest niedostêpna.");
            return;
        }

        foreach (List<Color> textureColors in colors)
        {
            List<Vector2> positions = new List<Vector2>();
            for (int x = 0; x < map.texture.width; x++)
            {
                for (int y = 0; y < map.texture.height; y++)
                {
                    Vector2 pixelPosition = new Vector2(x, y);
                    Color mappedColor = map.texture.GetPixel(x, y);

                    if (textureColors.Contains(mappedColor) && mappedColor.a != 0)
                    {
                        positions.Add(pixelPosition);
                    }
                }
            }
            colorPositions.Add(positions);
        }

        MapVector2ToColor(colorPositions, mapName, spriteVector2, pngTextures);
    }

    public static void MapVector2ToColor(List<List<Vector2>> positions, string mapName, List<List<Vector2>> spriteVector2, List<Sprite> pngTextures)
    {
        List<List<Color>> colors = new List<List<Color>>();
        Sprite normalMap = Resources.Load<Sprite>($"Maps/{mapName}_normal.map");

        if (normalMap == null || normalMap.texture == null)
        {
            Debug.LogError("Normal map nie zosta³a znaleziona lub tekstura jest niedostêpna.");
            return;
        }

        foreach (List<Vector2> texturePositions in positions)
        {
            List<Color> textureColors = new List<Color>();
            for (int x = 0; x < normalMap.texture.width; x++)
            {
                for (int y = 0; y < normalMap.texture.height; y++)
                {
                    Vector2 pixelPosition = new Vector2(x, y);
                    Color mappedColor = normalMap.texture.GetPixel(x, y);

                    if (texturePositions.Contains(pixelPosition) && mappedColor.a != 0)
                    {
                        textureColors.Add(mappedColor);
                    }
                }
            }
            colors.Add(textureColors);
        }

        ColorApplier(colors, spriteVector2, pngTextures);
    }

    public static void ColorApplier(List<List<Color>> colors, List<List<Vector2>> positions, List<Sprite> objectSprite)
    {
        for (int h = 0; h < objectSprite.Count; h++)
        {
            if (h >= colors.Count || h >= positions.Count)
            {
                Debug.LogWarning("Nie znaleziono odpowiedniej liczby kolorów lub pozycji dla tekstury.");
                continue;
            }

            Texture2D texture = objectSprite[h].texture;
            if (!texture.isReadable)
            {
                Debug.LogError("Tekstura musi byæ ustawiona jako Read/Write enabled w ustawieniach importu.");
                return;
            }

            for (int i = 0; i < positions[h].Count && i < colors[h].Count; i++)
            {
                int x = Mathf.RoundToInt(positions[h][i].x);
                int y = Mathf.RoundToInt(positions[h][i].y);

                if (colors[h][i].a != 0)
                {
                    texture.SetPixel(x, y, colors[h][i]);
                    texture.Apply();
                }
            }

        }
    }
}
