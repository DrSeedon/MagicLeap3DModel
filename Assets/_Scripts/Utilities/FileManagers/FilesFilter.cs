using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Numerics;

/// <summary>
/// Поиск файлов в директории с нужным расширением
/// </summary>
public class FilesFilter : MonoBehaviour
{
    public static FilesFilter instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    
    public string[] FilterFilesByExtension(string[] dirPath, string[] extensions)
    {
        string[][] allFiles = new string[extensions.Length][];
        int count = 0;

        for (int j = 0; j < dirPath.Length; j++)
        {
            for (int i = 0; i < allFiles.Length; i++)
            {
                allFiles[i] = Directory.GetFiles(dirPath[j], extensions[i]);
                count += Directory.GetFiles(dirPath[j], extensions[i]).Length;
            }
        }

        string[] filteredFiles = new string[count];
        count = 0;
        for (int i = 0; i < extensions.Length; i++)
        {
            for (int j = 0; j < allFiles[i].Length; j++)
            {
                filteredFiles[count] = allFiles[i][j];
                count++;
            }
        }
        return filteredFiles;
    }

    public string[] FilterFilesByExtension(string dirPath, string[] extensions)
    {
        if (!Directory.Exists(dirPath))
        {
            string[] noFiles = new string[0];
            return noFiles;
        }
        string[][] allFiles = new string[extensions.Length][];
        int count = 0;

        for (int i = 0; i < allFiles.Length; i++)
        {
            allFiles[i] = Directory.GetFiles(dirPath, extensions[i]);
            count += Directory.GetFiles(dirPath, extensions[i]).Length;
        }

        string[] filteredFiles = new string[count];
        count = 0;
        for (int i = 0; i < extensions.Length; i++)
        {
            for (int j = 0; j < allFiles[i].Length; j++)
            {
                filteredFiles[count] = allFiles[i][j];
                count++;
            }
        }
        return filteredFiles;
    }
}
