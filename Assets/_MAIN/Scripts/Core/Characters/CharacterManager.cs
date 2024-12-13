using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace CHARACTER
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; } // Синглтон, предоставляющий доступ к экземпляру класса
        private Dictionary<string, Character> characters = new Dictionary<string, Character>(); // Словарь для хранения персонажей по их имени

        // Получение конфигурации персонажей из диалоговой системы
        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigurationAsset;

        private const string CHARACTER_CASTING_ID = " as ";
        private const string CHARACTER_NAME_ID= "<charname>";
        public string characterRootPathFormat =>$"Characters/{CHARACTER_NAME_ID}";
        public string characterPrefabNameFormat =>$"Character -[{CHARACTER_NAME_ID}]";
        public string characterPrefabPathFormat => $"{characterRootPathFormat}/{characterPrefabNameFormat}";

        [SerializeField]private RectTransform _characterpanel = null;
        public RectTransform characterpanel => _characterpanel;

        private void Awake()
        {
            instance = this; // Устанавливаем текущий экземпляр как синглтон
        }

        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        public Character GetCharacter(string characterName, bool creatIfDoesNotExist = false)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                return characters[characterName.ToLower()];
            }
            else if (creatIfDoesNotExist)
            {
                return CreateCharacter(characterName);
            }

            return null;
        }

        // Метод для создания нового персонажа
        public Character CreateCharacter(string characterName)
        {
            // Проверяем, существует ли уже персонаж с таким именем
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create the character.");
                return null; // Возвращаем null, если персонаж уже существует
            }

            // Получаем информацию о персонаже
            CHARACTER_INFO info = GetCharacterInfo(characterName);

            // Создаем персонажа на основе информации
            Character character = CreateCharacterFronInfo(info);

            // Добавляем персонажа в словарь
            characters.Add(characterName.ToLower(), character);

            return character; // Возвращаем созданного персонажа
        }

        // Метод для получения информации о персонаже
        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO(); // Создаем экземпляр информации о персонаже

            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);
            result.name = nameData[0]; // Устанавливаем имя персонажа
            result.castingName = nameData.Length>1?nameData[1]:result.name;

            result.config = config.GetConfig(result.castingName); // Получаем конфигурацию персонажа из файла конфигурации

            result.prefab = GetPrefabForCharacter(result.castingName);

            result.rootCharacterFolder = FormatCharacterPath(characterPrefabPathFormat, result.castingName);

            return result; // Возвращаем заполненную информацию
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(characterPrefabPathFormat, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        public string FormatCharacterPath(string path, string characterName)=>path.Replace(CHARACTER_NAME_ID, characterName);

        // Метод для создания персонажа на основе информации
        private Character CreateCharacterFronInfo(CHARACTER_INFO info)
        {
            // Выбор типа персонажа на основе его конфигурации
            switch (info.config.characterType)
            {
                case Character.CharacterType.Text:
                    return new Character_Text(info.name, info.config); // Создаем текстового персонажа

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.name,info.config, info.prefab, info.rootCharacterFolder); // Создаем персонажа-спрайт

                case Character.CharacterType.Live2D:
                    return new Character_Live2D(info.name,info.config, info.prefab, info.rootCharacterFolder); // Создаем персонажа Live2D

                case Character.CharacterType.Model3D:
                    return new Character_Model3D(info.name,info.config, info.prefab, info.rootCharacterFolder); // Создаем 3D-модель персонажа

                default:
                    return null; // Возвращаем null для неподдерживаемого типа
            }
        }

        // Вспомогательный класс для хранения информации о персонаже
        private class CHARACTER_INFO
        {
            public string name = ""; // Имя персонажа
            public string castingName="";

            public string rootCharacterFolder = ""; 

            public CharacterConfigData config = null; // Конфигурация персонажа
            public GameObject prefab = null;
        }
    }
}