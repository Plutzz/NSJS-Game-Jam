using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


namespace PlayerMovement {
    public class PlayerMovement : MonoBehaviour, IPlayerController
    {
        public Vector3 Velocity { get; private set; }
        public FrameInput Input { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector3 RawMovement { get; private set; }
        public bool Grounded => colDown;

        private Vector3 lastPosition;
        private float currentHorizontalSpeed, currentVerticalSpeed;

        //Make sure colliders are fully established when update starts
        private bool _active;
        void Awake() => Invoke(nameof(Activate), 0.5f);
        void Activate() => _active = true;

        private void Update()
        {
            if (!_active) return;

            Velocity = (transform.position - lastPosition) / Time.deltaTime;
            lastPosition = transform.position;

            GatherInput();
            RunCollisionChecks();

            CalculateWalk(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Actually perform the axis movement
        }



        #region Player Input
        private void GatherInput()
        {
            Input = new FrameInput
            {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal")
            };

            if(Input.JumpDown)
            {
                lastJumpPressed = Time.time;
            }

        }
        #endregion

        #region Collisions

        [Header("COLLISIONS")]
        [SerializeField] private Bounds characterBounds;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private int detectorCount = 3;
        [SerializeField] private float detectionRayLength = 0.1f;
        [SerializeField][Range(0f, 0.3f)] private float rayBuffer = 0.05f; // prevents side rays from hitting the ground

        private RayRange raysUp, raysRight, raysDown, raysLeft;
        private bool colUp, colRight, colDown, colLeft;

        private float timeLeftGrounded;

        private void RunCollisionChecks()
        {
            CalculateRayRanged();

            // Ground
            LandingThisFrame = false;
            var _groundedCheck = RunDetection(raysDown);
            if (colDown && !_groundedCheck) timeLeftGrounded = Time.time; // Only trigger when first leaving
            else if (!colDown && _groundedCheck)
            {
                coyoteUsable = true; // Only trigger when first touching
                LandingThisFrame = true;
            }

            colDown = _groundedCheck;

            // The rest
            colUp = RunDetection(raysUp);
            colLeft = RunDetection(raysLeft);
            colRight = RunDetection(raysRight);

            bool RunDetection(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, detectionRayLength, groundLayer));
            }
        }


