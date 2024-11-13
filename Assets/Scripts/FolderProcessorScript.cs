using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationTextures", menuName = "Custom/Create Folder with Textures")]
public class FolderIterator : ScriptableObject
{
    public List<Texture2D> pngTextures;
}
