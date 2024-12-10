using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TESTING;
using CHARACTER;
using DIALOGUE;
using TMPro;

public class TestCharacters : MonoBehaviour
{
    void Start()
    {
        
        // Character Sparks = CharacterManager.instance.CreateCharacter("Миссис Спаркс");
        // Character Sparks2 = CharacterManager.instance.CreateCharacter("Миссис Спаркс");
        // Character Breadberry = CharacterManager.instance.CreateCharacter("Сара Брэдбери");

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        Character Stuard = CharacterManager.instance.CreateCharacter("Элизабет Стюард");
        Character Sparks = CharacterManager.instance.CreateCharacter("Миссис Спаркс");
        Character Breadberry = CharacterManager.instance.CreateCharacter("Сара Брэдбери");
        
        List<string> lines = new List<string>()
        {
            "Stuard \"Hi!\"",
            "This is a line",
            "And anothe",
            "And last one"
        };
        yield return Stuard.Say(lines);

        Stuard.SetNameColor(Color.red);
        Stuard.SetDialogueColor(Color.green);

        yield return Stuard.Say(lines);

        Stuard.ResetConfigurationData();

        yield return Stuard.Say(lines);

        lines = new List<string>()
        {
            "Sparks \"Hi!\"",
            "This is a line",
            "And anothe",
            "And last one"
        };
        yield return Sparks.Say(lines);
        yield return Breadberry.Say("And last one");

        Debug.Log("Finished");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
