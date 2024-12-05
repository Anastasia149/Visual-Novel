using CHARACTER;
using UnityEngine;
using DIALOGUE;
using TMPro;

// Атрибут CreateAssetMenu позволяет создать объект ScriptableObject через меню в Unity Editor.
[CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
public class DialogueSystemConfigurationSO : ScriptableObject
{
    // Ссылка на объект конфигурации персонажей, который хранит информацию о всех персонажах в диалогах.
    public CharacterConfigSO characterConfigurationAsset;

    // Цвет по умолчанию для текста в диалоге.
    public Color defaultTextColor = Color.white;

    // Шрифт по умолчанию, используемый в диалогах.
    public TMP_FontAsset defaultFont;
}
