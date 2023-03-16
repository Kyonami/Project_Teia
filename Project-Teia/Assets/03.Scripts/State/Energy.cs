using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Energy
{
    [SerializeField] byte energyCount = 1;
    [SerializeField] byte maxEnergyCount = 5;

    public byte EnergyCount { get { return energyCount; } }
    public byte MaxEnergyCount { get { return maxEnergyCount; } }

    private EventHandler _changeEnergyEvent;
    public event EventHandler ChangeEnergyEvent
    {
        add
        {
            _changeEnergyEvent += value;
        }
        remove
        {
            _changeEnergyEvent -= value;
        }
    }

    public void Init(byte count)
    {
        energyCount = count;
    }

    public void AddEnergy(byte count = 1)
    {
        energyCount += count;
        EventCall();
    }

    public bool SubtractEnergy(byte count = 1)
    {
        energyCount -= count;
        if (energyCount <= 0)
        {
            energyCount = 0;
            EventCall();
            return false;
        }
        EventCall();
        return true;
    }

    private void EventCall()
    {
        if (this._changeEnergyEvent != null)
        {
            // 이벤트핸들러들을 호출
            _changeEnergyEvent(this, EventArgs.Empty);
        }
    }
}
