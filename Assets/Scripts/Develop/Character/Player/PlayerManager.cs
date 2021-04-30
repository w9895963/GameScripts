using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Animate;
using Global.ObjectDynimicFunction;
using static Global.Function;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Global.Physic;

public class PlayerManager : MonoBehaviour, IFunctionManager

{

    public StateFunc.Data stateData = new StateFunc.Data();
    public ReferenceFunction.Data referenceFunction = new ReferenceFunction.Data();
    public GravityFunction.Data gravityData = new GravityFunction.Data();
    public WalkFunc.Data walkingData = new WalkFunc.Data();
    public GroundTestFunction.Data groundTest = new GroundTestFunction.Data();
    public JumpFuntion.Data jumpFuntionData = new JumpFuntion.Data();
    public AttackFunc.Data AttackData = new AttackFunc.Data();
    public PlayerAttackFunc.Data playerAttack = new PlayerAttackFunc.Data();
    public HitableFunction.Data hittable = new HitableFunction.Data();



    public FunctionManager functionManager;

    public FunctionManager Manager => functionManager;

    private void Awake()
    {
        functionManager = new FunctionManager(gameObject);

        var fm = functionManager;
        fm.CreateFunction<StateFunc>(stateData);
        fm.CreateFunction<ReferenceFunction>(referenceFunction);

        fm.CreateFunction<GroundTestFunction>(groundTest);
        fm.CreateFunction<GravityFunction>(gravityData);
        fm.CreateFunction<WalkFunc>(walkingData);
        fm.CreateFunction<AttackFunc>(AttackData);
        fm.CreateFunction<PlayerAttackFunc>(playerAttack);
        fm.CreateFunction<HitableFunction>(hittable);
        fm.CreateFunction<JumpFuntion>(jumpFuntionData);

        fm.CallLateCreateAction();
    }





    private void OnParticleTrigger()
    {
        Debug.Log(123);
    }










}

