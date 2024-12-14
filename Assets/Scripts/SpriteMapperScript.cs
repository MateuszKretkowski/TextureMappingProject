using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMapperScript
{
    public static void MapColorToVector2(List<List<Vector2Int>> spritesVector2, List<List<Color32>> spritesColor, string mapName, List<Sprite> pngTextures)
    {
        Sprite map = Resources.Load<Sprite>($"Maps/{mapName}.map");
        if (map == null || map.texture == null)
        {
            Debug.LogError("Map nie zosta�a znaleziona lub tekstura jest niedost�pna.");
            return;
        }

        // Budujemy s�ownik kolor -> lista pozycji pikseli dla mapy
        Texture2D mapTexture = map.texture;
        if (!mapTexture.isReadable)
        {
            Debug.LogError("Mapa musi mie� w��czone Read/Write.");
            return;
        }

        int mapWidth = mapTexture.width;
        int mapHeight = mapTexture.height;
        Color32[] mapPixels = mapTexture.GetPixels32();

        Dictionary<Color32, List<Vector2Int>> colorToPositions = new Dictionary<Color32, List<Vector2Int>>(new Color32Comparer());

        // Inicjalizacja s�ownika
        for (int y = 0; y < mapHeight; y++)
        {
            int rowStart = y * mapWidth;
            for (int x = 0; x < mapWidth; x++)
            {
                Color32 c = mapPixels[rowStart + x];
                if (c.a > 0)
                {
                    if (!colorToPositions.TryGetValue(c, out var positionsList))
                    {
                        positionsList = new List<Vector2Int>();
                        colorToPositions[c] = positionsList;
                    }
                    positionsList.Add(new Vector2Int(x, y));
                }
            }
        }

        List<List<Vector2Int>> colorPositions = new List<List<Vector2Int>>();

        // Teraz zamiast przeszukiwa� map� dla ka�dego koloru, pobieramy pozycje z dictionary
        foreach (List<Color32> spriteColorList in spritesColor)
        {
            List<Vector2Int> vector2List = new List<Vector2Int>();

            foreach (Color32 value in spriteColorList)
            {
                if (colorToPositions.TryGetValue(value, out var foundPositions))
                {
                    // Dodajemy wszystkie znalezione pozycje dla danego koloru
                    vector2List.AddRange(foundPositions);
                }
                else
                {
                    // Kolor nie zosta� znaleziony w mapie
                    // Mo�na to zlogowa� rzadziej lub zignorowa�, je�li to normalny przypadek
                    // Debug.LogError($"COLOR {value} ISN'T FOUND IN THE MAP");
                }
            }

            colorPositions.Add(vector2List);
        }

        if (colorPositions.Count != spritesVector2.Count)
        {
            Debug.LogError("Ilo�� przetworzonych pozycji kolor�w nie odpowiada ilo�ci wektor�w z spritesVector2.");
        }

        // Sprawdzamy zgodno�� ilo�ci pozycji - ewentualnie mo�na to zredukowa� lub logowa� jedynie w trybie debug.
        for (int i = 0; i < colorPositions.Count && i < spritesVector2.Count; i++)
        {
            if (colorPositions[i].Count == spritesVector2[i].Count)
            {
                // Mo�na zlogowa� sukces tylko raz lub pomin��.
            }
            else
            {
                // Je�li to jest b��d logiczny to logujemy, w przeciwnym razie mo�na ograniczy� logowanie.
                Debug.LogError("colorPositions.Count nie jest taki sam jak spritesVector2.Count dla indeksu " + i);
            }
        }

        MapVector2ToColor(colorPositions, mapName, pngTextures, spritesVector2);
    }

    public static void MapVector2ToColor(List<List<Vector2Int>> positions, string mapName, List<Sprite> pngTextures, List<List<Vector2Int>> originalPositions)
    {
        Sprite normalMap = Resources.Load<Sprite>($"Maps/{mapName}_normal.map");
        if (normalMap == null || normalMap.texture == null)
        {
            Debug.LogError("Normal map nie zosta�a znaleziona lub tekstura jest niedost�pna.");
            return;
        }

        Texture2D normalTexture = normalMap.texture;
        if (!normalTexture.isReadable)
        {
            Debug.LogError("Normal map musi mie� w��czone Read/Write.");
            return;
        }

        int normalWidth = normalTexture.width;
        int normalHeight = normalTexture.height;
        Color32[] normalPixels = normalTexture.GetPixels32();

        List<List<Color32>> colors = new List<List<Color32>>();

        // Pobieramy kolory z normalMap na podstawie pozycji
        for (int i = 0; i < positions.Count; i++)
        {
            List<Vector2Int> texturePositions = positions[i];
            List<Color32> textureColors = new List<Color32>();

            for (int j = 0; j < texturePositions.Count; j++)
            {
                Vector2Int pos = texturePositions[j];
                if (pos.x >= 0 && pos.x < normalWidth && pos.y >= 0 && pos.y < normalHeight)
                {
                    int idx = pos.y * normalWidth + pos.x;
                    Color32 mappedColor = normalPixels[idx];
                    textureColors.Add(mappedColor);
                }
                else
                {
                    // Poza zakresem, mo�na zlogowa� ostrze�enie je�li to niepo��dana sytuacja.
                }
            }
            colors.Add(textureColors);
        }

        ColorApplier(colors, originalPositions, pngTextures);
    }

    public static void ColorApplier(List<List<Color32>> colors, List<List<Vector2Int>> positions, List<Sprite> objectSprite)
    {
        // Dla ka�dego sprite'a stosujemy kolory
        for (int h = 0; h < objectSprite.Count; h++)
        {
            if (h >= colors.Count || h >= positions.Count)
            {
                Debug.LogWarning("Brak wystarczaj�cej ilo�ci kolor�w lub pozycji dla tekstury o indeksie " + h);
                return;
            }

            Texture2D texture = objectSprite[h].texture;
            if (!texture.isReadable)
            {
                Debug.LogError("Tekstura musi by� ustawiona jako Read/Write enabled w ustawieniach importu.");
                return;
            }

            Color32[] spritePixels = texture.GetPixels32();
            int width = texture.width;
            int height = texture.height;

            List<Vector2Int> spritePositions = positions[h];
            List<Color32> spriteColors = colors[h];

            for (int i = 0; i < spritePositions.Count; i++)
            {
                int x = spritePositions[i].x;
                int y = spritePositions[i].y;

                if (x >= 0 && x < width && y >= 0 && y < height && spriteColors[i].a != 0)
                {
                    int idx = y * width + x;
                    spritePixels[idx] = spriteColors[i];
                }
            }

            // Ustawiamy piksele zbiorczo i Apply
            texture.SetPixels32(spritePixels);
            texture.Apply();
            // Debug.LogFormat("Texture {0} updated and applied.", objectSprite[h].name);
        }
    }

    // Komparator dla Color32 potrzebny do dictionary, aby unikn�� problem�w z defaultowym por�wnywaniem
    private class Color32Comparer : IEqualityComparer<Color32>
    {
        public bool Equals(Color32 x, Color32 y)
        {
            return x.r == y.r && x.g == y.g && x.b == y.b && x.a == y.a;
        }

        public int GetHashCode(Color32 obj)
        {
            // Prost� metod� jest z�o�enie bajt�w koloru w jeden int
            return obj.r ^ (obj.g << 8) ^ (obj.b << 16) ^ (obj.a << 24);
        }
    }
}
