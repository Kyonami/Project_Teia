using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TalkerFace {
    EMOTIONLESS = 0,
    CASUAL,
    SMILE,
    BRIGHTSMILE,
    CURIOUS,
    SURPRISE,
    EMBARRASSED,
    EMOTIONMAX,
    SERIOUS,
    EXPECT
}

public class Talker : MonoBehaviour{
    [SerializeField]
    private string myName;
    public string MyName { get { return myName; } }
    [SerializeField]
    private List<Sprite> mySpriteList;
    public TalkerFace myFace = 0;

    public void ChangeFace(string face)
    {
        switch (face)
        {
            case "무표정":
                myFace = TalkerFace.EMOTIONLESS;
                break;
            case "무심":
                myFace = TalkerFace.CASUAL;
                break;
            case "웃음":
                myFace = TalkerFace.SMILE;
                break;
            case "밝게 웃음":
                myFace = TalkerFace.BRIGHTSMILE;
                break;
            case "놀람":
                myFace = TalkerFace.SURPRISE;
                break;
            case "당황":
                myFace = TalkerFace.EMBARRASSED;
                break;
            case "진지":
                myFace = TalkerFace.SERIOUS;
                break;
            case "기대":
                myFace = TalkerFace.EXPECT;
                break;
        }
    }
    public Sprite GetMySprite()
    {
        return mySpriteList[(int)TalkerFace.EMOTIONLESS];
        //return mySprite[(int)myFace];
    }
}