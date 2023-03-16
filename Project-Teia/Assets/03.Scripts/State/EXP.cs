using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EXP
{
    [SerializeField] byte level = 1;
    [SerializeField] float currentExp;
    [SerializeField] float expIncreasedLevelPerLevel;

    public byte Level { get { return level; } }
    public float Exp { get { return currentExp; } }
    public float ExpIncreasedLevelPerLevel { get { return expIncreasedLevelPerLevel; } }

    private EventHandler _changeExpEvent;
    public event EventHandler ChangeExpEvent
    {
        add
        {
            _changeExpEvent += value;
        }
        remove
        {
            _changeExpEvent -= value;
        }
    }

    public void Init()
    {
        currentExp = 0;
        expIncreasedLevelPerLevel = Mathf.Pow(level, 2f) * 9f;
    }

    public void AddEXP(float exp)
    {
        currentExp += exp;
        if (currentExp >= expIncreasedLevelPerLevel)
        {
            level++;
            currentExp -= expIncreasedLevelPerLevel;
            expIncreasedLevelPerLevel = Mathf.Pow(level, 2f) * 9f;
        }
        EventCall();
    }

    public bool SubtractEXP(float exp)
    {
        currentExp -= exp;
        
        if (currentExp <= 0)
        {
            currentExp = 0;
            EventCall();
            return false;
        }
        EventCall();
        return true;
    }

    private void EventCall()
    {
        if (this._changeExpEvent != null)
        {
            // 이벤트핸들러들을 호출
            _changeExpEvent(this, EventArgs.Empty);
        }
    }

    public float GetNowHpPoint()
    {
        return currentExp / expIncreasedLevelPerLevel;
    }
}