using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    // Класс, представляющий строку диалога
    public class DIALOGUE_LINE
    {
        public string speakerData; // Данные о персонаже, который говорит (например, имя или ID)
        public string dialogueData; // Текст диалога
        public DL_COMMAND_DATA commandData; // Дополнительные команды, связанные с этой строкой диалога

        // Свойство, определяющее, указан ли говорящий (не пустая строка)
        public bool hasSpeaker => speakerData != string.Empty;

        // Свойство, определяющее, указан ли текст диалога (не пустая строка)
        public bool hasDialogue => dialogueData != string.Empty;

        // Свойство, определяющее, присутствуют ли команды (командные данные не равны null)
        public bool hasCommands => commandData != null;

        // Конструктор для создания строки диалога
        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speakerData = speaker; // Устанавливаем данные говорящего
            this.dialogueData = dialogue; // Устанавливаем текст диалога
            // Если команды не пустые или не состоят только из пробелов, создаем объект DL_COMMAND_DATA
            this.commandData = (string.IsNullOrWhiteSpace(commands) ? null : new DL_COMMAND_DATA(commands));
        }
    }
}


