using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minX, maxX;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        //MARKER Method 1 tranditon method of move camera
        //transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);

        //MARKER Method 2 smoothly move camera
        //first parameter is current positon
        //second parameter is target position
        //third parameter is speed
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.position.x, transform.position.y, transform.position.z),
            smoothSpeed * Time.deltaTime);
        //MARKER limit the range
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
            transform.position.y, transform.position.z);
    }
}
