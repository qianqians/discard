using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventTriggerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("event trigger handler start!");

        EventTrigger.Entry on_click = new EventTrigger.Entry();
        on_click.eventID = EventTriggerType.PointerClick;
        on_click.callback = new EventTrigger.TriggerEvent();
        on_click.callback.AddListener(onClick);

        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        trigger.triggers.Add(on_click);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick(BaseEventData eventData)
    {
        Debug.Log("点击");
    }
}
