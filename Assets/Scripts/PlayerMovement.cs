using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 Velocity;

    private bool jumpDown;
    private bool jumpUp;
    private float xAxis;
    private Vector3 lastPosition;
    private float currentHorizontalSpeed, currentVerticalSpeed;


    private void Update()
    {
        GatherInput();

        CalculateWalk(); // Horizontal Movement
    }



    #region Player Input
    private void GatherInput()
    {
    
        jumpDown = Input.GetButtonDown("Jump");

        Debug.Log(jumpDown);

        jumpUp = Input.GetButtonUp("Jump");

        Debug.Log(jumpUp);

        xAxis = Input.GetAxisRaw("Horizontal");

    }
    #endregion

    #region Collisions

    [Header("COLLISIONS")]
    [SerializeField] private Bounds characterBounds;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int detectorCount = 3;
    [SerializeField] private float detectionRayLength = 0.1f;
    [SerializeField][Range(0.1f, 0.3f)] private float rayBuffer = 0.1f;

    #endregion

    #region Walk

    [Header("WALKING")][SerializeField] private float acceleration = 90;
    [SerializeField] private float moveClamp = 13;
    [SerializeField] private float deAcceleration = 60f;
    [SerializeField] private float apexBonus = 2;

    private void CalculateWalk()
    {
        if(xAxis != 0)
        {
            currentHorizontalSpeed = xAxis * acceleration * Time.deltaTime;

            currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -moveClamp, moveClamp);

            var _tempApexBonus = Mathf.Sign(xAxis) * apexBonus * apexPoint;
            currentHorizontalSpeed += _tempApexBonus * Time.deltaTime;
        }

    }

    #endregion

    #region Jump
    private float apexPoint; // Becomes 1 at apex of a jump
    #endregion
}
