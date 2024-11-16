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
                            spriteMap.Add(pixelPosition, color);
                            Debug.Log($"Pixel Position: ({x}, {y}), Color: {color}");
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
