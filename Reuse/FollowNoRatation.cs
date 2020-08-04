using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNoRatation : MonoBehaviour
{

    Transform toFollow;


    // Update is called once per frame
    void Update()
    {
        transform.position = toFollow.position;
    }
}
