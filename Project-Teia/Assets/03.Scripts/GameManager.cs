using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : SingleTon<GameManager> {

    [Header("Money")]
    [SerializeField] Money money;

    [Header("Energy")]
    [SerializeField] Energy energy;

    [Header("EXP")]
    [SerializeField] EXP exp;

    public Money Money { get { return money; } }
    public Energy Energy { get { return energy; } }
    public EXP Exp { get { return exp; } }

    void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
