using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ObjectSpriteProcessor : MonoBehaviour
{
    // The child of this Script object is the one who holds actual animator, and has the sprite renderer inside.

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    private ObjectSpriteMap spriteMap;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Awake()
    {
        spriteMap = Resources.Load<ObjectSpriteMap>("ObjectSpriteMap");

        if (spriteMap == null)
        {
            Debug.LogError("ObjectSpriteMap asset not found in Resources folder.");
            return;
        }
    }

    private void LateUpdate()
    {
        GetSpriteBits();
    }
    void Update()
    {

    }

    public void GetSpriteBits()
    {
        Sprite sprite = spriteRenderer.sprite;

        int textureWidth = sprite.texture.width;
        int textureHeight = sprite.texture.height;

        Debug.Log(textureHeight + textureWidth);

        if (textureWidth > 0 && textureHeight > 0)
        {
            for (int x=0; x<=textureWidth; x++)
            {
                for (int y=0; y<=textureHeight; y++)
                {
                    Vector2 pixelPosition = new Vector2(x, y);

                    float u = pixelPosition.x / sprite.texture.width;
                    float v = pixelPosition.y / sprite.texture.height;

                    Color color = sprite.texture.GetPixel(x, y);

                    if (color.a == 0 && spriteMap.GetColor(pixelPosition) != null)
                    {
                        spriteMap.Remove(pixelPosition);
                    }
                    else
                    {
                        spriteMap.SetColor(pixelPosition, color);
                    }
                }
            }
        }
    }
}
