using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 1500.0f;
    public float rotSpeed = 50.0f;
    private float verticalMovement;
    private float horizontalMovement;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion thing = transform.localRotation;
        //if(thing.eulerAngles.y == 0.0)
        //{
        //    move = new Vector3(0, 0, Input.GetAxis("Vertical"));
        //}
        //else if(thing.eulerAngles.y == 90.0)
        //{
        //    move = new Vector3(Input.GetAxis("Vertical"), 0, 0);
        //}
        //else if(thing.eulerAngles.y == 270.0)
        //{
        //    move = new Vector3(-Input.GetAxis("Vertical"), 0, 0);
        //}
        //else if (thing.eulerAngles.y == 180.0)
        //{
        //    move = new Vector3(0, 0, -Input.GetAxis("Vertical"));
        //}
        //move *= moveSpeed;
        //_controller.Move(move * Time.deltaTime);
        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    Rotate(-rotateDegrees);
        //}
        //else if(Input.GetKeyUp(KeyCode.D))
        //{
        //    Rotate(rotateDegrees);
        //}
        verticalMovement = Input.GetAxis("Vertical");
        horizontalMovement = Input.GetAxis("Horizontal");
        rb.velocity = (transform.forward * verticalMovement) * speed * Time.fixedDeltaTime;
        transform.Rotate((transform.up * horizontalMovement) * rotSpeed * Time.fixedDeltaTime);

    }

    //void Rotate(float degrees)
    //{
    //    var startRotation = transform.rotation;
    //    var startPosition = transform.position;
    //    transform.RotateAround(transform.position, Vector3.up, degrees);
    //    var endRotation = transform.rotation;
    //    var endPosition = transform.position;
    //    transform.rotation = startRotation;
    //    transform.position = startPosition;
    //    transform.rotation = endRotation;
    //    transform.position = endPosition;
    //}
}
