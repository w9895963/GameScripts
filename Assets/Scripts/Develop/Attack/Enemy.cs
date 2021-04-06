using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.FunctionManager;
using Global;

public class Enemy : MonoBehaviour, IFunctionManager
{

    public HitableFunction.Data hitableFunctionData = new HitableFunction.Data();
    private FunctionManager functionManager;
    public FunctionManager Manager => functionManager;

    private void Awake()
    {
        functionManager = new FunctionManager(gameObject);

        // HitableFunction hitableFunction = new HitableFunction();
        // functionManager.AddFunction<HitableFunction>(hitableFunction);
        // functionManager.AddData<HitableFunction.Data>(hitableFunctionData);
        // hitableFunction.Initial(functionManager);


        functionManager.FastBuildUp<HitableFunction, HitableFunction.Data>(hitableFunctionData);




    }


}