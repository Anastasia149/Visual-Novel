using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
 //класс-контейнер, хранящий ссылки на объекты интерфейса (UI), связанные с диалогами.
namespace DIALOGUE
{
   [System.Serializable]
   public class DialogueContainer
   {
      public GameObject root;
      public NameContainer nameContainer;
      public TextMeshProUGUI dialogueText;

      public void SetDialogueColor(Color color) => dialogueText.color=color;
      public void SetDialogueFont(TMP_FontAsset font)=> dialogueText.font=font;
   }
}

