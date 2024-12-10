using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;
using DIALOGUE;
using TMPro;

// Абстрактный класс для представления персонажа
public abstract class Character
{
    public string name = ""; // Имя персонажа
    public string displayName = "";
    public RectTransform root = null; // Корневой объект для размещения UI или визуальных компонентов персонажа
    public CharacterConfigData config;

    public DialogueSystem dialogueSystem => DialogueSystem.instance;

    // Конструктор, принимающий имя персонажа
    public Character(string name, CharacterConfigData config)
    {
        this.name = name; // Устанавливаем имя персонажа
        displayName = name;
        this.config = config;
    }

    public Coroutine Say(string dialogue) => Say(new List<string> {dialogue});
    public Coroutine Say(List<string> dialogue)
    {
        dialogueSystem.ShowSpeakerName(displayName);
        UpdateTextCustomizationOnScreen();
        return dialogueSystem.Say(dialogue);
    }

    public void SetNameFont(TMP_FontAsset font)=>config.nameFont=font;
    public void SetDialogueFont(TMP_FontAsset font)=>config.dialogueFont=font;
    public void SetNameColor(Color color)=>config.nameColor=color;
    public void SetDialogueColor(Color color)=>config.dialogueColor=color;
    public void ResetConfigurationData() =>config = CharacterManager.instance.GetCharacterConfig(name);
    public void UpdateTextCustomizationOnScreen()=>dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

    // Перечисление для определения типа персонажа
    public enum CharacterType
    {
        Text,         // Текстовый персонаж
        Sprite,       // Персонаж на основе одиночного спрайта
        SpriteSheet,  // Персонаж на основе спрайтового листа
        Live2D,       // Персонаж с использованием технологии Live2D
        Model3D       // Персонаж в формате 3D-модели
    }
}
