using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private GameObject newObject;
    bool newObjectCreated = false;
    // public void onPointerDown()
    // {
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Button Pressed");
        // create a new game object from instatiating the own game object
        newObject = Instantiate(gameObject);

        newObject.SetActive(false);

        newObjectCreated = true;
    }

    // public void onPointerUp()
    // {
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Button Released");
        newObjectCreated = false;
        newObject.SetActive(true);

        // create vector 3 from the position of the pointer
        Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pointerPosition.z = 1f;
        newObject.transform.position = pointerPosition;

        Debug.Log("last position of new object: " + newObject.transform.position);
    }

    // public void onPointerDrag()
    // {
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Button Dragged");
        // set the position of the new object to the position of pointer
        newObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pointerPosition.z = 1f;
        newObject.transform.position = pointerPosition;
         Debug.Log("position of new object: " + newObject.transform.position);
    }

    // void Update()
    // {
    //     if (newObjectCreated)
    //     {
    //         // set the position of the new object to the position of pointer
    //         onPointerDrag();
    //     }
    // }
}
