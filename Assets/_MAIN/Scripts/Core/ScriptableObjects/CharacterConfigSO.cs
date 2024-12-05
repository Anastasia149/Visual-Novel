using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;
using DIALOGUE;

// Атрибут CreateAssetMenu позволяет создавать объект ScriptableObject через меню в Unity Editor.
[CreateAssetMenu(fileName = "Character Configuration Asset", menuName = "Dialogue System/Character Configuration Asset")]
public class CharacterConfigSO : ScriptableObject
{
    // Массив данных конфигурации персонажей, который будет использоваться для настройки различных персонажей.
    public CharacterConfigData[] characters;

    // Метод для получения конфигурации персонажа по имени.
    // Он ищет конфигурацию, соответствующую имени персонажа или его псевдониму.
    public CharacterConfigData GetConfig(string characterName)
    {
        characterName = characterName.ToLower(); // Приводим имя к нижнему регистру для удобства поиска.

        // Проходим по всем конфигурациям и ищем совпадение по имени или псевдониму.
        for (int i = 0; i < characters.Length; i++)
        {
            CharacterConfigData data = characters[i];

            // Проверяем, совпадает ли имя персонажа или псевдоним с введенным именем.
            if (string.Equals(characterName, data.name.ToLower()) || string.Equals(characterName, data.alias.ToLower()))
                return data.Copy(); // Возвращаем копию конфигурации, чтобы избежать изменения оригинальных данных.
        }

        // Если не нашли конфигурацию, возвращаем конфигурацию по умолчанию.
        return CharacterConfigData.Default;
    }
}
