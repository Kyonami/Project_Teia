using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

public enum BattleResult{
    WIN,
    LOSE,
    DRAW    
}

public class BattleStateMachine : Subject
{
    // values
    #region
    private BattleState currentState;
    private int UpDialogCount;
    private int LeftTurnCount;
    public int TurnCount { get; private set; }
    public bool IsGameEnd { get; private set; }
    public bool IsTurnEnd { get; set; }
    #endregion

    [Header("Fighters")]
    #region
    [SerializeField]
    private Fighter[] fighters;
    #endregion

    [Header("Components in need")]
    #region
    [SerializeField]
    private CameraAction theCameraAction;
    [SerializeField]
    private UIAnimation theUIAnimation;
    [SerializeField]
    private StageManager theStageManager;
    #endregion

    [Header("UI")]
    #region
    [SerializeField]
    GameObject choice_panel;
    [SerializeField]
    GameObject result_panel;
    [SerializeField]
    GameObject end_panel;
    [SerializeField]
    Image skillButton_Image;
    [SerializeField]
    Text skillCoolDown_Text;
    [SerializeField]
    Text info_text;
    #endregion

    [Header("Resulting UIs")]
    #region
    [SerializeField]
    Image[] resultElementPanels;
    [SerializeField]
    Text[] resultElementTexts;
    #endregion

    public void Start()
    {
        currentState = new WaitingState();
        theCameraAction.PlayIdleAction();
        AttachObserver(theStageManager);
        info_text.text = "무엇을 하시겠습니까?";
        for (int i = 0; i < C.NUMBER_OF_LIFE; i++)
        {
            fighters[i].myInfo.CalculateLiveStat();
            fighters[i].ResetHP();
        }
    }
    public void Update()
    {
        currentState.Handle(this);
    }
    public void ChangeState(BattleState _state)
    {
        currentState.Exit(this);

        currentState = _state;

        currentState.Enter(this);
        currentState.Handle(this);
    }

    public Fighter GetFighterInfo(int _value)
    {
        if (fighters[_value] == null)
            return null;

        return fighters[_value];
    }
    public CameraAction GetCameraAction()
    {
        return theCameraAction;
    }

    public bool IsDead()
    {
        for(int i = 0; i < C.NUMBER_OF_LIFE; i++)
        {
            if (fighters[i].GetIsDead())
                return true;
        }
        return false;
    }

    public void ShowChoicePanel()
    {
        choice_panel.SetActive(true);
        result_panel.SetActive(false);
    }
    public void ShowResultPanel()
    {
        choice_panel.SetActive(false);
        result_panel.SetActive(true);
    }
    public void ShowEndPanel()
    {
        choice_panel.SetActive(false);
        result_panel.SetActive(false);
        end_panel.SetActive(true);
    }

    public void ShowResultElements(string _text)
    {
        this.resultElementTexts[UpDialogCount].text = _text;

        StartCoroutine(theUIAnimation.FadeIn(resultElementPanels[UpDialogCount], 1.0f));

        for (int i = 0; i <= UpDialogCount; i++)
        {
            StartCoroutine(theUIAnimation.MoveSmoothly(resultElementPanels[i].gameObject, new Vector2(0, (UpDialogCount - i) * 100 - 100)));
            if (UpDialogCount >= C.DIALOG_MAX)
            {
                StartCoroutine(theUIAnimation.FadeOut(resultElementPanels[i], 1.0f));
            }
        }
        UpDialogCount++;
        if (UpDialogCount >= C.DIALOG_MAX)
            UpDialogCount = 0;
    }
    public void InitializeResultElements()
    {
        for(int i = 0; i < C.DIALOG_MAX; i++)
        {
            resultElementPanels[i].transform.localPosition = new Vector2(0, -300);
            resultElementTexts[i].text = "";
        }
    }

    public void ChooseRandomly()
    {
        StartCoroutine(fighters[TurnCount].RandomChoice());
    }
    public void ChooseAttack()
    {
        fighters[TurnCount].Attack();
    }
    public void ChooseDefense()
    {
        fighters[TurnCount].Defend();
    }
    public void ChooseSkill()
    { 
        if (fighters[TurnCount].GetIsAbleToUseSkill())
            fighters[TurnCount].UseSkill();
        //StartCoroutine(AlertString("스킬이 쿨타임 중입니다..." + fighters[TurnCount].currentSkillCoolDown.ToString() + "턴 남음..."));
    }
    public void UpdateSkillImage()
    {
        if (fighters[TurnCount].GetIsAbleToUseSkill())
        {
            skillCoolDown_Text.gameObject.SetActive(false);
            skillButton_Image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            if (fighters[TurnCount].myInfo.isPlayer)
            {
                float fTemp = 0.5f + (1.0f - ((float)fighters[TurnCount].CurrentSkillCoolDown / fighters[TurnCount].myInfo.skillCoolDown)) / 2f;
                skillCoolDown_Text.gameObject.SetActive(true);
                skillButton_Image.color = new Color(fTemp, fTemp, fTemp, 1);
                skillCoolDown_Text.text = fighters[TurnCount].CurrentSkillCoolDown.ToString();
            }
        }
    }

