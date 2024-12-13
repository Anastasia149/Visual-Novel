using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class PlayerInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Этот метод вызывается каждый кадр.
        // Проверяем, была ли нажата клавиша Mouse0.
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Если клавиша была нажата, вызываем метод для продолжения диалога.
            PromptAdvance();
        }
    }

    // Метод для обработки пользовательского ввода.
    public void PromptAdvance()
    {
        // Вызов метода, который инициирует переход к следующей реплике в диалоге.
        // Это событие говорит системе диалогов, что пользователь готов продолжить.
        DialogueSystem.instance.OnUserPrompt_Next();
    }
}
