using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TypedInfo : MonoBehaviour
{
    private static string leaveMeAloneImTyping = "";
    private static Text thingToPutItOn;

    private static TypedInfo instance;





    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// Type message to target text output
    /// </summary>
    /// <param name="message"></param>
    /// <param name="textOutput"></param>
    public static void TypeMessage(string message, Text textOutput, System.Action<bool> complete)
    {
        leaveMeAloneImTyping = message;
        thingToPutItOn = textOutput;

        instance.StartCoroutine(PrintText(flag =>
        {
            if(flag)
            {
                complete(true);
            }
            complete(false);
        }));
    }

    private static IEnumerator PrintText(System.Action<bool> flag)
    {
        foreach(char letter in leaveMeAloneImTyping.ToCharArray())
        {
            thingToPutItOn.text += letter;


            flag(false);
            yield return false;
            yield return new WaitForSeconds(0.075f);
        }

        flag(true);
        yield return true;
    }
}
