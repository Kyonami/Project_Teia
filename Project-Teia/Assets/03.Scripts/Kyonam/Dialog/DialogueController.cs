using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : Subject {

    [Header("Values")]
    #region
    [SerializeField]
    private float textSpeed;
    [SerializeField]
    private float currentTextSpeed;
    [SerializeField]
    private float directionSpeed;
    [SerializeField]
    private string currentTalker;
    [SerializeField]
    private bool isPlaying;
    [SerializeField]
    private int dialogueCount;
    #endregion

    // objects
    #region
    private Coroutine nowPlayingTextCoroutine;
    private List<Dictionary<string, object>> storyBoard;
    #endregion

    [Header("Components in need")]
    #region
    [SerializeField]
    private CharacterDirection theCharacterDirection;
    [SerializeField]
    private StageManager theStageManager;
    #endregion

    [Header("UIs")]
    #region
    [SerializeField]
    private Text dialogText;
    [SerializeField]
    private GameObject background, dialoguePanel;
    #endregion

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => theStageManager.StageInfo != string.Empty);

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("StoryBoards/");
        stringBuilder.Append(theStageManager.StageInfo);

        storyBoard = CSVReader.Read(stringBuilder.ToString());
        AttachObserver(theStageManager);
        currentTextSpeed = textSpeed;
        dialogueCount = 0;
        NextDialogue();
    }
    private void OnEnable()
    {
        if (storyBoard == null || dialogueCount < 1)
            return;

        NextDialogue();
    }
    public void TouchDialogueWindow()
    {
        if (isPlaying)
        {
            FinishText();
            return;
        }
        currentTextSpeed = textSpeed;
        NextDialogue();
    }
    private void NextDialogue()
    {
        if ((int)storyBoard[dialogueCount]["Number"] == -1)
        {
            Debug.Log("dialogue ends");
            return;
        }
        if (storyBoard[dialogueCount]["Talker"].ToString().Equals("전투"))
        {
            Notify(EVENT.GOBATTLE);
            dialogueCount++;
            return;
        }
        if(storyBoard[dialogueCount]["Direction"].ToString().Length != 0)
        {
            theCharacterDirection.PlayDirection(
                storyBoard[dialogueCount]["Direction"].ToString(), 
                storyBoard[dialogueCount]["A"].ToString(), 
                storyBoard[dialogueCount]["B"].ToString(), 
                storyBoard[dialogueCount]["C"].ToString()
                );
        }

        StringBuilder _text = new StringBuilder();

        if (storyBoard[dialogueCount]["Talker"].ToString().Length != 0)
        {
            currentTalker = storyBoard[dialogueCount]["Talker"].ToString();

            //if (storyBoard[dialogueCount]["Face"].ToString().Length != 0)
            //{
            //    theCharacterDirection.ChangeFace(
            //        currentTalker, 
            //        storyBoard[dialogueCount]["Face"].ToString()
            //        );
            //}
            _text.Append(currentTalker);
            _text.Append(": ");
        }
        _text.Append(storyBoard[dialogueCount]["Speech"].ToString());

        StartCoroutine(ShowText(_text.ToString()));

        dialogueCount++;
    }
    private IEnumerator ShowText(string targetText)
    {
        isPlaying = true;

        currentTextSpeed = Mathf.Clamp(currentTextSpeed, 0f, 1.0f);

        dialogText.text = "";

        for(int i = 0; i < targetText.Length; i++)
        {
            if (currentTextSpeed > 0.9f)
                break;

            yield return new WaitForSeconds((1.0f - currentTextSpeed) / 10f);
            dialogText.text += targetText[i];
            
        }

        dialogText.text = targetText;

        isPlaying = false;
    }

    private void FinishText()
    {
        currentTextSpeed = 1.0f;
    }
}