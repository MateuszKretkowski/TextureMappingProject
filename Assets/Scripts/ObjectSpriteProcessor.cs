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
    public FolderIterator folderIterator;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (folderIterator != null && folderIterator.pngTextures != null)
        {
            folderIterator.GetSpriteBits();
        }
        else
        {
            Debug.LogError("Brak przypisanego FolderIterator lub tekstur do przetworzenia.");
        }
    }
}
