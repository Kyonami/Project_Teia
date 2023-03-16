using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IFighter {

    public Life myInfo;

    [Header("Private UI")]
    [SerializeField]
    private TextMesh myDamageText;
    [SerializeField]
    private HPBar myHPBar;

    [Header("Components in need")]
    [SerializeField]
    private UIAnimation theUIAnimation;
    [SerializeField]
    private Animator myAnimator;

    [SerializeField]
    private int currentHealthPoint;
    private int currentExp;
    private int currentSkillCoolDown;
    private int applyDamage;
    private bool isChoosing;
    private Choices myChoice;

    //Properties
    public bool IsChoosing { get => isChoosing; }
    public Choices MyChoice { get => myChoice; }
    public int CurrentSkillCoolDown { get => currentSkillCoolDown; }
    public int ApplyDamage { get => applyDamage; }

    public IEnumerator RandomChoice()
    {
        if (!isChoosing)
        {
            isChoosing = true;
            yield return new WaitForSeconds(1.5f);

            int i = (myInfo.isPlayer) ? Random.Range(0, 3) : Random.Range(0, 4);

            switch (i)
            {
                case 0:
                    Attack();
                    break;
                case 1:
                    UseSkill();
                    break;
                case 2:
                    Defend();
                    break;
                default:
                    Wait();
                    break;
            }
            isChoosing = false;
        }
    }
    public void Attack()
    {
        myChoice = Choices.ATTACK;
        PlayPreparingAnim(C.ATTACK);
        applyDamage = myInfo.attackPoint + (int)(Random.Range(myInfo.attackPoint - (myInfo.attackPoint / 6.0f), myInfo.attackPoint + (myInfo.attackPoint / 6.0f)));
        currentSkillCoolDown--;
    }
    public void Defend()
    {
        myChoice = Choices.DEFENSE;
        PlayPreparingAnim(C.DEFENSE);
        currentSkillCoolDown--;
        applyDamage = 0;
    }
    public void UseSkill()
    {
        if (currentSkillCoolDown > 0)
        {
            Attack();
            return;
        }
        myChoice = Choices.SKILL;
        PlayPreparingAnim(C.SKILL);
        applyDamage = (int)(myInfo.attackPoint * Random.Range(2.0f, 3.0f));
        currentSkillCoolDown = myInfo.skillCoolDown;
    }
    public void Wait()
    {
        myChoice = Choices.WAIT;
        PlayPreparingAnim(C.DEFENSE);
        applyDamage = 0;
        currentSkillCoolDown--;
    }
    public void ResetChoice()
    {
        myChoice = Choices.NONE;
    }

    public void DecreaseHP(int _value)
    {
        currentHealthPoint -= _value;

        if (currentHealthPoint < 0) currentHealthPoint = 0;
        else if (currentHealthPoint > myInfo.healthPoint) currentHealthPoint = myInfo.healthPoint;
        myHPBar.UpdateGauge();

        StartCoroutine(ShowDamageText(_value));
    }
    public void ResetHP()
    {
        currentHealthPoint = myInfo.healthPoint;
    }

    public IEnumerator ShowDamageText(int _damage)
    {
        myDamageText.text = _damage.ToString();
        myDamageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        myDamageText.gameObject.transform.localPosition = new Vector3(0f, 2.5f, 0f);
        myDamageText.gameObject.SetActive(false);
    }
    public void PlayPreparingAnim(int _value)
    {
        if (myAnimator == null)
            return;

        switch (_value)
        {
            case C.ATTACK:
                myAnimator.SetInteger("Choice", 1);
                break;
            case C.DEFENSE:
                myAnimator.SetInteger("Choice", 2);
                break;
            case C.SKILL:
                myAnimator.SetInteger("Choice", 1);
                break;
            default:
                break;
        }
    }
    public void ActionAnimStart()
    {
        if (myAnimator == null)
            return;

        myAnimator.SetBool("ActionStartFlag", true);
    }
    public void ActionAnimStop()
    {
        if (myAnimator == null)
            return;

        myAnimator.SetBool("ActionStartFlag", false);
    }
    public void ResetAnim()
    {
        if (myAnimator == null)
            return;

        myAnimator.SetInteger("Choice", 0);
        myAnimator.SetBool("ActionStartFlag", false);
    }
    public float GetCurrentHPRatio()
    {
        return (float)currentHealthPoint / myInfo.healthPoint;
    }
    public bool GetIsAbleToUseSkill()
    {
        return currentSkillCoolDown <= 0;
    }
    public string GetMySentence()
    {
        string targetString = "";

        switch (MyChoice)
        {
            case Choices.ATTACK:
                targetString = myInfo.attackText;
                break;
            case Choices.DEFENSE:
                targetString = myInfo.defenseText;
                break;
            case Choices.SKILL:
                targetString = myInfo.skillText;
                break;
            case Choices.WAIT:
                targetString = myInfo.waitText;
                break;
        }

        return targetString;
    }
    public bool GetIsDead()
    {
        if (currentHealthPoint <= 0)
            return true;
        return false;
    }
}