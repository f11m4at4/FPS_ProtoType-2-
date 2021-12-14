using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float rotspeed = 200f;
    float xIn_yRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        xIn_yRot += mouse_X * rotspeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, xIn_yRot, 0);

    }
}
