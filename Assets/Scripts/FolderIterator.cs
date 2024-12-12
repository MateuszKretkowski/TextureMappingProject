using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FolderIterator", menuName = "Custom/Create Folder with Textures")]
public class FolderIterator : ScriptableObject
{
    public bool isMappingTextures;
    public List<Sprite> pngTextures;

    [ContextMenu("Launch TextureMapping")]
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
                for (int x = 0; x <= textureWidth; x++)
                {
                    for (int y = 0; y <= textureHeight; y++)
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
