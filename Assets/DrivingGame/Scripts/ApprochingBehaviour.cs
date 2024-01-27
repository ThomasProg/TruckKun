using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachingBehaviour : MonoBehaviour
{
    public float speed;
    public float velocity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed += velocity * Time.deltaTime;
        transform.Translate(Vector3.back * (Time.deltaTime * speed));
    }
}
