using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class HighScoreUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject highscoreUIElementPrefab;
    [SerializeField] Transform elementWrapper;

    List<GameObject> uiElements = new List<GameObject>();


    public void ShowPanel()
    {
        panel.GetComponent<Canvas>().enabled=true;
        //Debug.Log(HighScoreHandler.Instance.highScorelist.Count);
        UpdateUI(HighScoreHandler.Instance.highScorelist);
    }



    public void ClosePanel()
    {
        panel.GetComponent<Canvas>().enabled = false;
    }

    public void UpdateUI(List<HighScoreElement> list)
    {

        //Debug.Log(list.Count.ToString());
        //Debug.Log(list[0].date);

        for (int i = 0; i < list.Count; i++)
        {
            HighScoreElement element = list[i];

            if (element.score > 0)
            {
                if (i>= uiElements.Count)
                {
                    var inst = Instantiate(highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper);

                    uiElements.Add(inst);
                }

                //Debug.Log(uiElements.Count);

                var text = uiElements[i].GetComponentsInChildren<Text>();
                text[0].text = element.score.ToString();
                text[1].text = element.date.ToString();

                uiElements[i].transform.localScale = Vector3.one;

            }
        }
    }
}
