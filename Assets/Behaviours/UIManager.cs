using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [SerializeField] Text delivered_count_text;
    [SerializeField] float deliver_text_time;
    bool displaying_delivered;

    [SerializeField] Image end_screen_bg;
    [SerializeField] Text end_screen_text;

    string[] end_screen_messages = { "In 1914 during the First Battle of the Marne, the French army advanced 72 pigeon lofts with the troops.",
        "The US Army Signal Corps used 600 pigeons in France alone.",
        "On her final mission in October 1918, Blue Check hen Cher Ami delivered a message despite having been injured from gunfire.",
        "During World War II, the United Kingdom used about 250, 000 homing pigeons for many purposes, including communicating with those behind enemy lines.",
        "Pigeons were considered an essential element of naval aviation communication when the first United States aircraft carrier USS Langley was commissioned on 20 March 1922.",
        "United States Navy aviators maintained 12 pigeon stations in France with a total inventory of 1, 508 pigeons when World War I ended.",
        "During World War II, pigeons were trained to deliver small explosives or bioweapons to precise targets."};

    bool game_over = false;

    private void Start()
    {
        end_screen_bg.GetComponent<FadableGraphic>().FadeOut(1);
    }

    private void Update()
    {
        if (game_over)
        {
            if (Input.GetButton("Controller 1 - A"))
                SceneManager.LoadScene(1);

            if (Input.GetButton("Controller 1 - B"))
                SceneManager.LoadScene(0);
        }
    }

    public void updateUIText(int _newTextInt)
    {
        delivered_count_text.text = "Messages Delivered: " + _newTextInt.ToString();
        StartCoroutine("showDelivered");
    }

    public void displayEndScreen()
    {
        end_screen_text.text = end_screen_messages[Random.Range(0, end_screen_messages.Length - 1)];

        end_screen_bg.GetComponent<FadableGraphic>().FadeIn(1);
        end_screen_text.GetComponent<FadableGraphic>().FadeIn(3);
        StartCoroutine("endGame");
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(1);

        game_over = true;
        delivered_count_text.GetComponent<FadableGraphic>().FadeIn(deliver_text_time / 3);

        GameManager.scene.mobile_control.state = MobileControl.ControlState.GAMEOVER;
    }

    IEnumerator showDelivered()
    {
        displaying_delivered = true;

        delivered_count_text.GetComponent<FadableGraphic>().FadeIn(deliver_text_time / 3);

        yield return new WaitForSeconds(deliver_text_time);

        delivered_count_text.GetComponent<FadableGraphic>().FadeOut(deliver_text_time / 3);
    }
}
