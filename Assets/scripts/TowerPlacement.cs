using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int price;
    private Text priceText;
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    public int Price { get => price; }

    private void Awake()
    {
        priceText = transform.Find("Price").GetComponent<Text>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price + "<color=lime>$</color>";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
