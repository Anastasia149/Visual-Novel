using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;

// Класс для персонажа на основе 3D-модели, наследуется от базового класса Character
public class Character_Model3D : Character
{
    // Конструктор для создания 3D-персонажа
    public Character_Model3D(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
    {
        // Выводим сообщение в консоль, что 3D-персонаж создан
        Debug.Log($"Created 3D Model Character: '{name}'");
    }
}
