using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TESTING;
using CHARACTER;
using DIALOGUE;
using TMPro;
using UnityEngine.Timeline;
using System;

public class TestCharacters : MonoBehaviour
{
    private Character CreateCharacter(string name)=>CharacterManager.instance.CreateCharacter(name);

    void Start()
    {
        //Character Sara = CharacterManager.instance.CreateCharacter("Сара");
        //Character Stuard_child = CharacterManager.instance.CreateCharacter("Stuard child");
        // Character Breadberry = CharacterManager.instance.CreateCharacter("Сара Брэдбери");

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        Character StuardChild = CreateCharacter("Стюард ребенок");
        Character_Sprite Sara = CreateCharacter("Сара") as Character_Sprite;

        StuardChild.Show();
        Sara.Show();

        Sprite SaraSprite = Sara.GetSprite("Сара, грустная");
        Debug.Log(SaraSprite == null ? "Sprite not found" : "Sprite found: " + SaraSprite.name);

        Sara.SetSprite(SaraSprite, 0);
        Debug.Log("Sprite applied to Sara");
        
        Sara.Show();

        yield return StuardChild.Say("Элизабет Стюард\"привет\"");

        yield return Sara.Say("Сара\"пока\"");

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
