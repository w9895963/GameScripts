using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MovementGenerator
{
    public Vector2 velocity = Vector2.zero;
    public float distance = 0;
    private Vector2 position = Vector2.zero;

    public float deltaTime = Time.fixedDeltaTime;
    public float mass = 1;

    public List<Movement> movementList = new List<Movement>();


    public int lastStep => (movementList.Count - 1).ClampMin(0);
    public int currentStep => movementList.Count;





    private void RecordDate()
    {
        Movement item = new Movement();
        item.movement = this;
        item.index = movementList.Count;
        item.velocity = velocity;
        item.position = position;
        item.distance = distance;
        movementList.Add(item);
    }

    public void UpdateZeroStep()
    {
        if (movementList.Count == 0)
            RecordDate();
    }
    public void UpdateStep()
    {
        position += velocity * deltaTime;
        distance += velocity.magnitude * deltaTime;
        RecordDate();
    }



    public void SimulateUntil(Action beforeUpdate, Func<bool> stopCondition, int? maxSimTimes = null)
    {
        UpdateZeroStep();
        int count = 0;
        bool reachCountLimit() => maxSimTimes == null ? false : count >= (int)maxSimTimes;

        while (!stopCondition() & !reachCountLimit())
        {
            beforeUpdate.Invoke();
            UpdateStep();
            count++;

        }

    }

    public void SimulateSteps(Action beforeUpdate, int step)
    {
        UpdateZeroStep();

        while (step > 0)
        {
            beforeUpdate.Invoke();
            UpdateStep();
            step--;
        }

    }

    public int TimeToStep(float time)
    {
        return (time / deltaTime).CeilToInt();
    }





    public Movement GenerateMovement(Vector2 endVelocity, float time)
    {
        MovementGenerator mov = this;
        var startVelocity = mov.velocity;
        int step = mov.TimeToStep(time);

        int startStep = mov.lastStep;
        Action onUpdate = () =>
        {
            int curr = mov.currentStep - startStep;
            mov.velocity = curr.Map(0, step, startVelocity, endVelocity);
        };

        mov.SimulateSteps(onUpdate, step);
        return mov.movementList[0];
    }



}


public class Movement
{
    public MovementGenerator movement;
    public int index;
    public Vector2 velocity;
    public Vector2 position;
    public float distance;
    public float deltaTime => movement.deltaTime;
    public List<Movement> movementDates => movement.movementList;
    public int count => movementDates.Count;
    public bool isLast => count == index + 1;

    public Movement nextDate => count > index + 1 ? movementDates[index + 1] : null;

    public Vector2 force => !isLast ? (nextDate.velocity - velocity) / deltaTime * movement.mass : Vector2.zero;
}