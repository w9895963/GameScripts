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

    public StateFunction.Data stateData = new StateFunction.Data();
    public ReferenceFunction.Data referenceFunction = new ReferenceFunction.Data();
    public GravityFunction.Data gravityData = new GravityFunction.Data();
    public MoveFunction.Data walkingData = new MoveFunction.Data();
    public GroundTestFunction.Data groundTest = new GroundTestFunction.Data();
    public JumpFuntion.Data jumpFuntionData = new JumpFuntion.Data();
    public AttackFunc.Data AttackData = new AttackFunc.Data();



    public FunctionManager functionManager;

    public FunctionManager Manager => functionManager;

    private void Awake()
    {
        functionManager = new FunctionManager(gameObject);

        var fm = functionManager;
        fm.CreateFunction<StateFunction>(stateData);
        fm.CreateFunction<ReferenceFunction>(referenceFunction);

        fm.CreateFunction<GroundTestFunction>(groundTest);
        fm.CreateFunction<GravityFunction>(gravityData);
        fm.CreateFunction<MoveFunction>(walkingData);
        fm.CreateFunction<AttackFunc>(AttackData);
        fm.CreateFunction<JumpFuntion>(jumpFuntionData);

        fm.CallLateCreateAction();
    }
















}