        private void CalculateRayRanged()
        {
            var _b = new Bounds(transform.position + characterBounds.center, characterBounds.size);

            raysDown = new RayRange(_b.min.x + rayBuffer, _b.min.y, _b.max.x - rayBuffer, _b.min.y, Vector2.down);
            raysUp = new RayRange(_b.min.x + rayBuffer, _b.max.y, _b.max.x - rayBuffer, _b.max.y, Vector2.up);
            raysLeft = new RayRange(_b.min.x, _b.min.y + rayBuffer, _b.min.x, _b.max.y - rayBuffer, Vector2.left);
            raysRight = new RayRange(_b.max.x, _b.min.y + rayBuffer, _b.max.x, _b.max.y - rayBuffer, Vector2.right);
        }

        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        {
            for (var i = 0; i < detectorCount; i++)
            {
                var t = (float)i / (detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        private void OnDrawGizmos()
        {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + characterBounds.center, characterBounds.size);

            // Rays
            if (!Application.isPlaying)
            {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { raysUp, raysRight, raysDown, raysLeft })
                {
                    foreach (var point in EvaluateRayPositions(range))
                    {
                        Gizmos.DrawRay(point, range.Dir * detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            Gizmos.color = Color.red;
            var move = new Vector3(currentHorizontalSpeed, currentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + characterBounds.center + move, characterBounds.size);
        }


        #endregion

        #region Walk

        [Header("WALKING")][SerializeField] private float acceleration = 90;
        [SerializeField] private float moveClamp = 13;
        [SerializeField] private float deAcceleration = 60f;
        [SerializeField] private float apexBonus = 2;
        [SerializeField] private float movementMultiplier = 100;

        private void CalculateWalk()
        {
            if (Input.X != 0)
            {
                //Set horizontal move speed
                currentHorizontalSpeed = Input.X * acceleration * movementMultiplier * Time.deltaTime;

                // Clamp by max movement per frame
                currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -moveClamp, moveClamp);

                // Apply bonus at apex of the jump
                var _tempApexBonus = Mathf.Sign(Input.X) * apexBonus * apexPoint;
                currentHorizontalSpeed += _tempApexBonus * Time.deltaTime;
            }

            else
            {
                // No input = slow player down
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, deAcceleration * Time.deltaTime);
            }


            if (currentHorizontalSpeed > 0 && colRight || currentHorizontalSpeed < 0 && colLeft)
            {
                currentHorizontalSpeed = 0;
            }


        }

        #endregion

        
        #region Gravity
        [Header("GRAVITY")][SerializeField] private float fallClamp = -40f;
        [SerializeField] private float minFallSpeed = 80f;
        [SerializeField] private float maxFallSpeed = 120f;
        private float fallSpeed;

        private void CalculateGravity()
        {
            if (colDown)
            {
                // Move out of the ground
                if (currentVerticalSpeed < 0) currentVerticalSpeed = 0;
            }
            else
            {
                // Add downward force while ascending if we ended the jump early
                var _fallSpeed = endedJumpEarly && currentVerticalSpeed > 0 ? fallSpeed * jumpEndEarlyGravityModifier : fallSpeed;

                // Fall
                currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (currentVerticalSpeed < fallClamp) currentVerticalSpeed = fallClamp;
            }
        }
        #endregion
        

        #region Jump
        [Header("JUMPING")][SerializeField] private float jumpHeight = 30;
        [SerializeField] private float jumpApexThreshold = 10f;
        [SerializeField] private float coyoteTimeThreshold = 0.1f;
        [SerializeField] private float jumpBuffer = 0.1f;
        [SerializeField] private float jumpEndEarlyGravityModifier = 3;
        private bool coyoteUsable;
        private bool endedJumpEarly = true;
        private float apexPoint; // Becomes 1 at the apex of a jump
        private float lastJumpPressed;
        private bool canUseCoyote => coyoteUsable && !colDown && timeLeftGrounded + coyoteTimeThreshold > Time.time;
        private bool hasBufferedJump => colDown && lastJumpPressed + jumpBuffer > Time.time;

        private void CalculateJumpApex()
        {
            if (!colDown)
            {
                // Gets stronger the closer to the top of the jump
                apexPoint = Mathf.InverseLerp(jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                fallSpeed = Mathf.Lerp(minFallSpeed, maxFallSpeed, apexPoint);
            }
            else
            {
                apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (Input.JumpDown && canUseCoyote || hasBufferedJump)
            {
                currentVerticalSpeed = jumpHeight;
                endedJumpEarly = false;
                coyoteUsable = false;
                timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
            }
            else
            {
                JumpingThisFrame = false;
            }

            // End the jump early if button released
            if (!colDown && Input.JumpUp && !endedJumpEarly && Velocity.y > 0)
            {
                currentVerticalSpeed = 0;
                endedJumpEarly = true;
            }

            if (colUp)
            {
                if (currentVerticalSpeed > 0) currentVerticalSpeed = 0;
            }
        }
        #endregion

        #region Move
        [Header("MOVE")]
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int freeColliderIterations = 10;
        [SerializeField] private int nudgeAmount = 2;

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter()
        {
            var pos = transform.position + characterBounds.center;
            RawMovement = new Vector3(currentHorizontalSpeed, currentVerticalSpeed); // Used externally
            var move = RawMovement * Time.deltaTime;
            var furthestPoint = pos + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            var hit = Physics2D.OverlapBox(furthestPoint, characterBounds.size, 0, groundLayer);
            if (!hit)
            {
                transform.position += move;
                return;
            }

            // otherwise increment away from current pos; see what closest position we can move to
            var positionToMoveTo = transform.position;
            for (int i = 1; i < freeColliderIterations; i++)
            {
                // increment to check all but furthestPoint
                var t = (float)i / freeColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, characterBounds.size, 0, groundLayer))
                {
                    transform.position = positionToMoveTo;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i <= 1)
                    {
                        if (currentVerticalSpeed < 0) currentVerticalSpeed = 0;
                        var dir = transform.position - hit.bounds.center;
                        transform.position += dir.normalized * move.magnitude * nudgeAmount;
                    }

                    return;
                }

                positionToMoveTo = posToTry;
            }
        }
        #endregion






















    }

}
