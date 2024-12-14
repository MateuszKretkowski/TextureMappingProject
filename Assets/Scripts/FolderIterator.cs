using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FolderIterator", menuName = "Custom/Create Folder with Textures")]
public class FolderIterator : ScriptableObject
{
    public bool isMappingTextures;
    public List<Sprite> pngTextures;

    public void ChangeMapPart(string mapName, int nthX, int nthY, int blockSizeX, int blockSizeY, Sprite mapPart)
    {
        // Wczytujemy mapê
        Sprite map = Resources.Load<Sprite>($"Maps/{mapName}_normal.map");
        if (map == null || map.texture == null)
        {
            Debug.LogError("Map nie zosta³a znaleziona lub tekstura jest niedostêpna.");
            return;
        }

        // Aktualizacja assetów (jeœli konieczne, mo¿na to zrobiæ raz w innym miejscu)
        foreach (Sprite sprite in pngTextures)
        {
            string assetPath = AssetDatabase.GetAssetPath(sprite.texture);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        Texture2D mapTexture = map.texture;
        Texture2D mapPartTexture = mapPart.texture;

        if (!mapPartTexture.isReadable)
        {
            Debug.LogError($"Tekstura {mapPart.name} musi byæ ustawiona jako Read/Write enabled.");
            return;
        }

        // Szybsze pobieranie pikseli z mapPartTexture
        Color32[] partPixels = mapPartTexture.GetPixels32();
        int partWidth = mapPartTexture.width;
        int partHeight = mapPartTexture.height;

        // Tworzymy tablicê pikseli mapy i ustawiamy piksele zbiorczo
        Color32[] mapPixels = mapTexture.GetPixels32();
        int mapWidth = mapTexture.width;

        for (int x = 0; x < partWidth; x++)
        {
            for (int y = 0; y < partHeight; y++)
            {
                int partIndex = y * partWidth + x;
                Color32 color = partPixels[partIndex];
                if (color.a > 0)
                {
                    int mapX = x + (nthX * blockSizeX);
                    int mapY = y + (nthY * blockSizeY);

                    // Sprawdzamy, czy nie wychodzimy poza mapê
                    if (mapX >= 0 && mapX < mapWidth && mapY >= 0 && mapY < mapTexture.height)
                    {
                        int mapIndex = mapY * mapWidth + mapX;
                        mapPixels[mapIndex] = color;
                    }
                }
            }
        }

        // Zapisujemy piksele do mapy i stosujemy zmiany
        mapTexture.SetPixels32(mapPixels);
        mapTexture.Apply();

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

            int textureWidth = texture.width;
            int textureHeight = texture.height;

            Color32[] allPixels = texture.GetPixels32();
            List<Vector2Int> spriteVector2 = new List<Vector2Int>();
            List<Color32> spriteColor = new List<Color32>();

            // Brak logowania dla ka¿dego piksela - to znacznie przyspieszy dzia³anie.
            // Mo¿emy ewentualnie dodaæ jeden log per tekstura.
            // Debug.Log($"Przetwarzanie tekstury: {sprite.name}, Rozmiar: {textureWidth}x{textureHeight}");

            for (int y = 0; y < textureHeight; y++)
            {
                int rowStart = y * textureWidth;
                for (int x = 0; x < textureWidth; x++)
                {
                    Color32 color = allPixels[rowStart + x];
                    if (color.a > 0)
                    {
                        spriteVector2.Add(new Vector2Int(x, y));
                        spriteColor.Add(color);
                    }
                }
            }

            spritesVector2.Add(spriteVector2);
            spritesColor.Add(spriteColor);
        }

        // Wywo³anie mapowania po przetworzeniu wszystkich tekstur
        // Debug.Log("All color and pixel position lists have been processed.");
        SpriteMapperScript.MapColorToVector2(spritesVector2, spritesColor, "Player", pngTextures);
    }
}
