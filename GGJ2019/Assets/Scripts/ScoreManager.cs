using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScoreManager : MonoBehaviour
{
    public static float currScore { get; private set; }
    public static ScoreManager Instance;
    [SerializeField]
    float InitialSellValue;
    [SerializeField]
    InventorySystem Inventory;

    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Assert(true, "There is more than one Score Manager trying to be spawned! This isn't allowed!");
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        Debug.Assert(Inventory, "The Inventory on the " + gameObject.name + " is missing!");
        SpawnManager.PlayerRespawned += PlayerRespawnHandler;
        currScore = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //CalcSellValue();
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SellHouse();
        //    SceneManager.LoadScene("GameOverScene");
        //}
	}
    void CalcSellValue()
    {
        //Sum the captured people's current value and sell it
        //-do this on a per person basis 
        if (Inventory)
        {
            currScore += InitialSellValue;

            foreach (Person person in Inventory.People)
            {
                float person_addition = person.BuyerTime * person.PercentModifier * InitialSellValue;
                currScore += person_addition;
            }
        }
    }
    public void SellHouse()
    {
        CalcSellValue();
    }

    void PlayerRespawnHandler(GameObject new_player)
    {
        Inventory = new_player.GetComponent<InventorySystem>();
    }
}
