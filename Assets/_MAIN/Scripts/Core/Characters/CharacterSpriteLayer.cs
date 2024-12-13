using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;
using UnityEngine.UI;

public class CharacterSpriteLayer
{
    private CharacterManager characterManager=>CharacterManager.instance;

    public int layer{get; private set;} = 0;
    public Image render {get; private set;} = null;
    public CanvasGroup renderCG => render.GetComponent<CanvasGroup>();

    private List<CanvasGroup> oldRenders = new List<CanvasGroup>();

    public CharacterSpriteLayer (Image defaultRender, int layer = 0)
    {
        render = defaultRender;
        this.layer=layer;
    }

    public void SetSprite(Sprite sprite)
    {
        render.sprite=sprite;
    }
}
