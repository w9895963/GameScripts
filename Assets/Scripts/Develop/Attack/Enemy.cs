using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.ObjectDynimicFunction;
using Global;

public class Enemy : MonoBehaviour, IFunctionManager
{

    public HitableFunction.Data hitableFunctionData = new HitableFunction.Data();
    public MoveFunction.Data walkingFuncionData = new MoveFunction.Data();
    public StateFunction.Data StateFunctionData = new StateFunction.Data();
    private FunctionManager functionManager;

    public FunctionManager Manager => functionManager;

    private void Awake()
    {
        functionManager = new FunctionManager(gameObject);



        functionManager.CreateFunction<StateFunction>(StateFunctionData);
        functionManager.CreateFunction<HitableFunction>(hitableFunctionData);
        functionManager.CreateFunction<MoveFunction>(walkingFuncionData);




    }


}