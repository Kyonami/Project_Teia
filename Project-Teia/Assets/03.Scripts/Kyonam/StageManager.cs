using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour, IObserver
{
    // values
    #region
    [SerializeField]
    private string stageInfo;
    [SerializeField]
    private bool TestMode;
    #endregion

    //Properties
    #region
    public string StageInfo { get { return stageInfo; } }
    #endregion

    [SerializeField]
    GameObject battleObject, dialogObject;

    private void Start()
    {
        if (TestMode)
            PlayBattle();
        else
            PlayDialogue();
    }

    private void PlayBattle()
    {
        battleObject.SetActive(true);
        dialogObject.SetActive(false);
    }

    private void PlayDialogue()
    {
        battleObject.SetActive(false);
        dialogObject.SetActive(true);
    }

    void IObserver.OnNotify(EVENT _event)
    {
        switch (_event)
        {
            case EVENT.GOBATTLE:
                PlayBattle();
                break;
            case EVENT.ENDBATTLE:
                PlayDialogue();
                break;
        }
    }
}
