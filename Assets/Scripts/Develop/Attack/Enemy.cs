using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.ObjectDynimicFunction;
using Global;

public class Enemy : MonoBehaviour, IFunctionManager
{
    private FunctionManager functionManager;

    public HitableFunction.Data hitableFunctionData = new HitableFunction.Data();
    public WalkFunc.Data walkingFuncionData = new WalkFunc.Data();
    public StateFunc.Data StateFunctionData = new StateFunc.Data();
    public AttackFunc.Data attack = new AttackFunc.Data();
    public AutoAttackFunc.Data autoAttack = new AutoAttackFunc.Data();

    public FunctionManager Manager => functionManager;

    private void Awake()
    {
        functionManager = new FunctionManager(gameObject);



        functionManager.CreateFunction<StateFunc>(StateFunctionData);
        functionManager.CreateFunction<HitableFunction>(hitableFunctionData);
        functionManager.CreateFunction<WalkFunc>(walkingFuncionData);
        functionManager.CreateFunction<AttackFunc>(attack);
        functionManager.CreateFunction<AutoAttackFunc>(autoAttack);



        functionManager.CallLateCreateAction();




    }


}