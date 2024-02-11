﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System;
using System.Collections.Generic;

public class Interacable : MonoBehaviour
{
    [Header("Selection")]
    [Space(10)]
    public List<Conditions> selectConditions;
    public UnityEvent onSelected;
    [Header("Deselection")]
    [Space(10)]
    public List<Conditions> deselectConditions;
    public UnityEvent onDeselected;

    public Rigidbody rb;

    private bool selected;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact() 
    {
        if (selected)
            return;

        int temp = 0;

        for (int i = 0; i < selectConditions.Count; i++)
        {
            temp += selectConditions[i].CheckCondition() ? 1 : 0;
        }

        if (temp == selectConditions.Count)
            Select();
    }

    public void DeInteract() 
    {
        for (int i = 0; i < selectConditions.Count; i++)
        {
            selectConditions[i].Reset();
        }
    }

    public void Select()
    {
        for(int i = 0;i < selectConditions.Count;i++)
        {
            selectConditions[i].Reset();
        }

        selected = true;
        onSelected?.Invoke();
    }

    public void DeSelect()
    {
        selected = false;
        onDeselected?.Invoke();
    }

}

[Serializable]
public class Conditions
{
    public ConditionActor conditionType;

    public ConditionAction conditionAction;

    public float conditionValue;

    // Variable instance
    private float timer;
    private float distance;
    private int count;

    public Conditions(ConditionActor type , ConditionAction action, float value)
    {
        conditionType = type;
        conditionAction = action;
        conditionValue = value;
    }

    public void Reset()
    {
        timer = 0;
        count = 0;
        distance = 0;
    }

    public bool CheckCondition()
    {
        switch (conditionType)
        {
            case ConditionActor.LookAt:
                return LookAtCheck();
            case ConditionActor.Blink:
                return true;
            default:
                return false;
        }
    }

    private bool LookAtCheck()
    {
        switch (conditionAction)
        {
            case ConditionAction.Time:
                return LookAtTimer(conditionValue);
            case ConditionAction.Amount:
                return true;
            case ConditionAction.Distance:
                return true;
            default:
                return false;
        }
    }

    public bool LookAtTimer(float time)
    {
        timer += Time.deltaTime;

        return timer >= time;
    }
}

public enum ConditionActor
{
    LookAt, Cursor, Blink, Grab, Pinch, Touch
}

public enum ConditionAction
{
    Time, Amount, Distance
}

public enum GrabType
{
    EYE, HAND
}