//using System.Collections;
//using UnityEngine.UI;
//using UnityEngine;

//public class Life : MonoBehaviour, ILife
//{
//    public bool isDead;
//    public bool isAuto;
//    public bool isPlayer;

//    [Header("Stat")]
//    private int healthPoint;
//    private int attackPoint;
//    public int applyDamage;
//    public int reinforcePoint;
//    public int level;
//    public int expInNeed;
//    [SerializeField]
//    private int currentHealthPoint;
//    private int currentExp;

//    [SerializeField]
//    private float hp_CoefficientToMultiple;
//    [SerializeField]
//    private int ad_CoefficientToDivide = 1;

//    [Header("Texts")]
//    public string attackText;
//    public string defenseText;
//    public string skillText;
//    public string waitText;

//    [Header("Private UI")]
//    [SerializeField]
//    private TextMesh myDamageText;
//    [SerializeField]
//    private Slider myHPBar;

//    public string lifeName;

//    public Choices MyChoice;
    
//    [Header("Components in need")]
//    [SerializeField]
//    private UIAnimation theUIAnimation;

//    public IEnumerator RandomChoice()
//    {
//        yield return new WaitForSeconds(1.5f);

//        int i = (isPlayer) ? Random.Range(0, 3) : Random.Range(0, 4);

//        switch (i)
//        {
//            case 0:
//                Attack();
//                break;
//            case 1:
//                UseSkill();
//                break;
//            case 2:
//                Defend();
//                break;
//            default:
//                Wait();
//                break;
//        }
//    }
//    public void Attack()
//    {
//        MyChoice = Choices.ATTACK;
//        applyDamage = attackPoint + (int)(Random.Range(attackPoint - (attackPoint / 6.0f), attackPoint + (attackPoint / 6.0f)));
//    }
//    public void Defend()
//    {
//        MyChoice = Choices.DEFENSE;
//        applyDamage = 0;
//    }
//    public void UseSkill()
//    {
//        MyChoice = Choices.SKILL;
//        applyDamage = (int)(attackPoint * Random.Range(2.0f, 3.0f));
//    }
//    public void Wait()
//    {
//        MyChoice = Choices.WAIT;
//        applyDamage = 0;
//    }
//    public void DecreaseHP(int value)
//    {
//        currentHealthPoint -= value;

//        if (currentHealthPoint < 0) currentHealthPoint = 0;
//        else if (currentHealthPoint > healthPoint) currentHealthPoint = healthPoint;

//        StartCoroutine(ShowDamageText(value));
//    }
//    public void InitializeChoice()
//    {
//        MyChoice = Choices.NONE;
//    }
//    public void DoLevelUp()
//    {
//        level++;
//        Mathf.Clamp(level, 0, 50);
//        CalculateLiveStat();
//        currentHealthPoint = healthPoint;
//    }
//    public void CalculateLiveStat()
//    {
//        expInNeed = level * 9;
//        attackPoint = expInNeed / ad_CoefficientToDivide;
//        healthPoint = (int)(expInNeed * hp_CoefficientToMultiple);
//        currentHealthPoint = healthPoint;
//    }
//    public void UpdateHpGuage()
//    {
//        myHPBar.value = (float)((float)currentHealthPoint / (float)healthPoint);
//    }

//    public string GetLifeName()
//    {
//        return lifeName;
//    }
//    public string GetMySentence()
//    {
//        string targetString = "";

//        switch (MyChoice)
//        {
//            case Choices.ATTACK:
//                targetString = attackText;
//                break;
//            case Choices.DEFENSE:
//                targetString = defenseText;
//                break;
//            case Choices.SKILL:
//                targetString = skillText;
//                break;
//            case Choices.WAIT:
//                targetString = waitText;
//                break;
//        }

//        return targetString;
//    }
//    public bool GetIsDead()
//    {
//        if (currentHealthPoint <= 0)
//            return true;
//        return false;
//    }

//    public IEnumerator ShowDamageText(int damage)
//    {
//        myDamageText.text = damage.ToString();
//        myDamageText.gameObject.SetActive(true);

//        yield return new WaitForSeconds(1.0f);
//        myDamageText.gameObject.transform.localPosition = new Vector3(0f, 2.5f, 0f);
//        myDamageText.gameObject.SetActive(false);
//    }
//}

