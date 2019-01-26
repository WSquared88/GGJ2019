using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    float currScore;
    float sellValue;

	// Use this for initialization
	void Start () {
        currScore = 0;
        sellValue = 0;
	}
	
	// Update is called once per frame
	void Update () {
        CalcSellValue();
	}
    void CalcSellValue()
    {
        //Sum the captured people's current value and sell it
            //-do this on a per person basis 
        return;
    }
    void SellHouse()
    {
        currScore = currScore + sellValue;
    }
}
