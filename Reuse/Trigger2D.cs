using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger2D : MonoBehaviour
{

    public UnityEvent TriggerEnter;
    public UnityEvent TriggerStay;
    public UnityEvent TriggerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerExit.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerStay.Invoke();
    }
}
