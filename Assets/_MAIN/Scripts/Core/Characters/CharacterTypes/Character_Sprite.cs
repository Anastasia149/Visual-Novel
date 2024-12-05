using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;

public class Character_Sprite : Character
{
    // Конструктор для создания персонажа-спрайта
    public Character_Sprite(string name) : base(name)
    {
        // Выводим сообщение в консоль, что персонаж-спрайт создан
        Debug.Log($"Created Sprite Character: '{name}'");
    }
}
