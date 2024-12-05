using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;

// Абстрактный класс для представления персонажа
public abstract class Character
{
    public string name = ""; // Имя персонажа
    public RectTransform root = null; // Корневой объект для размещения UI или визуальных компонентов персонажа

    // Конструктор, принимающий имя персонажа
    public Character(string name)
    {
        this.name = name; // Устанавливаем имя персонажа
    }

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
