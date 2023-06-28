using UnityEngine;
using UnityEngine.EventSystems;

public class DragButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject clonePrefab; // Prefab do clone do botão

    private GameObject cloneButton; // Referência ao clone do botão
    private Canvas canvas; // Referência ao componente Canvas
    private RectTransform rectTransform; // Referência ao componente RectTransform

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Instancia o clone do botão
        cloneButton = Instantiate(clonePrefab, canvas.transform);
        RectTransform cloneRectTransform = cloneButton.GetComponent<RectTransform>();
        cloneRectTransform.position = rectTransform.position;

        // Desabilita o botão original enquanto arrasta o clone
        gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Atualiza a posição do clone de acordo com o ponteiro (mouse)
        cloneButton.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Destroi o clone do botão
        Destroy(cloneButton);

        // Reabilita o botão original após o término do arrasto
        gameObject.SetActive(true);
    }
}
