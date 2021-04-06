using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Global.Animate;
using Global.FunctionManager;
using static Global.Function;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Global.Physic;

public class PlayerManager : MonoBehaviour, IFunctionManager
{
    public GravityFunction.Data gravityData = new GravityFunction.Data();
    public WalkingFuncion.Data walkingData = new WalkingFuncion.Data();
    public GroundTestFunction.Data contactData = new GroundTestFunction.Data();
    public JumpFuntion.Data jumpFuntionData = new JumpFuntion.Data();
    public AttackFunction.Data attackFunctionData = new AttackFunction.Data();



    public FunctionManager functionManager;

    public FunctionManager Manager => functionManager;

    private void Awake()
    {
        functionManager = new FunctionManager(gameObject);


        GroundTestFunction groundTestFunction = new GroundTestFunction();
        functionManager.AddFunction<GroundTestFunction>(groundTestFunction);
        functionManager.AddData<GroundTestFunction.Data>(contactData);
        groundTestFunction.Initial(functionManager);




        GravityFunction gravityFunction = new GravityFunction();
        functionManager.AddFunction<GravityFunction>(gravityFunction);
        functionManager.AddData<GravityFunction.Data>(gravityData);
        gravityFunction.Initial(functionManager);



        WalkingFuncion walkingFunction = new WalkingFuncion();
        functionManager.AddFunction<WalkingFuncion>(walkingFunction);
        functionManager.AddData<WalkingFuncion.Data>(walkingData);
        walkingFunction.Initial(functionManager);


        JumpFuntion jumpFuntion = new JumpFuntion();
        functionManager.AddFunction<JumpFuntion>(jumpFuntion);
        functionManager.AddData<JumpFuntion.Data>(jumpFuntionData);
        jumpFuntion.Initial(functionManager);


        functionManager.FastBuildUp<AttackFunction, AttackFunction.Data>(attackFunctionData);



    }














}

