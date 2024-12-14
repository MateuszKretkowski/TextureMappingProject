using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FolderIterator", menuName = "Custom/Create Folder with Textures")]
public class FolderIterator : ScriptableObject
{
    public bool isMappingTextures;
    public List<Sprite> pngTextures;

    public void ChangeMapPart(string mapName, int nthX, int nthY, int blockSizeX, int blockSizeY, Sprite mapPart)
    {
        Sprite map = Resources.Load<Sprite>($"Maps/{mapName}_normal.map");

        if (map == null || map.texture == null)
        {
            Debug.LogError("Map nie zosta³a znaleziona lub tekstura jest niedostêpna.");
            return;
        }

        foreach (Sprite sprite in pngTextures)
        {
            string assetPath = AssetDatabase.GetAssetPath(sprite.texture);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        Texture2D mapPartTexture = mapPart.texture;

        for (int i = 0; i <= mapPartTexture.width; i++)
        {
            for (int j = 0; j <= mapPartTexture.height; j++)
            {
                Color color = mapPartTexture.GetPixel(i, j);
                if (color.a > 0)
                {
                    map.texture.SetPixel(i + (nthX * blockSizeX), j + (nthY * blockSizeY), color);
                }
            }
        }
        map.texture.Apply();
        GetSpriteBits();
    }


    public void GetSpriteBits()
    {
        isMappingTextures = true;
        List<List<Vector2Int>> spritesVector2 = new List<List<Vector2Int>>();
        List<List<Color32>> spritesColor = new List<List<Color32>>();

        foreach (Sprite sprite in pngTextures)
        {
            Texture2D texture = sprite.texture;

            if (!texture.isReadable)
            {
                Debug.LogError($"Tekstura {sprite.name} musi byæ ustawiona jako Read/Write enabled w ustawieniach importu.");
                continue;
            }

            List<Vector2Int> spriteVector2 = new List<Vector2Int>();
            List<Color32> spriteColor = new List<Color32>();
            int textureWidth = texture.width;
            int textureHeight = texture.height;

            Debug.Log($"Przetwarzanie tekstury: {sprite.name}, Rozmiar: {textureWidth}x{textureHeight}");

            if (textureWidth > 0 && textureHeight > 0)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    for (int y = 0; y < textureHeight; y++)
                    {
                        Vector2Int pixelPosition = new Vector2Int(x, y);
                        Color32 color = texture.GetPixel(x, y);

                        if (color.a > 0) // Log only non-transparent pixels
                        {
                            spriteVector2.Add(pixelPosition);
                            spriteColor.Add(color);
                            Debug.Log($"Pixel Position: ({x}, {y}), Color: {color}");
                        }
                    }
                }
            }
            spritesVector2.Add(spriteVector2);
            spritesColor.Add(spriteColor);
        }

        // Call the mapping method
        Debug.Log("All color and pixel position lists have been processed.");
        SpriteMapperScript.MapColorToVector2(spritesVector2, spritesColor, "Player", pngTextures);
    }
}
