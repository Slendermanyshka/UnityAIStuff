using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStuff : MonoBehaviour
{
    [SerializeField] float xValue = 1f;
    [SerializeField] float yValue = 1f;
    [SerializeField]float zValue = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, zValue);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -zValue);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-xValue, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(xValue, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, yValue, 0);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, -yValue, 0);
        }
       
    }
}
