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

    bool game_over = false;

    private void Start()
    {
        end_screen_bg.GetComponent<FadableGraphic>().FadeOut(1);
    }

    private void Update()
    {
        if(game_over && Input.GetButton("Controller 1 - A"))
            SceneManager.LoadScene(1);
    }

    public void updateUIText(int _newTextInt)
    {
        delivered_count_text.text = "Messages Delivered: " + _newTextInt.ToString();
        StartCoroutine("showDelivered");
    }

    public void displayEndScreen()
    {
        end_screen_bg.GetComponent<FadableGraphic>().FadeIn(1);
        end_screen_text.GetComponent<FadableGraphic>().FadeIn(3);
        StartCoroutine("endGame");
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(1);
        game_over = true;
        delivered_count_text.GetComponent<FadableGraphic>().FadeIn(deliver_text_time / 3);
        yield break;
    }

    IEnumerator showDelivered()
    {
        displaying_delivered = true;

        delivered_count_text.GetComponent<FadableGraphic>().FadeIn(deliver_text_time / 3);
        yield return new WaitForSeconds(deliver_text_time);
        delivered_count_text.GetComponent<FadableGraphic>().FadeOut(deliver_text_time / 3);

        yield break;
    }
}
