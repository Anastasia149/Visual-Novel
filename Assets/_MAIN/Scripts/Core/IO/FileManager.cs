using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{
    // Метод для чтения строк из текстового файла по указанному пути.
    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
    {
        // Если путь не начинается с '/', добавляем корневую директорию.
        if (!filePath.StartsWith('/'))
        {
            filePath = FilePath.root + filePath;
        }

        // Список для хранения строк из файла.
        List<string> lines = new List<string>();

        try
        {
            // Используем StreamReader для построчного чтения файла.
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    // Если включены пустые строки или строка не пуста, добавляем её в список.
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
        }
        catch (FileNotFoundException ex) // Обрабатываем исключение, если файл не найден.
        {
            Debug.LogError("File not found: " + ex.FileName);
        }

        // Возвращаем список строк.
        return lines;
    }

    // Метод для чтения текстового ресурса (TextAsset) из папки Resources.
    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
    {
        // Загружаем текстовый ресурс из Resources по указанному пути.
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if (asset == null) // Если ресурс не найден, выводим сообщение об ошибке.
        {
            Debug.LogError("Asset not found: " + filePath);
            return null;
        }

        // Вызываем перегруженный метод для обработки загруженного TextAsset.
        return ReadTextAsset(asset, includeBlankLines);
    }

    // Перегруженный метод для чтения строк из объекта TextAsset.
    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        // Список для хранения строк.
        List<string> lines = new List<string>();

        // Используем StringReader для построчного чтения текста из TextAsset.
        using (StringReader sr = new StringReader(asset.text))
        {
            while (sr.Peek() > -1) // Читаем, пока не достигнем конца строки.
            {
                string line = sr.ReadLine();

                // Если включены пустые строки или строка не пуста, добавляем её в список.
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                {
                    lines.Add(line);
                }
            }
        }

        // Возвращаем список строк.
        return lines;
    }
}
