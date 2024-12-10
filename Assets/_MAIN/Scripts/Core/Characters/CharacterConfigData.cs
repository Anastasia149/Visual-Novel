using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;
using UnityEngine.TextCore.Text;
using TMPro;
using DIALOGUE;

[System.Serializable] // Указывает, что класс может быть сериализован (например, для хранения в JSON или для отображения в редакторе Unity).
public class CharacterConfigData 
{
    public string name; // Имя персонажа
    public string alias; // Альтернативное имя или псевдоним персонажа
    public Character.CharacterType characterType; // Тип персонажа (определяется в пространстве имен CHARACTER)

    public Color nameColor; // Цвет текста имени персонажа
    public Color dialogueColor; // Цвет текста диалога персонажа

    public TMP_FontAsset nameFont; // Шрифт, используемый для имени персонажа
    public TMP_FontAsset dialogueFont; // Шрифт, используемый для диалога персонажа

    // Метод для создания копии объекта CharacterConfigData
    public CharacterConfigData Copy()
    {
        CharacterConfigData result = new CharacterConfigData(); // Создаем новый экземпляр

        result.name = name; // Копируем имя
        result.alias = alias; // Копируем псевдоним
        result.characterType = characterType; // Копируем тип персонажа
        result.nameFont = nameFont; // Копируем шрифт имени
        result.dialogueFont = dialogueFont; // Копируем шрифт диалога

        // Копируем цвет имени с сохранением прозрачности (альфа-канала)
        result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a); 
        // Копируем цвет диалога с сохранением прозрачности
        result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

        return result; // Возвращаем копию
    }

    // Статическое свойство для получения цвета текста по умолчанию
    private static Color defaultColor => DialogueSystem.instance.config.defaultTextColor; 
    // Статическое свойство для получения шрифта по умолчанию
    private static TMP_FontAsset defaultFont => DialogueSystem.instance.config.defaultFont;

    // Статическое свойство для получения конфигурации персонажа по умолчанию
    public static CharacterConfigData Default
    {
        get
        {
            CharacterConfigData result = new CharacterConfigData(); // Создаем новый экземпляр конфигурации

            result.name = ""; // Имя по умолчанию (пустое)
            result.alias = "alias"; // Псевдоним по умолчанию
            result.characterType = Character.CharacterType.Text; // Тип персонажа по умолчанию (например, текст)

            result.nameFont = defaultFont; // Шрифт имени по умолчанию
            result.dialogueFont = defaultFont; // Шрифт диалога по умолчанию
            // Устанавливаем цвет имени по умолчанию
            result.nameColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a); 
            // Устанавливаем цвет диалога по умолчанию
            result.dialogueColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);

            return result; // Возвращаем объект конфигурации по умолчанию
        }
    }
}
