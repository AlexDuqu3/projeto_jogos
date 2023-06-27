using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [SerializeField] public GameObject minimap;
    [SerializeField] public GameObject button;
    [SerializeField] public Sprite showMinimapSprite; // Sprite para o botão quando o minimapa é mostrado
    [SerializeField] public Sprite hideMinimapSprite; // Sprite para o botão quando o minimapa é ocultado
   
    private float hideOffset = 114f; // A quantidade de deslocamento para cima ao ocultar o minimapa
    private bool isMinimapVisible = true;
    private Vector3 originalButtonPosition;
    private Image buttonImage; // Referência ao componente Image do botão

    private void Start()
    {
        originalButtonPosition = button.transform.position;
        buttonImage = GetComponent<Image>();
    }

    public void ToggleMinimapVisibility()
    {
        isMinimapVisible = !isMinimapVisible;

        if (isMinimapVisible)
        {
            minimap.SetActive(true);
            button.transform.position = originalButtonPosition;
            buttonImage.sprite = hideMinimapSprite; // Atualiza a imagem do botão para o sprite de ocultar minimapa
        }
        else
        {
            Vector3 newButtonPosition = originalButtonPosition + new Vector3(0f, hideOffset, 0f);

            minimap.SetActive(false);
            button.transform.position = newButtonPosition;
            buttonImage.sprite = showMinimapSprite; // Atualiza a imagem do botão para o sprite de mostrar minimapa
        }

        Debug.Log("Minimap visibility toggled. New visibility: " + isMinimapVisible);
    }
}
