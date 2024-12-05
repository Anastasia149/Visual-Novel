using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using Unity.VisualScripting;
using COMMANDS;

// Класс для управления диалогами в игре
public class ConversationManager
{
    // Ссылка на единственный экземпляр `DialogueSystem`, доступ через свойство (Singleton).
    private DialogueSystem dialogueSystem => DialogueSystem.instance;

    // Хранит текущую корутину, выполняющую процесс диалога. Если `process` не равен null, значит диалог запущен.
    private Coroutine process = null;

    // Свойство, возвращающее true, если диалог в процессе выполнения.
    public bool isRunning => process != null;

    // Архитектор текста, отвечающий за постепенное отображение текста (например, эффект "печати").
    private TextArchitect architect = null;

    // Флаг, указывающий на получение пользовательского ввода для продолжения диалога.
    private bool userPrompt = false;

    // Конструктор класса принимает `TextArchitect` и сохраняет ссылку на него.
    public ConversationManager(TextArchitect architect)
    {
        this.architect = architect;

        // Подписываемся на событие, которое возникает при нажатии пользователем кнопки "далее".
        dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
    }

    // Метод, вызываемый при событии "пользователь нажал кнопку продолжения".
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
            return;

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

            // Если строка содержит текст диалога, запускаем выполнение диалога.
            if (line.hasDialogue)
                yield return Line_RunDialogue(line);

            // Если строка содержит команды, выполняем их.
            if (line.hasCommands)
                yield return Line_RunCommands(line);

            // Если в строке есть текст диалога, ждём пользовательского ввода для продолжения.
            if (line.hasDialogue)
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

        // Ждём завершения "печати" текста.
        yield return BuildDialogue(line.dialogueData);
    }

    // Корутин для выполнения команд, указанных в строке диалога.
    IEnumerator Line_RunCommands(DIALOGUE_LINE line)
    {
        // Извлекаем список команд из строки диалога.
        List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;

        // Выполняем каждую команду из списка.
        foreach (DL_COMMAND_DATA.Command command in commands)
        {
            if (command.waitForCompletion)
                yield return CommandManager.instance.Execute(command.name, command.arguments); // Ждём завершения команды.
            else
                CommandManager.instance.Execute(command.name, command.arguments); // Выполняем команду без ожидания.
        }

        yield return null; // Завершаем выполнение корутина.
    }

    // Корутин для "печати" текста с использованием `TextArchitect`.
    IEnumerator BuildDialogue(string dialogue)
    {
        // Передаём текст архитектору для отображения.
        architect.Build(dialogue);

        // Ждём завершения процесса "печати".
        while (architect.isBuilding)
        {
            // Если пользователь нажал кнопку "далее".
            if (userPrompt)
            {
                // Если текст всё ещё "печатается", ускоряем процесс.
                if (!architect.hurryUp)
                {
                    architect.hurryUp = true;
                }
                else
                {
                    architect.ForseComplit(); // Если уже ускорено, завершаем "печать" текста.
                }
                userPrompt = false;
            }

            yield return null; // Ожидание завершения на каждом кадре.
        }
    }

    // Корутин для ожидания пользовательского ввода.
    IEnumerator WaitForUserInput()
    {
        // Ждём, пока пользователь не нажмёт кнопку "далее".
        while (!userPrompt)
        {
            yield return null;
        }

        userPrompt = false; // Сбрасываем флаг пользовательского ввода.
    }
}