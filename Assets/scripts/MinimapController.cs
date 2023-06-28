using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public GameObject minimap;
    public Sprite showMinimapSprite; // Sprite para o botão quando o minimapa é mostrado
    public Sprite hideMinimapSprite; // Sprite para o botão quando o minimapa é ocultado

    private Button button;
    // private float hideOffset = 145f; // A quantidade de deslocamento para cima ao ocultar o minimapa
    private bool isMinimapVisible = true;
    // private Vector3 originalButtonPosition;
    private Image buttonImage; // Referência ao componente Image do botão

    private void Start()
    {
        button = GetComponent<Button>();
        // originalButtonPosition = button.transform.position;
        buttonImage = GetComponent<Image>();

        // Debug.Log("Button position: " + button.transform.position);
    }

    public void ToggleMinimapVisibility()
    {
        isMinimapVisible = !isMinimapVisible;

        if (isMinimapVisible)
        {
            minimap.SetActive(true);
            // button.transform.position = originalButtonPosition;
            buttonImage.sprite = hideMinimapSprite; // Atualiza a imagem do botão para o sprite de ocultar minimapa
        }
        else
        {
            // Vector3 newButtonPosition = originalButtonPosition;
            // newButtonPosition.y += hideOffset; // Desloca o botão para cima

            minimap.SetActive(false);
            // button.transform.position = newButtonPosition;
            buttonImage.sprite = showMinimapSprite; // Atualiza a imagem do botão para o sprite de mostrar minimapa
        }

        // Debug.Log("Minimap visibility toggled. New visibility: " + isMinimapVisible);
        // Debug.Log("Button position: " + button.transform.position);
    }
}
