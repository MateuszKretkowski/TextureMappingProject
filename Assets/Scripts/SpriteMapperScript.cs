using System.Collections.Generic;
using UnityEngine;

public class SpriteMapperScript
{
    public static void MapColorToVector2(List<Dictionary<Vector2Int, Color32>> spritesMap, string mapName, List<Sprite> pngTextures)
    {
        List<List<Vector2Int>> colorPositions = new List<List<Vector2Int>>();
        Sprite map = Resources.Load<Sprite>($"Maps/{mapName}.map");

        if (map == null || map.texture == null)
        {
            Debug.LogError("Map nie zosta³a znaleziona lub tekstura jest niedostêpna.");
            return;
        }

        foreach (Dictionary<Vector2Int, Color32> spriteMap in spritesMap)
        {
            List<Vector2Int> vector2List = new List<Vector2Int>();
            HashSet<Color32> colorsInSpriteMap = new HashSet<Color32>(spriteMap.Values);

            for (int x = 0; x < map.texture.width; x++)
            {
                for (int y = 0; y < map.texture.height; y++)
                {
                    Vector2Int pixelPosition = new Vector2Int(x, y);
                    Color32 mappedColor = map.texture.GetPixel(x, y);

                    if (mappedColor.a > 0 && colorsInSpriteMap.Contains(mappedColor))
                    {
                        vector2List.Add(pixelPosition);
                    }
                }
            }
            colorPositions.Add(vector2List);
        }

        MapVector2ToColor(colorPositions, mapName, pngTextures);
    }

    public static void MapVector2ToColor(List<List<Vector2Int>> positions, string mapName, List<Sprite> pngTextures)
    {
        List<List<Color32>> colors = new List<List<Color32>>();
        Sprite normalMap = Resources.Load<Sprite>($"Maps/{mapName}_normal.map");

        if (normalMap == null || normalMap.texture == null)
        {
            Debug.LogError("Normal map nie zosta³a znaleziona lub tekstura jest niedostêpna.");
            return;
        }

        foreach (List<Vector2Int> texturePositions in positions)
        {
            List<Color32> textureColors = new List<Color32>();
            HashSet<Vector2Int> positionsSet = new HashSet<Vector2Int>(texturePositions);

            for (int x = 0; x < normalMap.texture.width; x++)
            {
                for (int y = 0; y < normalMap.texture.height; y++)
                {
                    Vector2Int pixelPosition = new Vector2Int(x, y);
                    Color32 mappedColor = normalMap.texture.GetPixel(x, y);

                    if (positionsSet.Contains(pixelPosition) && mappedColor.a > 0)
                    {
                        textureColors.Add(mappedColor);
                    }
                }
            }
            colors.Add(textureColors);
        }

        ColorApplier(colors, positions, pngTextures);
    }

    public static void ColorApplier(List<List<Color32>> colors, List<List<Vector2Int>> positions, List<Sprite> objectSprite)
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
                int x = positions[h][i].x;
                int y = positions[h][i].y;

                if (colors[h][i].a != 0)
                {
                    texture.SetPixel(x, y, colors[h][i]);
                }
            }
            texture.Apply();
        }
    }
}
