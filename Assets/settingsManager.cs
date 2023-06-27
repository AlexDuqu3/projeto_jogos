using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    public void openSettings() {
        canvas.GetComponent<Canvas>().enabled = true;

    }

    public void quit()
    {
        canvas.GetComponent<Canvas>().enabled = false;
    }
}