    public string CompareLifeChoice()
    {
        StringBuilder stringBuilder = new StringBuilder();

        switch (fighters[C.PLAYER].MyChoice)
        {
            case Choices.ATTACK:
                switch (fighters[C.ENEMY].MyChoice)
                {
                    case Choices.ATTACK:
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("(와)과 ");
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("(이)가 서로 공격을 주고 받았다!");
                        break;
                    case Choices.DEFENSE:
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("(이)가 ");
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("의 공격을 막아냈다!");
                        break;
                    case Choices.SKILL:
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("(은)는 ");
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("에게 치명타를 입혔다!");
                        break;
                    case Choices.WAIT:
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("(은)는 ");
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("(을)를 공격했다!");
                        break;
                    default:
                        break;
                }
                break;
            case Choices.DEFENSE:
                switch (fighters[C.ENEMY].MyChoice)
                {
                    case Choices.ATTACK:
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("(이)가 ");
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("의 공격을 막아냈다!");
                        break;
                    case Choices.DEFENSE:
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("(와)과 ");
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("(은)는 서로 견제했다.");
                        break;
                    case Choices.SKILL:
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("(은)는 ");
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("의 스킬을 방어했다!");
                        break;
                    case Choices.WAIT:
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("(은)는 자신을 보호한다!");
                        break;
                    default:
                        break;
                }
                break;
            case Choices.SKILL:
                switch (fighters[C.ENEMY].MyChoice)
                {
                    case Choices.ATTACK:
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("(은)는 ");
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("에게 치명타를 입혔다!");
                        break;
                    case Choices.DEFENSE:
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("(은)는 ");
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("의 스킬을 방어했다!");
                        break;
                    case Choices.SKILL:
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("(와)과 ");
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("의 스킬이 충돌하여 사라졌다!");
                        break;
                    case Choices.WAIT:
                        stringBuilder.Append(fighters[C.PLAYER].myInfo.lifeName);
                        stringBuilder.Append("(은)는 ");
                        stringBuilder.Append(fighters[C.ENEMY].myInfo.lifeName);
                        stringBuilder.Append("에게 스킬을 사용했다!");
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        return stringBuilder.ToString();
    }
    public bool ApplyDamage()
    {
        switch (fighters[C.PLAYER].MyChoice)
        {
            case Choices.ATTACK:

                if (fighters[C.ENEMY].MyChoice == Choices.ATTACK)
                {
                    StartCoroutine(Damage(C.ENEMY, fighters[C.PLAYER].ApplyDamage, 0));
                    StartCoroutine(Damage(C.PLAYER, fighters[C.ENEMY].ApplyDamage, 1.5f));
                }
                else if (fighters[C.ENEMY].MyChoice == Choices.DEFENSE)
                {
                    StartCoroutine(Damage(C.ENEMY, fighters[C.PLAYER].ApplyDamage / 2, 0));
                }
                else if (fighters[C.ENEMY].MyChoice == Choices.SKILL)
                {
                    StartCoroutine(Damage(C.PLAYER, fighters[C.ENEMY].ApplyDamage * 2, 0));
                }
                else if (fighters[C.ENEMY].MyChoice == Choices.WAIT)
                {
                    StartCoroutine(Damage(C.ENEMY, fighters[C.PLAYER].ApplyDamage, 0));
                }
                else return false;

                return true;

            case Choices.DEFENSE:

                if (fighters[C.ENEMY].MyChoice == Choices.ATTACK)
                {
                    StartCoroutine(Damage(C.PLAYER, fighters[C.ENEMY].ApplyDamage / 2, 0));
                }
                else if (fighters[C.ENEMY].MyChoice == Choices.SKILL)
                {
                    StartCoroutine(Damage(C.PLAYER, fighters[C.ENEMY].ApplyDamage / 2, 0));
                }
                else return false;

                return true;
            case Choices.SKILL: 
                if (fighters[C.ENEMY].MyChoice == Choices.ATTACK)
                {
                    StartCoroutine(Damage(C.ENEMY, fighters[C.PLAYER].ApplyDamage * 2, 0));
                }
                else if (fighters[C.ENEMY].MyChoice == Choices.DEFENSE)
                {
                    StartCoroutine(Damage(C.ENEMY, fighters[C.PLAYER].ApplyDamage / 2, 0));
                }
                else if (fighters[C.ENEMY].MyChoice == Choices.WAIT)
                {
                    StartCoroutine(Damage(C.ENEMY, fighters[C.PLAYER].ApplyDamage, 0));
                }
                else return false;

                return true;

            case Choices.WAIT: 
                if (fighters[C.ENEMY].MyChoice == Choices.ATTACK)
                {
                    StartCoroutine(Damage(C.PLAYER, fighters[C.ENEMY].ApplyDamage, 0));
                }
                else if (fighters[C.ENEMY].MyChoice == Choices.SKILL)
                {
                    StartCoroutine(Damage(C.PLAYER, fighters[C.ENEMY].ApplyDamage, 0));
                }
                else return false;

                return true;
        }
        return false;
    }
    public IEnumerator Damage(int _subject, int _damage, float _restTime)
    {
        int attacker = 1 - _subject;
        yield return new WaitForSeconds(_restTime);
        fighters[attacker].ActionAnimStart();

        yield return new WaitForSeconds(fighters[1- _subject].myInfo.attackStartDelay);
        fighters[_subject].DecreaseHP(_damage);
        theCameraAction.PlayDamagedAction();
        fighters[attacker].ActionAnimStop();

        yield return new WaitForSeconds(fighters[1- _subject].myInfo.attackDelay + fighters[1- _subject].myInfo.attackEndDelay);
    }
    public void IncreaseTurnCount()
    {
        TurnCount++;

        if (TurnCount >= C.NUMBER_OF_LIFE)
            TurnCount = C.NUMBER_OF_LIFE - 1;
    }
    public void InitializeTurnCount()
    {
        TurnCount = 0;
    }
    public void InitializeLifeAnim()
    {
        for(int i = 0; i < fighters.Length; i++)
        {
            fighters[i].ResetAnim();
        }
    }

    public IEnumerator AlertString(string _text)
    {
        info_text.text = _text;
        yield return new WaitForSeconds(1.0f);
        info_text.text = "무엇을 하시겠습니까?";
    }
}
