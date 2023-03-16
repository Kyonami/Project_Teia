using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Life : MonoBehaviour
{
    [Header("Stat")]
    public string lifeName;
    public bool isAuto;
    public bool isPlayer;

    public int reinforcePoint;
    public int level;
    public int expInNeed;
    public int skillCoolDown;

    public int healthPoint;
    public int attackPoint;

    public float attackStartDelay;
    public float attackDelay;
    public float attackEndDelay;
    public float hp_CoefficientToMultiple;
    public int ad_CoefficientToDivide = 1;

    [Header("Texts")]
    public string attackText;
    public string defenseText;
    public string skillText;
    public string waitText;

    private void Awake()
    {
        CalculateLiveStat();
    }
    public void DoLevelUp()
    {
        level++;
        Mathf.Clamp(level, 0, 50);
        CalculateLiveStat();
    }
    public void CalculateLiveStat()
    {
        expInNeed = level * 9;
        attackPoint = expInNeed / ad_CoefficientToDivide;
        healthPoint = (int)(expInNeed * hp_CoefficientToMultiple);
    }
}

