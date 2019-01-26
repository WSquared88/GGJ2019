using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomCounter : MonoBehaviour
{
    [System.Serializable]
    struct RoomDescriptor
    {
        public string RoomName;
        public RoomTypes UIRoomType;
    }

    [SerializeField]
    RoomDescriptor[] RoomUIText;
    [SerializeField]
    InventorySystem PlayerInventory;
    Text[] RoomTextComponents;

	// Use this for initialization
	void Start ()
    {
        RoomTextComponents = GetComponentsInChildren<Text>();
        Debug.Assert(PlayerInventory != null, "The player inventory field on the UIRoomCounter wasn't set!");
	}
	
	// Update is called once per frame
	void Update ()
    {
		for(int i = 0;i<RoomTextComponents.Length;i++)
        {
            RoomTextComponents[i].text = string.Format(RoomUIText[i].RoomName, PlayerInventory.GetRoomCount(RoomUIText[i].UIRoomType));
        }
	}
}
