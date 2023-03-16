using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 5.25 3.5 1.75 0 1.75 3.5 5.25
public class CharacterDirection : MonoBehaviour {
    
    private Vector3 centerPos = new Vector3(0, 0, 0);
    private Vector3 firstPos = new Vector3(1.75f, 0, 0);
    private Vector3 outside = new Vector3(6, 0, 0);

    [Header("Values")]
    [SerializeField]
    private float directionSpeed;

    [Header("Components")]
    [SerializeField]
    private CameraAction theCameraAction;
    [SerializeField]
    private UIAnimation myUIAnimation;

    [Header("Objects")]
    [SerializeField]
    private List<Sprite> characterSprites;
    [SerializeField]
    private List<Talker> talkerList;
    [SerializeField]
    private GameObject character_A, character_B, character_C;
    [SerializeField]
    private SpriteRenderer charSpr_A, charSpr_B, charSpr_C;

    public void Awake()
    {
        //Sprite[] temp = Resources.LoadAll<Sprite>("CharacterSprites");
        Talker[] _talkerArray = Resources.LoadAll<Talker>("Talkers");

        for (int i = 0; i < _talkerArray.Length; i++)
        {
            talkerList.Add(_talkerArray[i]);
        }
        charSpr_A = character_A.GetComponent<SpriteRenderer>();
        charSpr_B = character_B.GetComponent<SpriteRenderer>();
        charSpr_C = character_C.GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        character_A.SetActive(false);
        character_B.SetActive(false);
        character_C.SetActive(false);
    }
    public void ChangeFace(string name, string face)
    {
        Talker _temp = talkerList.Find(o => o.MyName.Equals(name));

        _temp.ChangeFace(face);
    }
    public void PlayDirection(string value, string name_A, string name_B, string name_C)
    {
        if (name_A.Length != 0) charSpr_A.sprite = talkerList.Find(o => o.MyName.Equals(name_A)).GetMySprite();
        if (name_B.Length != 0) charSpr_B.sprite = talkerList.Find(o => o.MyName.Equals(name_B)).GetMySprite();
        if (name_C.Length != 0) charSpr_C.sprite = talkerList.Find(o => o.MyName.Equals(name_C)).GetMySprite();

        switch (value)
        {   
            case "1":
                Direction1();
                break;
            case "2":
                Direction2();
                break;
            case "3":
                Direction3();
                break;
            case "4":
                Direction4();
                break;
            case "5-1":
                Direction5_1();
                break;
            case "5-2":
                Direction5_2();
                break;
            case "6-1":
                Direction6_1();
                break;
            case "6-2":
                Direction6_2();
                break;
            case "7-1":
                Direction7_1();
                break;
            case "7-2":
                Direction7_2();
                break;
            case "8-1":
                Direction8_1();
                break;
            case "8-2":
                Direction8_2();
                break;
            case "9":
                Direction9();
                break;
            case "10":
                Direction10();
                break;
            case "11":
                Direction11();
                break;
        }
    }
    // 0 + 1
    void Direction1()  
    {
        character_A.transform.localPosition = Vector3.zero;
        character_A.SetActive(true);
        character_B.SetActive(false);
        character_C.SetActive(false);
    }
    // 1 - 1
    void Direction2()
    {
        character_A.SetActive(false);
        character_B.SetActive(false);
        character_C.SetActive(false);
    }
    // 0 + 2
    void Direction3()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);
    }
    // 2 - 2
    void Direction4()
    {
        character_A.SetActive(false);
        character_B.SetActive(false);
    }
    // 1 + 1 from left
    void Direction5_1()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);

        StartCoroutine(myUIAnimation.MoveObject(character_A, centerPos, firstPos, directionSpeed));
        StartCoroutine(myUIAnimation.MoveObject(character_B, -outside, -firstPos, directionSpeed));
    }
    // 1 + 1 | space was prepared and enter from left
    void Direction5_2()
    { 
        character_A.SetActive(true);
        character_B.SetActive(true);

        character_A.transform.localPosition = firstPos;
        StartCoroutine(myUIAnimation.MoveObject(character_B, -outside, -firstPos, directionSpeed));
    }
    // 1 + 1 from right
    void Direction6_1()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);

        StartCoroutine(myUIAnimation.MoveObject(character_A, centerPos, -firstPos, directionSpeed));
        StartCoroutine(myUIAnimation.MoveObject(character_B, outside, firstPos, directionSpeed));
    }
    // 1 + 1 | space was prepared and enter from right
    void Direction6_2()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);

        character_A.transform.localPosition = -firstPos;
        StartCoroutine(myUIAnimation.MoveObject(character_B, outside, firstPos, directionSpeed));
    }
    // 2 - 1 | out to left and modulate left one's sit
    void Direction7_1()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);

        StartCoroutine(myUIAnimation.MoveObject(character_A, -firstPos, -outside, directionSpeed));
        StartCoroutine(myUIAnimation.MoveObject(character_B, firstPos, centerPos, directionSpeed));
    }
    // 2 - 1 | out to left
    void Direction7_2()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);

        StartCoroutine(myUIAnimation.MoveObject(character_A, -firstPos, -outside, directionSpeed));
        character_B.transform.localPosition = firstPos;
    }
    // 2 - 1 | out to left and modulate left one's sit
    void Direction8_1()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);

        StartCoroutine(myUIAnimation.MoveObject(character_A, -firstPos, centerPos, directionSpeed));
        StartCoroutine(myUIAnimation.MoveObject(character_B, firstPos, outside, directionSpeed));
    }
    // 2 - 1 | out to left
    void Direction8_2()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);

        character_A.transform.localPosition = -firstPos;
        StartCoroutine(myUIAnimation.MoveObject(character_B, firstPos, outside, directionSpeed));
    }
    // 2 - 1 + 1 | from left
    void Direction9()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);
        character_C.SetActive(true);

        StartCoroutine(myUIAnimation.MoveObject(character_A, -firstPos, firstPos, directionSpeed));
        StartCoroutine(myUIAnimation.MoveObject(character_B, firstPos, outside, directionSpeed));
        StartCoroutine(myUIAnimation.MoveObject(character_C, -outside, -firstPos, directionSpeed));
    }
    // 2 - 1 + 1 | from right
    void Direction10()
    {
        character_A.SetActive(true);
        character_B.SetActive(true);
        character_C.SetActive(true);

        StartCoroutine(myUIAnimation.MoveObject(character_A, -firstPos, -outside, directionSpeed));
        StartCoroutine(myUIAnimation.MoveObject(character_B, firstPos, -firstPos, directionSpeed));
        StartCoroutine(myUIAnimation.MoveObject(character_C, outside, firstPos, directionSpeed));
    }
    // Shake Screen
    void Direction11()
    {
        theCameraAction.PlayDamagedAction();
    }
}