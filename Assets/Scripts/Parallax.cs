using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    private float startPos;
    [SerializeField] GameObject Cam;
    public float parallaxEffect;

    private void Start()
    {
        startPos = transform.position.x;
    }

    private void FixedUpdate()
    {
        float dist = (Cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
