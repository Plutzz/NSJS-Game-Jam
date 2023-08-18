using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor.Rendering;

public class EnemyPathfind : MonoBehaviour
{
    

    public float NextWayPointDistance;
    public float movementSpeed = 500f;
    public bool facingRight;

    private AIPath aiPath;
    private Transform target;
    private Path path;
    private int currentWaypoint = 0;
    //private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private GameObject player;
    private Transform enemyGraphics;
    

    public void Start()
    {
        player = PlayerController.playerController.gameObject;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.playerController.gameObject;
        target = player.transform;
        enemyGraphics = GetComponentInChildren<Transform>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path == null) return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            //reachedEndOfPath = true;
            return;
        }
        else
        {
            //reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * movementSpeed * Time.deltaTime * GetComponent<Enemy>().movementScale;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < NextWayPointDistance)
        {
            currentWaypoint++;
        }

        if(force.x >= 0.01f)
        {
            enemyGraphics.transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
        else if(force.x <= -0.01f)
        {
            enemyGraphics.transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }
}
