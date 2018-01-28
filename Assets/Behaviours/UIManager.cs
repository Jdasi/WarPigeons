using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] Text delivered_count_text;
    [SerializeField] float deliver_text_time;
    bool displaying_delivered;

    public void updateUIText(int _newTextInt)
    {
        delivered_count_text.text = "Messages Delivered: " + _newTextInt.ToString();
        StartCoroutine("showDelivered");
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
