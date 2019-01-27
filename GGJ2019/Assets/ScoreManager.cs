using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScoreManager : MonoBehaviour {
    public static float currScore { get; private set; }
    [SerializeField]
    float InitialSellValue;
    float CurrSellValue;
    InventorySystem Inventory;

	// Use this for initialization
	void Start () {
        Inventory = GetComponent<InventorySystem>();
	}
	
	// Update is called once per frame
	void Update () {
        CalcSellValue();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SellHouse();
            SceneManager.LoadScene("GameOverScene");
        }
	}
    void CalcSellValue()
    {
        //Sum the captured people's current value and sell it
        //-do this on a per person basis 
        CurrSellValue = InitialSellValue;
        foreach (Person person in Inventory.People)
        {
            float person_addition = person.BuyerTime * person.PercentModifier * InitialSellValue;
            CurrSellValue += person_addition;
        }
    }
    void SellHouse()
    {
        currScore = currScore + CurrSellValue;
    }
}
