using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using Unity.VisualScripting;

public class ConversationManager
{
    // Ссылка на единственный экземпляр `DialogueSystem`, доступ через свойство (Singleton).
    private DialogueSystem dialogueSystem => DialogueSystem.instance;

    // Хранит текущую корутину, выполняющую процесс диалога. Если `process` не равен null, значит диалог запущен.
    private Coroutine process = null;

    // Свойство, возвращающее true, если диалог в процессе выполнения.
    public bool isRunning => process != null;

    // Архитектор текста, который отвечает за постепенное отображение текста (например, эффект "печати").
    private TextArchitect architect = null;
    private bool userPrompt = false;

    // Конструктор класса принимает `TextArchitect` и сохраняет ссылку на него.
    public ConversationManager(TextArchitect architect)
    {
        this.architect = architect;
        dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
    }

    private void OnUserPrompt_Next()
    {
        userPrompt = true;
    }

    // Метод для запуска нового диалога.
    public void StartConversation(List<string> conversation)
    {
        // Прерываем текущий диалог, если он идёт.
        StopConversation();

        // Запускаем новую корутину для обработки переданного списка строк диалога.
        process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
    }

    // Метод для остановки текущего диалога.
    public void StopConversation()
    {
        // Если диалог не запущен, выходим из метода.
        if (!isRunning)
        {
            return; // Ошибка: эта строка завершает метод и делает код ниже недостижимым. Исправить, переместив `dialogueSystem.StopCoroutine` выше.
        }

        // Останавливаем текущую корутину.
        dialogueSystem.StopCoroutine(process);

        // Обнуляем ссылку на корутину, так как диалог завершён.
        process = null;
    }

    // Корутин для обработки диалога. 
    IEnumerator RunningConversation(List<string> conversation)
    {
        // Проходимся по каждой строке в переданном списке.
        for (int i = 0; i < conversation.Count; i++)
        {
            // Если строка пустая или содержит только пробелы, пропускаем её.
            if (string.IsNullOrWhiteSpace(conversation[i]))
                continue;
            
            // Парсим строку в объект `DIALOGUE_LINE`, чтобы разделить текст и команды.
            DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

            // Если строка содержит диалог, запускаем выполнение диалога.
            if (line.hasDialogue)
                yield return Line_RunDialogue(line);

            // Если строка содержит команды, выполняем их.
            if (line.hasCommands)
                yield return Line_RunCommands(line);
            
            if(line.hasDialogue)
                yield return WaitForUserInput();
        }
    }

    // Корутин для выполнения строки диалога.
    IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
    {
        // Если указано имя говорящего, отображаем его.
        if (line.hasSpeaker)
        {
            dialogueSystem.ShowSpeakerName(line.speakerData);
        }
        // Если имени нет, скрываем панель имени.
        else
        {
            dialogueSystem.HideSpeakerName();
        }

        // Ждём завершения "печати" текста, пока `TextArchitect` работает.
        yield return BuildDialogue(line.dialogueData); // Ожидание завершения на каждом кадре.
    }

    // Корутин для выполнения команд, указанных в строке диалога.
    IEnumerator Line_RunCommands(DIALOGUE_LINE line)
    {
        List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;

        foreach(DL_COMMAND_DATA.Command command in commands)
        {
            CommandManager.instance.Execute(command.name, command.arguments);
        }
        // Корутин завершает выполнение.
        yield return null;
    }

    IEnumerator BuildDialogue(string dialogue)
    {
        architect.Build(dialogue);

        // Ждём завершения "печати" текста, пока `TextArchitect` работает.
        while (architect.isBuilding)
        {
            if (userPrompt)
            {
                if (!architect.hurryUp)
                {
                    architect.hurryUp = true;
                }
                else
                {
                    architect.ForseComplit();
                }
                userPrompt=false;
            }
            yield return null; // Ожидание завершения на каждом кадре.
        }
    }

    IEnumerator WaitForUserInput()
    {
        while (!userPrompt)
        {
            yield return null;
        }

        userPrompt = false;
    }
}

