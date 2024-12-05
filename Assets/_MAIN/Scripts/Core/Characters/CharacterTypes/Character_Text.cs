using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;

// Класс для текстового персонажа, наследуется от базового класса Character
public class Character_Text : Character
{
    // Конструктор для создания текстового персонажа
    public Character_Text(string name) : base(name)
    {
        // Выводим сообщение в консоль, что текстовый персонаж создан
        Debug.Log($"Created Text Character: '{name}'");
    }
}
