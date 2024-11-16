using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationTextures", menuName = "Custom/Create Folder with Textures")]
public class FolderIterator : ScriptableObject
{
    public bool isMappingTextures;
    public List<Sprite> pngTextures;

    [ContextMenu("Launch TextureMapping")]
    public void GetSpriteBits()
    {
        isMappingTextures = true;
        List<Dictionary<Vector2Int, Color32>> spritesMap = new List<Dictionary<Vector2Int, Color32>>();

        foreach (Sprite sprite in pngTextures)
        {
            Texture2D texture = sprite.texture;

            if (!texture.isReadable)
            {
                Debug.LogError($"Tekstura {sprite.name} musi byæ ustawiona jako Read/Write enabled w ustawieniach importu.");
                continue;
            }

            Dictionary<Vector2Int, Color32> spriteMap = new Dictionary<Vector2Int, Color32>();

            // Use sprite.rect to get the correct width and height
            int spriteWidth = Mathf.RoundToInt(sprite.rect.width);
            int spriteHeight = Mathf.RoundToInt(sprite.rect.height);

            // Get the starting position within the texture
            int xMin = Mathf.RoundToInt(sprite.rect.x);
            int yMin = Mathf.RoundToInt(sprite.rect.y);

            Debug.Log($"Przetwarzanie tekstury: {sprite.name}, Rozmiar: {spriteWidth}x{spriteHeight}");

            if (spriteWidth > 0 && spriteHeight > 0)
            {
                for (int x = 0; x < spriteWidth; x++)
                {
                    for (int y = 0; y < spriteHeight; y++)
                    {
                        // Adjust the texture coordinates based on the sprite's position in the texture
                        int textureX = x + xMin;
                        int textureY = y + yMin;

                        Color32 color = texture.GetPixel(textureX, textureY);

                        if (color.a > 0) // Log only non-transparent pixels
                        {
                            Vector2Int pixelPosition = new Vector2Int(x, y); // Position within the sprite
                            spriteMap.Add(pixelPosition, color);
                            Debug.Log($"Pozycja pikseli: ({x}, {y}), Kolor: {color}");
                        }
                    }
                }
            }
            spritesMap.Add(spriteMap);
        }


        // Call the mapping method
        Debug.Log("All color and pixel position lists have been processed.");
        SpriteMapperScript.MapColorToVector2(spritesMap, "Player", pngTextures);
    }
}
