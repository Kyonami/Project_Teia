using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Money
{
    [SerializeField] long coin = 1;
    public long Coin { get { return coin; } }

    private EventHandler _changeMoneyEvent;
    public event EventHandler ChangeMoneyEvent
    {
        add
        {
            _changeMoneyEvent += value;
        }
        remove
        {
            _changeMoneyEvent -= value;
        }
    }

    public void Init(long coin)
    {
        this.coin = coin;
    }

    public void AddCoin(long coin)
    {
        this.coin += coin;
        EventCall();
    }

    public bool SubtractCoin(long coin)
    {
        this.coin -= coin;
        if (this.coin <= 0)
        {
            this.coin += coin;
            EventCall();
            return false;
        }
        EventCall();
        return true;
    }

    private void EventCall()
    {
        if (this._changeMoneyEvent != null)
        {
            // 이벤트핸들러들을 호출
            _changeMoneyEvent(this, EventArgs.Empty);
        }
    }
}
