using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DIALOGUE_LINE
    {
        public string speakerData;
        public string dialogueData;
        public DL_COMMAND_DATA commandData;

        public bool hasSpeaker => speakerData != string.Empty;
        public bool hasDialogue => dialogueData != string.Empty;
        public bool hasCommands =>commandData != null;

        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speakerData=speaker;
            this.dialogueData=dialogue;
            this.commandData=(string.IsNullOrWhiteSpace(commands) ? null : new DL_COMMAND_DATA(commands));
        }
    }
}

