using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallax;
    [SerializeField] private float zPosition;

    private float length;
    private float xPosition;


    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    void FixedUpdate()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallax);
        float distanceToMove = cam.transform.position.x * parallax;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y, zPosition);

        if (distanceMoved > xPosition + length)
        {
            xPosition = xPosition + length;
        }
    }
}
