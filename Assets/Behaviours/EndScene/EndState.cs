using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndState : MonoBehaviour
{
    [SerializeField]
    private Image sceneCover;
    [SerializeField]
    private Image textCover;
    [SerializeField]
    private Text textOutput;
    [SerializeField]
    private string message = "Im a very special message about the herotic pigeons that served in WW1";
    
    
    
    public bool ThePigeonIsGone { get; set; }




    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        ThePigeonIsGone = false;


        //sceneCover = GetComponentInChildren<Image>();
        //if(!sceneCover)
        //{
        //    Debug.Log("ERROR: sceneCover Image object not attached");
        //    return;
        //}
        //
        //textCover = GetComponentInChildren<Image>();
        //if(!textCover)
        //{
        //    Debug.Log("ERROR: textCover Image object not attached");
        //    return;
        //}

        textOutput = GetComponentInChildren<Text>();
        if(!sceneCover)
        {
            Debug.Log("ERROR:TextOutput object not attached");
            return;
        }
    }


    private void Update()
    {
        if(ThePigeonIsGone || Input.GetKeyDown(KeyCode.End))
        {
            ThePigeonIsGone = false;
            StartCoroutine(FadeOut(sceneCover, flag =>
            {
                if(flag)
                {
                    TypedInfo.TypeMessage(message, textOutput, allTextDisplayed =>
                    {
                        if(allTextDisplayed)
                        {
                            LoadMainMenu();
                        }
                    });
                }
            }));
        }
    }


    private void LoadMainMenu()
    {
        Debug.Log("MEssage finished");

        StartCoroutine(FadeOut(textCover, flag => 
        { 
            // TODO: Load Main Menu
            Debug.Log("Text Fade Out"); 
       }));
    }




    private IEnumerator FadeOut(Image image, System.Action<bool> complete)
    {
        Image blackBox = image;


        while(true)
        {
            var color = blackBox.color;
            var alpha = color.a;
            var newAlpha = Mathf.LerpUnclamped(alpha, 1f, 0.5f * Time.deltaTime);

            blackBox.color = new Color(color.r, color.g, color.b, newAlpha);
            
            if(newAlpha > 0.95f) break;

            complete(false);
            yield return false;
        }
        complete(true);
        yield return true;

    }

}
