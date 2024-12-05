using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;


public class DatabaseHandler : MonoBehaviour
{
    private string dbPath;
    private string filePath;
     public void Start()
    {
        #if UNITY_EDITOR
        dbPath = "URI=file:" + Application.dataPath + "/Plugins/VS.db"; // Путь для редактора Unity
        #elif UNITY_ANDROID
        dbPath = "URI=file:" + Application.persistentDataPath + "/VS.db"; // Путь для Android
        #elif UNITY_IOS
        dbPath = "URI=file:" + Application.persistentDataPath + "/VS.db"; // Путь для iOS
        #endif
        filePath = Application.dataPath + "/_MAIN/Resources/testFile.txt"; // Путь сохранения
    }

    public void ExportDialoguesToText(string filePath)
    {
        // Подключение к базе данных
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                 command.CommandText = @"
                    SELECT Character.Name, Dialogues.DialogueLine 
                    FROM Dialogues 
                    JOIN Character ON Dialogues.CharacterID = Character.ID";
                using (IDataReader reader = command.ExecuteReader())
                {
                    using (var writer = new StreamWriter(filePath))
                    {
                        while (reader.Read())
                        {
                            
                            string charactername = reader.GetString(0);
                            string dialogueLine = reader.GetString(1);
                            writer.WriteLine($"{charactername}\" {dialogueLine}\"");
                        }
                    }
                }
            }
        }
    
    }
}
