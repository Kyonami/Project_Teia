using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour {

    [Header("Money")]
    [SerializeField] Money money;
    [SerializeField] Text moneyText;

    [Header("Energy")]
    [SerializeField] Energy energy;
    [SerializeField] GameObject[] energyObjs;

	// Use this for initialization
	void Start () {
        if (GameManager.Instance == null)
            return;
        money = GameManager.Instance.Money;
        money.ChangeMoneyEvent += SetMoneyText;
        SetMoneyText(this, EventArgs.Empty);

        energy = GameManager.Instance.Energy;
        energy.ChangeEnergyEvent += SetEnergyObj;
        SetEnergyObj(this, EventArgs.Empty);
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnDestroy()
    {
        money.ChangeMoneyEvent -= SetMoneyText;
        energy.ChangeEnergyEvent -= SetEnergyObj;
    }

    void SetMoneyText(object sender, EventArgs handler)
    {
        moneyText.text = string.Format("{0:N0}", money.Coin);
    }

    void SetEnergyObj(object sender, EventArgs handler)
    {
        for(int i = 0; i < energy.EnergyCount; i++)
        {
            energyObjs[i].SetActive(true);
        }

        for(int i = energy.EnergyCount - 1; i < energy.MaxEnergyCount - 1; i++)
        {
            energyObjs[i + 1].SetActive(false);
        }
    }

    public void ChangeMoney(int money)
    {
        this.money.AddCoin(money);
    }

    public void ChangeEnergy(int energy)
    {
        this.energy.AddEnergy((byte)energy);
    }
}
