using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float maxHeight = 3;
    public float minHeight = 1;
    public float jumpForce = 100;
    public float deForce = 40;
    public float maxSpeed = 20;
    public Vector2 jumpDirection = Vector2.up;
    public float remainDist;
    public float currV;



    private PhysicMathBundle.PhysicMath_DistanceToForce dtf;

    private Rigidbody2D body => gameObject.GetComponent<Rigidbody2D>();
    private float mass => body.mass;

    private void Awake()
    {
        ObjectState.OnStateAdd.Add(gameObject, StateName.OnGround, () =>
        {
            Stop();
        });
    }


    private void FixedUpdateAction()
    {
        float v;
        currV = body.velocity.ProjectToFloat(jumpDirection);
        v = PhysicMath.DistanceToForce(ref dtf, maxHeight, maxSpeed, jumpForce, deForce, currV, mass);
        body.AddForce(jumpDirection * v);

        remainDist = dtf.remainDist;

        if (remainDist < 0.01f)
        {
            Stop();
        }
    }

    private void Stop()
    {
        ObjectState.State.Remove(gameObject, StateName.Jump);
        BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
        dtf = null;
        enabled = false;
    }

    private void OnEnable()
    {
        dtf = null;
        BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
        ObjectState.State.Add(gameObject, StateName.Jump);

    }
    private void OnDisable()
    {
        if (dtf == null) { return; }
        dtf.targetDistance = minHeight;
    }





}
