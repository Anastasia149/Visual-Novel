using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;
using Unity.VisualScripting;
using UnityEditor.UI;
using System;
using UnityEngine.UI;

public class Character_Sprite : Character
{
    private const string SPRITE_RENDERED_PARENT_NAME = "Renderes";
    private CanvasGroup rootcG => root.GetComponent<CanvasGroup>();

    public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

    private string artAssetsDirectory="";

    // Конструктор для создания персонажа-спрайта
    public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
    {
        rootcG.alpha=0;
        artAssetsDirectory = rootAssetsFolder + "/Images";

        GetLayers();
        Debug.Log($"Created Sprite Character: '{name}'");
    }

    private void GetLayers()
    {
        Transform rendererRoot = animator.transform.Find(SPRITE_RENDERED_PARENT_NAME);

        if (rendererRoot == null)
            return;
        
        for (int i = 0; i < rendererRoot.childCount; i++)
        {
            Transform child = rendererRoot.transform.GetChild(i);

            Image rendererImage = child.GetComponent<Image>();

            if(rendererImage !=null)
            {
                CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                layers.Add(layer);
                child.name = $"Layer: {i}";
            }
        }
    }

    public void SetSprite(Sprite sprite, int layer = 0)
    {
        layers[layer].SetSprite(sprite);
    }

    public Sprite GetSprite(string spriteName)
    {
        if(config.characterType == CharacterType.SpriteSheet)
        {
            return null;
        }
        else
        {
            return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
        }
    }

    public override IEnumerator ShowingOrHiding(bool show)
    {
        float targetAlpha = show ? 1f : 0;
        CanvasGroup self = rootcG;

        while (self.alpha!=targetAlpha)
        {
            self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime);
            yield return null;
        }

        co_revealing = null;
        co_hiding = null;
    }
}
