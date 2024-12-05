using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TESTING;
using CHARACTER;

public class TestCharacters : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Character Stuard = CharacterManager.instance.CreateCharacter("Элизабет Стюард");
        Character Sparks = CharacterManager.instance.CreateCharacter("Миссис Спаркс");
        Character Sparks2 = CharacterManager.instance.CreateCharacter("Миссис Спаркс");
        Character Breadberry = CharacterManager.instance.CreateCharacter("Сара Брэдбери");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
