using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;

// Класс для персонажа с использованием технологии Live2D, наследуется от базового класса Character
public class Character_Live2D : Character
{
    // Конструктор для создания Live2D-персонажа
    public Character_Live2D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
    {
        // Выводим сообщение в консоль, что Live2D-персонаж создан
        Debug.Log($"Created Live2D Character: '{name}'");
    }
}
