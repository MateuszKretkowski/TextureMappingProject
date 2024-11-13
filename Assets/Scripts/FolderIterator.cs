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
        List<List<Color>> listOfTextureColors = new List<List<Color>>();
        List<List<Vector2>> listOfTextureVectors = new List<List<Vector2>>();

        foreach (Sprite sprite in pngTextures)
        {
            Texture2D texture = sprite.texture;

            if (!texture.isReadable)
            {
                Debug.LogError($"Tekstura {sprite.name} musi byæ ustawiona jako Read/Write enabled w ustawieniach importu.");
                continue;
            }

            List<Color> textureColorList = new List<Color>();
            List<Vector2> textureVector2List = new List<Vector2>();
            int textureWidth = texture.width;
            int textureHeight = texture.height;

            Debug.Log($"Przetwarzanie tekstury: {sprite.name}, Rozmiar: {textureWidth}x{textureHeight}");

            if (textureWidth > 0 && textureHeight > 0)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    for (int y = 0; y < textureHeight; y++)
                    {
                        Vector2 pixelPosition = new Vector2(x, y);
                        Color color = texture.GetPixel(x, y);

                        if (color.a != 0) // Logowanie tylko nieprzezroczystych pikseli
                        {
                            textureColorList.Add(color);
                            textureVector2List.Add(pixelPosition);
                            Debug.Log($"Pozycja pikseli: ({x}, {y}), Kolor: {color}");
                        }
                    }
                }
            }
            listOfTextureColors.Add(textureColorList);
            listOfTextureVectors.Add(textureVector2List);
        }

        // Wywo³anie metody mapowania
        Debug.Log("Ca³a lista kolorów i pozycji pikseli zosta³a przetworzona.");
        SpriteMapperScript.MapColorToVector2(listOfTextureColors, "Player", listOfTextureVectors, pngTextures);
    }
}
