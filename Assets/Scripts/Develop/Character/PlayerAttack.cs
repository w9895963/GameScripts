using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterBundle;
using Global;
using Global.AttackBundle;
using Global.ObjectDynimicFunction;
using UnityEngine;
using UnityEngine.InputSystem;





public class PlayerAttack : IFunctionCreate, ILateCreate
{
    [System.Serializable]
    public class Data
    {
        public AttackType basicSlap = AttackType.HeroDefaultSlap;

    }


    #region Basic Fields ------------
    private FunctionManager fm;
    private StateFunc state;
    private GameObject gameObject;
    private Data data;
    #endregion
    // ** ---------------------------------- 


    public void OnCreate(FunctionManager functionManager)
    {
        this.fm = functionManager;
        var fm = functionManager;
        gameObject = fm.gameObject;


        data = fm.GetData<Data>(this);
        data = data == null ? new Data() : data;

    }


    public void LateCreate()
    {
        InputManager.GetInputAction(InputManager.InputName.Shot).performed += ShotInputAction;
        state = fm.GetFunction<StateFunc>();
    }

    private void ShotInputAction(InputAction.CallbackContext d)
    {
        DoBasicSlap();
    }

    public void DoBasicSlap()
    {
        AttackType basicSlap = data.basicSlap;
        AttackProfile attackProfile = AttackProfile.GetGlobalProfile(basicSlap);
        float attackStateLasttime = attackProfile.actionLastTime;


        bool stateAllowAttack = state.HasNo(CharacterState.Attack);
        if (stateAllowAttack)
        {
            #region Play Animation
            Animator animator = gameObject.GetComponentInChildren<Animator>();
            RuntimeAnimatorController contrl = animator.runtimeAnimatorController;
            bool exist = contrl.animationClips.First((clip) => clip.name == "Attack") != null;
            if (exist)
            {
                Player.Animation.Play(Player.AnimationState.Slap);

            }
            #endregion
            // * Region Play Animation End---------------------------------- 




            #region State Update

            state.Add(CharacterState.Attack);
            new Timer.TimerWait(attackStateLasttime, () => { state.Remove(CharacterState.Attack); });
            #endregion
            // * Region State Update End---------------------------------- 


            Vector2 position = default;
            float angle = 0;
            GetPositionAndAngle(out position, out angle);
            bool flipX = false;
            flipX = GetFlipX(flipX);

            Attack.CreateAttack(basicSlap, position, angle, flipX);


        }

        void GetPositionAndAngle(out Vector2 position, out float angle)
        {
            GameObject obj = null;
            AttackLocatorLo attackLocatorLo = gameObject.GetComponentInChildren<AttackLocatorLo>();
            if (attackLocatorLo != null)
            {
                obj = attackLocatorLo.gameObject;
            }
            if (obj == null) { obj = gameObject; }
            position = obj.GetPosition2d();
            angle = obj.transform.rotation.z;
        }

        bool GetFlipX(bool flipX)
        {
            if (state != null)
            {
                flipX = state.HasAll(CharacterState.FaceLeft);
            }

            return flipX;
        }
    }




}

