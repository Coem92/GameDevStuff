using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPathFollower : MonoBehaviour
{
    public float speed;
    public Vector3[] points;
    float count;
    int index = 0;
    float distance;
    bool reverse = false;
    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(points[index], points[index + 1]);
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime * speed;

        if (!reverse)
        {
            transform.position = Vector3.Lerp(points[index], points[index + 1], (count / distance));
            if (Vector3.SqrMagnitude(transform.position - points[index + 1]) < 0.01f)
            {
                if (points.Length > index+2)
                {
                    index++;
                    distance = Vector3.Distance(points[index], points[index + 1]);
                }
                else
                {
                    index++;
                    reverse = true;
                }
                count = 0;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(points[index], points[index - 1], (count / distance));
            if (Vector3.SqrMagnitude(transform.position - points[index - 1]) < 0.01f)
            {
                if ((index-2) >= 0)
                {
                    index--;
                    distance = Vector3.Distance(points[index], points[index -1]);
                }
                else
                {
                    index--;
                    reverse = false;
                }
                count = 0;
            }
        }
    }

    
}
