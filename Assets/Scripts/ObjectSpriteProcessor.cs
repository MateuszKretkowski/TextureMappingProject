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

    public Sprite head_front_dark_example;

    float timer;
    public float timeMax;

    void Start()
    {
        timer = timeMax;
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
    public bool canChange;
    void FixedUpdate()
    {
        if (canChange)
        {
            folderIterator.ChangeMapPart("Player", 0, 4, 16, 16, head_front_dark_example);
            canChange = false;
        }
    }
}
