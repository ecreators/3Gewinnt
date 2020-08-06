using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ZellInhaltComponent : MonoBehaviour
{
    private const float MINIMUM_DISTANCE = 0.1f;

    public EContentType contentType = EContentType.APPLE;

    public ZelleComponent Cell { get; set; }

    public static bool DisableGravityAll { get; set; } = false;
    private Rigidbody2D RigidBody { get; set; }
    private float GravityScale { get; set; }
    public bool NotMoving { get; set; }

    private void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        GravityScale = RigidBody.gravityScale;
    }

    private void Update()
    {
        var gravityScale = DisableGravityAll == false ? GravityScale : 0;
        if (gravityScale != RigidBody.gravityScale)
        {
            RigidBody.gravityScale = gravityScale;
        }

        UpdateMoveDetection();
    }

    private void UpdateMoveDetection()
    {
        if (DisableGravityAll == false)
        {
            if (NotMoving == false && RigidBody.velocity == Vector2.zero)
            {
                NotMoving = TestAtCell();

                // snap to cell position
                if (NotMoving)
                {
                    transform.position = Cell.transform.position;
                }
            }
            else if (NotMoving == true && RigidBody.velocity != Vector2.zero)
            {
                NotMoving = false;
            }

            bool TestAtCell() => Cell != null && (Cell.transform.position - transform.position).magnitude <= MINIMUM_DISTANCE;
        }
    }
}