using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    private float speed =1;
    private float x;
    private float y;
    void Start()
    {
       x = this.gameObject.GetComponent<Transform>().position.x;
         y = this.gameObject.GetComponent<Transform>().position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, 0,Time.deltaTime * speed);
        if (transform.position.z <=-15)
        {
            transform.position = new Vector3(x, y, 15f);
        }
    }
}
