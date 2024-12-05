using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DIALOGUE;

// Класс для работы с данными команд в диалогах
public class DL_COMMAND_DATA
{
    public List<Command> commands; // Список команд, содержащихся в данных
    private const char COMMANDSPLITTER_ID = ','; // Символ, разделяющий команды
    private const char ARGUMENTSCONTAINER_ID = '('; // Символ, обозначающий начало аргументов команды
    private const string WAITCOMMAND_ID = "[wait]"; // Префикс для команды, которая требует ожидания завершения

    // Структура для представления отдельной команды
    public struct Command
    {
        public string name; // Имя команды
        public string[] arguments; // Аргументы команды
        public bool waitForCompletion; // Указывает, нужно ли дождаться завершения команды
    }

    // Конструктор, принимающий строку с сырыми командами
    public DL_COMMAND_DATA(string rawCommands)
    {
        commands = RipCommands(rawCommands); // Извлекаем команды из строки
    }

    // Метод для извлечения списка команд из строки
    public List<Command> RipCommands(string rawCommands)
    {
        // Разделяем строку на отдельные команды по символу COMMANDSPLITTER_ID
        string[] data = rawCommands.Split(COMMANDSPLITTER_ID, System.StringSplitOptions.RemoveEmptyEntries);
        List<Command> result = new List<Command>();

        foreach (string cmd in data)
        {
            Command command = new Command(); // Создаем новую команду
            int index = cmd.IndexOf(ARGUMENTSCONTAINER_ID); // Находим индекс начала аргументов
            command.name = cmd.Substring(0, index).Trim(); // Извлекаем имя команды
            
            // Проверяем, начинается ли команда с префикса [wait]
            if (command.name.ToLower().StartsWith(WAITCOMMAND_ID))
            {
                command.name = command.name.Substring(WAITCOMMAND_ID.Length); // Убираем префикс [wait]
                command.waitForCompletion = true; // Устанавливаем ожидание завершения
            }
            else
            {
                command.waitForCompletion = false; // Не требует ожидания
            }

            // Извлекаем аргументы команды, удаляя круглые скобки
            command.arguments = GetArgs(cmd.Substring(index + 1, cmd.Length - index - 2));
            result.Add(command); // Добавляем команду в список
        }

        return result; // Возвращаем список команд
    }

    // Метод для извлечения аргументов из строки
    private string[] GetArgs(string args)
    {
        List<string> argList = new List<string>(); // Список для хранения аргументов
        StringBuilder currentArg = new StringBuilder(); // Текущий аргумент
        bool inQuotes = false; // Указывает, находится ли текущий символ внутри кавычек

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == '"') // Если символ — кавычка, переключаем режим кавычек
            {
                inQuotes = !inQuotes;
                continue;
            }

            // Если не в кавычках и символ — пробел, считаем аргумент завершенным
            if (!inQuotes && args[i] == ' ')
            {
                argList.Add(currentArg.ToString()); // Добавляем текущий аргумент в список
                currentArg.Clear(); // Очищаем для следующего аргумента
                continue;
            }

            currentArg.Append(args[i]); // Добавляем символ к текущему аргументу
        }

        // Добавляем последний аргумент, если он есть
        if (currentArg.Length > 0)
        {
            argList.Add(currentArg.ToString());
        }

        return argList.ToArray(); // Возвращаем массив аргументов
    }
}
