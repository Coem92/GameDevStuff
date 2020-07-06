using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Action()
    {
        Debug.Log("this is a base action");
    }
}
