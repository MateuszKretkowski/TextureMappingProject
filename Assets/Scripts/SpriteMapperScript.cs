using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteMapperScript
{
    public static void MapColorToVector2(List<List<Vector2Int>> spritesVector2, List<List<Color32>> spritesColor, string mapName, List<Sprite> pngTextures)
    {
        List<List<Vector2Int>> colorPositions = new List<List<Vector2Int>>();
        Sprite map = Resources.Load<Sprite>($"Maps/{mapName}.map");

        if (map == null || map.texture == null)
        {
            Debug.LogError("Map nie zosta³a znaleziona lub tekstura jest niedostêpna.");
            return;
        }

        foreach (List<Color32> spriteColor in spritesColor)
        {
            List<Vector2Int> vector2List = new List<Vector2Int>();
            List<Color32> colorsInSpriteMap = new List<Color32>();
            foreach (Color32 value in spriteColor)
            {
                bool isFound = new bool();
                isFound = false;
                for (int x = 0; x <= map.texture.width; x++)
                {
                    for (int y = 0; y <= map.texture.height; y++)
                    {
                        Vector2Int pixelPosition = new Vector2Int(x, y);
                        Color32 mappedColor = map.texture.GetPixel(x, y);
                        if (value.Equals(mappedColor))
                        {
                            isFound = true;
                        }
                        if (mappedColor.a > 0 && value.Equals(mappedColor))
                        {
                            vector2List.Add(pixelPosition);
                            isFound = true;
                            Debug.LogFormat("Mapping color {0} at position {1}, with this pixelPosition {2}", mappedColor, pixelPosition, value);
                        }
                    }
                }
                if (!isFound)
                {
                    Debug.LogError("COLOR ISNT FOUND");
                }
            }
            colorPositions.Add(vector2List);
        }
        if (colorPositions.Count != spritesVector2.Count)
        {
            Debug.LogError("ZLE COS JESTTTTY");
        }
        for (int i = 0; i < colorPositions.Count && i < spritesVector2.Count; i++)
        {
            if (colorPositions[i].Count == spritesVector2[i].Count)
            {
                Debug.Log("git");
            }
            else
            {
                Debug.LogError("colorPositions.Count isnt the same as spritesVector2.Count");
                Debug.Log(colorPositions.Count);
                Debug.Log(colorPositions[i].Count);
                Debug.Log(spritesVector2.Count);
                Debug.Log(spritesVector2[i].Count);
            }
        }
                MapVector2ToColor(colorPositions, mapName, pngTextures, spritesVector2);
    }

    public static void MapVector2ToColor(List<List<Vector2Int>> positions, string mapName, List<Sprite> pngTextures, List<List<Vector2Int>> originalPositions)
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
            foreach (Vector2Int vector2Int in texturePositions)
            {
                Vector2Int pixelPosition = new Vector2Int(vector2Int.x, vector2Int.y);
                Color32 mappedColor = normalMap.texture.GetPixel(vector2Int.x, vector2Int.y);
                textureColors.Add(mappedColor);
                Debug.Log("Mapped Color: " + mappedColor + " at Position: " + pixelPosition);
            }
            colors.Add(textureColors);
        }

        ColorApplier(colors, originalPositions, pngTextures);
    }

    public static void ColorApplier(List<List<Color32>> colors, List<List<Vector2Int>> positions, List<Sprite> objectSprite)
    {
        for (int h = 0; h < objectSprite.Count; h++)
        {
            if (h >= colors.Count || h >= positions.Count)
            {
                Debug.LogWarning("Nie znaleziono odpowiedniej liczby kolorów lub pozycji dla tekstury.");
                return;
            }

            Texture2D texture = objectSprite[h].texture;
            if (!texture.isReadable)
            {
                Debug.LogError("Tekstura musi byæ ustawiona jako Read/Write enabled w ustawieniach importu.");
                return;
            }

            for (int i = 0; i < positions[h].Count; i++)
            {
                int x = positions[h][i].x;
                int y = positions[h][i].y;

                if (colors[h][i].a != 0)
                {
                    Debug.LogFormat("Applying color {0} at position ({1}, {2}) on texture {3}", colors[h][i], x, y, objectSprite[h].name);
                    texture.SetPixel(x, y, colors[h][i]);
                }
            }
            texture.Apply();
            Debug.LogFormat("Texture {0} updated and applied.", objectSprite[h].name);
        }
    }
}
