using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

// Пространство имён для управления системой диалогов.
namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        // Конфигурация системы диалогов, заданная через ScriptableObject.
        [SerializeField] private DialogueSystemConfigurationSO _config;
        public DialogueSystemConfigurationSO config => _config;

        // Объект для хранения UI-элементов системы диалогов.
        public DialogueContainer dialogueContainer = new DialogueContainer();

        // Управляет процессом проведения диалогов.
        private ConversationManager conversationManager;

        // Обеспечивает постепенное отображение текста (эффект "печати").
        private TextArchitect architect;

        // Единственный экземпляр системы диалогов (Singleton).
        public static DialogueSystem instance { get; private set; }

        // Делегат для событий, связанных с вводом пользователя (например, нажатие "далее").
        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;

        // Свойство, показывающее, идёт ли в данный момент диалог.
        public bool isRunningConversation => conversationManager.isRunning;

        // Обработчик базы данных, используемый для экспорта/импорта данных.
        private DatabaseHandler dbHandler;

        // Метод, вызываемый при создании объекта, чтобы инициализировать систему.
        private void Awake()
        {
            // Проверяем, есть ли уже экземпляр системы диалогов.
            if (instance == null)
            {
                instance = this; // Устанавливаем текущий экземпляр.
                Initialize(); // Инициализируем систему.
            }
            else
            {
                DestroyImmediate(gameObject); // Удаляем лишний экземпляр, если он существует.
            }
        }

        // Метод, вызываемый после инициализации, например, для настройки базы данных.
        void Start()
        {
            dbHandler = new DatabaseHandler();
            dbHandler.Start();

            // Указываем путь для сохранения файла с диалогами.
            string filePath = Application.dataPath + "/_MAIN/Resources/testFile.txt";

            // Экспортируем диалоги в текстовый файл.
            dbHandler.ExportDialoguesToText(filePath);
        }

        // Флаг, указывающий, была ли система уже инициализирована.
        bool _initialized = false;

        // Метод для инициализации системы (создание необходимых объектов).
        private void Initialize()
        {
            if (_initialized)
            {
                return; // Если система уже была инициализирована, выходим из метода.
            }

            // Создаём архитектора текста и менеджера диалогов.
            architect = new TextArchitect(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architect);

            _initialized = true; // Отмечаем, что инициализация завершена.
        }

        // Метод для обработки пользовательского ввода, вызывающий связанное событие.
        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }

        // Показ имени говорящего в UI.
        public void ShowSpeakerName(string speakerName = "") => dialogueContainer.nameContainer.Show(speakerName);

        // Скрытие имени говорящего в UI.
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        // Метод для запуска диалога, принимающий имя говорящего и его реплику.
        public void Say(string speaker, string dialogue)
        {
            // Создаём список строк из одной реплики.
            List<string> conversation = new List<string>() { $"{speaker}\"{dialogue}\"" };

            // Запускаем диалог с этим списком.
            Say(conversation);
        }

        // Метод для запуска диалога, принимающий список строк (реплик).
        public void Say(List<string> conversation)
        {
            // Передаём список строк в `ConversationManager` для обработки.
            conversationManager.StartConversation(conversation);
        }
    }
}