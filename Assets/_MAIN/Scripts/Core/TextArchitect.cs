using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class TextArchitect
{
    //используемый для текста в интерфейсе Unity
   private TextMeshProUGUI tmpro_ui;
   //используемый для текста в мировом пространстве (не в UI).
   private TextMeshPro tmpro_world;
   //роверяет, присвоен ли tmpro_ui; если это так, используется tmpro_ui, иначе — tmpro_world.
   public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui:tmpro_world;
   //Это свойство возвращает текущий текст
   public string currentText => tmpro.text;
   //Это строка, которая будет в конечном итоге отображена.
   public string targetText {get; private set;} = "";
   //Эта переменная хранит любой существующий текст, который может быть до нового текста
   public string preText {get; private set;} = "";
   private int preTextLength = 0;
   //Это свойство конкатенирует preText и targetText, чтобы вернуть полную строку, которая будет построена.
   public string fullTargetText => preText+targetText;
   public enum BuildMethod
   {
        instant, //Моментальное отображение текста.
        typewriter, //Отображение текста по одному символу за раз, как на печатной машинке.
        fade //Отображение текста с эффектом затухания.
   }
   public BuildMethod buildMethod = BuildMethod.typewriter; //Это поле определяет, какой метод отображения текста будет использоваться
   public Color textColor {get{return tmpro.color;} set{tmpro.color = value;}} //Это свойство позволяет получить и установить цвет текста в компоненте TMP.
   public float speed{get{return baseSpeed*speedMultiplier;}set {speedMultiplier=value;}} //Это скорость эффекта печатной машинки.
   private const float baseSpeed = 1; //speed она основана на значении baseSpeed
   private float speedMultiplier=1; //speed может быть умножена на speedMultiplier

    //Это свойство вычисляет, сколько символов должно быть обработано за один цикл в зависимости от скорости.
    public int charactersPerCycle {get {return speed<=2f?characterMultiplier:speed<=2.5f?characterMultiplier*2:characterMultiplier*3;}}
    private int characterMultiplier =1;

    //Это флаг, который может быть использован для ускорения или пропуска отображения текста
    public bool hurryUp = false;

    public TextArchitect (TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui=tmpro_ui;
    }

    public TextArchitect (TextMeshPro tmpro_world)
    {
        this.tmpro_world=tmpro_world;
    }

    //метод устанавливает targetText для отображения и запускает процесс построения текста.
    public Coroutine Build(string text)
    {
        preText="";
        targetText=text;

        Stop();
        //Запускает новый корутин (BuildProcess), который будет управлять процессом построения текста.
        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    //тот метод добавляет новый текст к текущему.
    public Coroutine Append(string text)
    {
        preText=tmpro.text;
        targetText=text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    //используется для остановки корутины, если это необходимо.
    private Coroutine buildProcess = null;
    //Это свойство возвращает булево значение, указывающее, выполняется ли в данный момент процесс построения текста.
    public bool isBuilding=>buildProcess !=null;

    //Этот метод останавливает любой текущий работающий корутин, который строит текст.
    public void Stop()
    {
        //корутин работает
        if(!isBuilding)
        {
            return;
        }
        //останавливает корутин и присваивает buildProcess значение null.
        tmpro.StopCoroutine(buildProcess);
        buildProcess=null;
    }
    // корутина, которая управляет процессом построения текста в зависимости от выбранного метода
    IEnumerator Building()
    {
        Prepear();

        switch(buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;
            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }
        OnComplete();
    }

    //Завершение
    private void OnComplete()
    {
        buildProcess=null;
        hurryUp = false;
    }

    //Этот метод принудительно завершает процесс отображения текста
    public void ForseComplit()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
            tmpro.maxVisibleCharacters=tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                tmpro.ForceMeshUpdate();
                break;
        }

        Stop();
        OnComplete();
    }

    private void Prepear()
    {
        switch(buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_Typewriter();
                break;
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }

    //Мгновенно показывает весь текст.
    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }

    //Подготавливает текст для эффекта печатной машинки 
    private void Prepare_Typewriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters=0;
        tmpro.text = preText;

        if (preText!="")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text+=targetText;
        tmpro.ForceMeshUpdate();
    }

    //Метод для подготовки эффекта затухания
    private void Prepare_Fade()
    {
        tmpro.text = preText;
        if (preText !="")
        {
            tmpro.ForceMeshUpdate();
            preTextLength = tmpro.textInfo.characterCount;
        }
        else
        {
            preTextLength=0;
        }
        

        tmpro.text = targetText;
        tmpro.maxVisibleCharacters = int.MaxValue;
        tmpro.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color colorVisable = new Color(textColor.r,textColor.g,textColor.b,1);
        Color colorHidden = new Color(textColor.r,textColor.g,textColor.b,0);

        Color32[] vertexColor = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            if (i<preTextLength)
            {
                for (int v = 0; v < 4; v++)
                {
                    vertexColor[charInfo.vertexIndex+v]=colorVisable;
                }
            }
            else
            {
                for (int v = 0; v < 4; v++)
                {
                    vertexColor[charInfo.vertexIndex+v]=colorHidden;
                }
            }
        }

        tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    //Управляет процессом отображения текста по одному символу за раз
    private IEnumerator Build_Typewriter()
    {
        while (tmpro.maxVisibleCharacters<tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters+= hurryUp ? charactersPerCycle * 5 : charactersPerCycle;
            yield return new WaitForSeconds(0.015f / speed);
        }
    }
    //Заготовка для эффекта затухания
    private IEnumerator Build_Fade()
    {
        int minRange = preTextLength;
        int maxRange = minRange+1;

        byte alphaThreshold = 15;

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color32[] vertexColor = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;
        float[] alphas = new float[textInfo.characterCount];

        while (true)
        {
            float fadeSpeed = ((hurryUp ? charactersPerCycle * 5 : charactersPerCycle)*speed)*4f;

            for (int i = minRange; i < maxRange; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                {
                    continue;
                }

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                alphas[i] = Mathf.MoveTowards(alphas[i],255,fadeSpeed);

                for (int v = 0; v < 4; v++)
                {
                    vertexColor[charInfo.vertexIndex+v].a=(byte)alphas[i];
                }

                if (alphas[i]>=255)
                {
                    minRange++;
                }
            }

            tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            bool lastCharacterIsInvisible = !textInfo.characterInfo[maxRange-1].isVisible;
            if (alphas[maxRange-1]>alphaThreshold||lastCharacterIsInvisible)
            {
                if(maxRange<textInfo.characterCount)
                    maxRange++;
                
                else if (alphas[maxRange-1]>=255||lastCharacterIsInvisible)
                    break;
                
            }

            yield return new WaitForEndOfFrame();
        }
        
        
    }
}
