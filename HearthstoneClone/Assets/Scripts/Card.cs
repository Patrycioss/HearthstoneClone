using System.Collections;
using System.Collections.Generic;
using Cards.Interfaces.MouseOver;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Card : MonoBehaviour, IMouseOver
{
    public bool IsHovering { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && IsHovering)
        {
            Debug.Log("Clicked on card");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ja");
    }

    
    public void OnStartHover()
    {
        Debug.Log("Started hovering");
        
    }

    public void OnEndHover()
    {
        Debug.Log("Stoped hovering");
    }
}
