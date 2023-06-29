using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsButton : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    public void openSettings() {
        Debug.Log(canvas);
        canvas.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 0;
    }
}
