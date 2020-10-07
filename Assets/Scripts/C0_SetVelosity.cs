using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.Events;

public class C0_SetVelosity : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Require require = new Require ();
        [System.Serializable] public class Require {
            public Vector2 velosity = default;
            public float maxForce;
        }

        public Optional optional = new Optional ();
        [System.Serializable] public class Optional {
            public bool ignoreMass = true;

        }

    }
    // * ---------------------------------- 


    [SerializeField, ReadOnly] private Vector2 velosity;
    [SerializeField, ReadOnly] private Vector2 force;
    private Vector2 position;

    private void FixedUpdate () {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
        float deltaTime = Time.fixedDeltaTime;

        Vector2 targetV = default;
        Vector2 AcAdd = default;


        Main (setting, rigidbody, deltaTime, ref AcAdd, out targetV);




        bool ignoreMass = setting.optional.ignoreMass;
        float massScale = ignoreMass ? rigidbody.mass : 1;
        rigidbody.AddForce (AcAdd * massScale);




        velosity = rigidbody.velocity;
        position = rigidbody.position;
        force = AcAdd;
    }



    //* Private Method
    private static void Main (Setting normalMode, Rigidbody2D rigidbody, float deltaTime,
        ref Vector2 AcAdd, out Vector2 targetV
    ) {

        targetV = normalMode.require.velosity;
        var currVonDir = rigidbody.velocity.Project (targetV);
        float maxForce = normalMode.require.maxForce;
        float maxAcc = maxForce;
        Vector2 deltaV = targetV - currVonDir;
        var accelAdd = deltaV / deltaTime;
        accelAdd = accelAdd.ClamMax (maxAcc);

        AcAdd += accelAdd;
    }

}