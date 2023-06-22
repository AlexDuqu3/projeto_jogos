using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeOverlay : MonoBehaviour
{

    private GameObject tower;
    private bool isColliding;
    private void Awake()
    {
        isColliding = false;
        tower = transform.parent.gameObject;
        transform.position= tower.transform.position;
        transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        gameObject.SetActive(false);
    }
  
    public bool IsColliding()
    {
        return isColliding;
    }
   private void Show()
    {
        ChangeGameObjectOpacity(1f);
        RefreshRangeVisual();
    }
    private void Hide()
    {
        ChangeGameObjectOpacity(0f);
        RefreshRangeVisual();
    }

    private void RefreshRangeVisual()
    {
        Tower towerClass= tower.GetComponent<Tower>();
        float circleScale = towerClass.GetRange();
        transform.localScale = new Vector3(circleScale, circleScale, 1f);
    }
    private void OnMouseEnter()
    {
        isColliding = true;
        Show();
    }
    private void OnMouseExit()
    {
        isColliding = false;
        Hide();
    }

    private void ChangeGameObjectOpacity(float opacity)
    {
        Renderer ghostRenderer = gameObject.GetComponent<Renderer>();
        Color meshColor = ghostRenderer.material.color;

        //Set Alpha
        float alpha = opacity;
        meshColor.a = alpha;

        //Apply the new color to the material
        ghostRenderer.material.color = meshColor;
    }
}
