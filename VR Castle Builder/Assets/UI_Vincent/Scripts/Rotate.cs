using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 15;

    void Update()
    {
        transform.Rotate(new Vector3(0,5 * Time.deltaTime, 0));
    }
}
