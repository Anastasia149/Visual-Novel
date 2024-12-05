using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DIALOGUE;

namespace DIALOGUE
{
    [System.Serializable]
    public class NameContainer
    {
        // Ссылка на корневой объект UI-компонента, отображающего имя говорящего.
        [SerializeField] private GameObject root;

        // Ссылка на текстовое поле для отображения имени.
        [SerializeField] private TextMeshProUGUI nameText;

        // Метод для отображения имени говорящего.
        public void Show(string nameToShow = "")
        {
            // Включаем корневой объект, чтобы он стал видимым.
            root.SetActive(true);

            // Если передано имя для отображения, обновляем текст в компоненте TextMeshPro.
            if (nameToShow != string.Empty)
            {
                nameText.text = nameToShow;
            }
        }

        // Метод для скрытия имени говорящего.
        public void Hide()
        {
            // Отключаем корневой объект, чтобы скрыть имя.
            root.SetActive(false);
        }
    }
}
