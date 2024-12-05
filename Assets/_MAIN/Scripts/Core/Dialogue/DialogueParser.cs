using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DIALOGUE;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class DialogueParser
{
    // Регулярное выражение для поиска команд в строке.
    private const string commandRegexPattern = @"[\w\[\]]*[^\s]\(";

    // Метод для разбора строки диалога и возвращения объекта `DIALOGUE_LINE`.
    public static DIALOGUE_LINE Parse(string rawLine)
    {
        // Логирование для отладки.
        Debug.Log("Parsing line - " + rawLine);

        // Разбираем строку на компоненты: спикера, текст диалога и команды.
        (string speaker, string dialogue, string commands) = RipContent(rawLine);

        // Логирование результатов разбора.
        Debug.Log("Speaker = " + speaker + "\nDialogue = " + dialogue + "\nCommands = " + commands);

        // Возвращаем объект `DIALOGUE_LINE` с разобранными данными.
        return new DIALOGUE_LINE(speaker, dialogue, commands);
    }

    // Вспомогательный метод для извлечения компонентов строки.
    private static (string, string, string) RipContent(string rawLine)
    {
        // Инициализация переменных.
        string speaker = "", dialogue = "", commands = "";

        // Переменные для поиска границ текста диалога.
        int dialogueStart = -1;
        int dialogueEnd = -1;
        bool isEscaped = false; // Флаг для обработки экранированных символов.

        // Проходим по строке посимвольно.
        for (int i = 0; i < rawLine.Length; i++)
        {
            char current = rawLine[i];
            // Если встречается символ экранирования (\), меняем состояние флага.
            if (current == '\\')
            {
                isEscaped = !isEscaped;
            }
            // Если текущий символ — кавычка, проверяем, находится ли она внутри текста диалога.
            else if (current == '"' && !isEscaped)
            {
                if (dialogueStart == -1)
                {
                    // Указываем начало текста диалога.
                    dialogueStart = i;
                }
                else if (dialogueEnd == -1)
                {
                    // Указываем конец текста диалога.
                    dialogueEnd = i;
                }
            }
            else
            {
                isEscaped = false; // Сбрасываем флаг экранирования.
            }
        }

        // Ищем команды с помощью регулярного выражения.
        Regex commandRegex = new Regex(commandRegexPattern);
        MatchCollection matches = commandRegex.Matches(rawLine);
        int commandStart = -1;
        foreach (Match match in matches)
        {
            if(match.Index<dialogueStart || match.Index>dialogueEnd)
            {
                commandStart=match.Index;
                break;
            }
        }

        if (commandStart!=-1 && (dialogueStart == -1 && dialogueEnd == -1))
        {
            return("","", rawLine.Trim());
        }

        // Разбираем строку, если найден текст диалога.
        if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
        {
            // Извлекаем имя спикера (до текста диалога).
            speaker = rawLine.Substring(0, dialogueStart).Trim();

            // Извлекаем сам текст диалога (между кавычками).
            dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1)
                                .Replace("\\\"", "\""); // Убираем экранированные кавычки.

            // Извлекаем команды, если они есть.
            if (commandStart != -1)
            {
                commands = rawLine.Substring(commandStart).Trim();
            }
        }
        // Если команда указана перед текстом диалога.
        else if (commandStart != -1 && dialogueStart > commandStart)
        {
            commands = rawLine;
        }
        // Если строка содержит только имя спикера.
        else
        {
            dialogue = rawLine;
        }

        // Возвращаем разобранные компоненты строки.
        return (speaker, dialogue, commands);
    }
}

