using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceController : MonoBehaviour
{
    private Collider2D coll;
    private Renderer rd;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rd = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowSpace();
    }

    void ShowSpace()
    {
        if (Input.GetButton("Jump"))
        {
            coll.enabled = false;
            rd.enabled = false;
        }
        else
        {
            coll.enabled = true;
            rd.enabled = true;
        }
    }
}