using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C0_SetVelosity : MonoBehaviour {
    public Setting setting = new Setting ();
    [System.Serializable] public class Setting {
        public Require require = new Require ();
        [System.Serializable] public class Require {
            public Vector2 direction = default;
            public float speed;
            public float maxForce;
        }
        public Optional optional = new Optional ();
        [System.Serializable] public class Optional {
            public bool ignoreMass = true;
            public FractionFix frictionFix = new FractionFix ();
            [System.Serializable] public class FractionFix {
                public bool enabled = false;
                public float maxForce;
            }

        }

    }

    public Vector2 velosity = default;
    public Vector2 force;

    private void FixedUpdate () {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
        bool ignoreMass = setting.optional.ignoreMass;
        float massScale = ignoreMass ? rigidbody.mass : 1;
        var deltaTime = Time.fixedDeltaTime;
        Vector2 targetV = setting.require.direction.normalized * setting.require.speed;
        var currV = rigidbody.velocity.Project (targetV);
        Vector2 accAdd = AccAdd ();
        Vector2 AccAdd () {
            float maxForce = setting.require.maxForce;
            float maxAcc = maxForce;
            Vector2 deltaV = targetV - currV;
            var accelAdd = deltaV / deltaTime;
            accelAdd = accelAdd.ClamMax (maxAcc);
            return accelAdd;
        }


        Vector2 fricFixAcc = FrictionFixAcc ();
        Vector2 FrictionFixAcc () {
            Vector2 frictionAcc = default;
            if (setting.optional.frictionFix.enabled) {
                var s = setting.optional.frictionFix;
                Vector2 frameDeltaV = currV - velosity.Project (targetV);
                Vector2 tarDeltaV = force.Project (targetV) * deltaTime / rigidbody.mass;
                frictionAcc = (frameDeltaV - tarDeltaV) / deltaTime;
                frictionAcc = frictionAcc.ClamMax (s.maxForce);
            }
            return -frictionAcc;
        }




        Vector2 AccApply = accAdd + fricFixAcc;

        Vector2 forceAdd = AccApply * massScale;
        rigidbody.AddForce (forceAdd);



        velosity = currV;
        force = forceAdd;


    }

}