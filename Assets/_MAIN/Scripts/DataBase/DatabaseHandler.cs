using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE; // Используется для работы с диалогами (возможно, определенный вами класс)
using Mono.Data.Sqlite; // Библиотека для работы с SQLite базой данных
using System.Data; // Пространство имен для работы с IDataReader
using System.IO; // Для работы с файлами

// Класс для работы с базой данных и экспортом данных в текстовый файл
public class DatabaseHandler : MonoBehaviour
{
    private string dbPath; // Путь к базе данных SQLite
    private string filePath; // Путь для сохранения текстового файла

    // Метод Start вызывается при запуске игры или сцены
    public void Start()
    {
        #if UNITY_EDITOR
        // Путь к базе данных в редакторе Unity
        dbPath = "URI=file:" + Application.dataPath + "/Plugins/VS.db";
        #elif UNITY_ANDROID
        // Путь к базе данных на Android
        dbPath = "URI=file:" + Application.persistentDataPath + "/VS.db";
        #elif UNITY_IOS
        // Путь к базе данных на iOS
        dbPath = "URI=file:" + Application.persistentDataPath + "/VS.db";
        #endif

        // Путь для сохранения текстового файла
        filePath = Application.dataPath + "/_MAIN/Resources/testFile.txt";
    }

    // Метод для экспорта диалогов из базы данных в текстовый файл
    public void ExportDialoguesToText(string filePath)
    {
        // Подключаемся к базе данных
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open(); // Открываем соединение с базой данных

            // Создаем команду для выполнения SQL-запроса
            using (var command = connection.CreateCommand())
            {
                // SQL-запрос для выборки данных: имя персонажа и строка диалога
                command.CommandText = @"
                    SELECT Character.Name, Dialogues.DialogueLine 
                    FROM Dialogues 
                    JOIN Character ON Dialogues.CharacterID = Character.ID";
                
                // Выполняем команду и получаем данные
                using (IDataReader reader = command.ExecuteReader())
                {
                    // Открываем поток для записи в текстовый файл
                    using (var writer = new StreamWriter(filePath))
                    {
                        // Читаем строки данных из результата запроса
                        while (reader.Read())
                        {
                            // Получаем имя персонажа и строку диалога из результата
                            string charactername = reader.GetString(0);
                            string dialogueLine = reader.GetString(1);
                            
                            // Записываем данные в текстовый файл в формате "Имя персонажа: строка диалога"
                            writer.WriteLine($"{charactername}\" {dialogueLine}\"");
                        }
                    }
                }
            }
        }
    }
}
