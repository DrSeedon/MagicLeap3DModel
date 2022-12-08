using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageFilter : MonoBehaviour
{
    public string[] extensions = { "*.png", "*.jpg" };
    public string nameFolder;
    public string dir;
    public List<Texture2D> textures;
    public string[] dirImages;
    private void Start()
    {
        FilterImages();
    }
    public void FilterImages()
    {
        dir = Application.streamingAssetsPath + "/" + nameFolder;
        dirImages = FilesFilter.instance.FilterFilesByExtension(dir, extensions);
        for (int i = 0; i < dirImages.Length; i++)
        {
            byte[] pngBytes = File.ReadAllBytes(dirImages[i]);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);
            textures.Add(tex);
        }
    }
}