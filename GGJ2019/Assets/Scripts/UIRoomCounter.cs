using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomCounter : MonoBehaviour
{
    [System.Serializable]
    struct RoomDescriptor
    {
        public Sprite RoomName;
        public RoomTypes UIRoomType;
    }

    [SerializeField]
    InventorySystem PlayerInventory;
    [SerializeField]
    float FloorHeight;
    [SerializeField]
    float FloorWidth;
    List<Image> Floors;

	// Use this for initialization
	void Start ()
    {
        Debug.Assert(PlayerInventory, "The player inventory field on the UIRoomCounter wasn't set!");
        Debug.Assert(FloorHeight > 0.0f + float.Epsilon, "The floor height wasn't set! The floors will stack on top of each other!");
        Debug.Assert(FloorWidth > 0.0f + float.Epsilon, "The floor width wasn't set! The rooms will be stacked on top of each other!");
        PlayerInventory.SubscribeToPickedUpEvent(PickedUpEventHandler);
        Floors = new List<Image>();
    }

    void PickedUpEventHandler(PickupComponent pickup_component)
    {
        RoomPickup room_pickup = pickup_component as RoomPickup;

        if(room_pickup)
        {
            RoomTypes room_type = room_pickup.GetRoomType();
            Sprite room_sprite = room_pickup.GetRoomSprite();

            if (room_type == RoomTypes.Floor)
            {
                SpawnFloorUI(room_pickup);
            }
            else
            {
                SpawnRoomUI(room_pickup);
            }
        }
    }

    void SpawnFloorUI(RoomPickup room_pickup)
    {
        GameObject floor_obj = new GameObject();
        Image floor_image = floor_obj.AddComponent<Image>();
        floor_image.sprite = room_pickup.GetRoomSprite();
        RectTransform rect_transform = floor_obj.GetComponent<RectTransform>();
        rect_transform.SetParent(transform);
        //Set the pivot position to the bottom middle
        rect_transform.anchorMin = new Vector2(0.5f, 0.0f);
        rect_transform.anchorMax = new Vector2(0.5f, 0.0f);
        rect_transform.pivot = new Vector2(0.5f, 0.0f);
        //Subtract one because the inventory has already marked this floor as added, but we want to know the pre-add numbers
        rect_transform.anchoredPosition = new Vector3(0, FloorHeight / 2 * (PlayerInventory.GetRoomCount(RoomTypes.Floor) - 1), 0);
        rect_transform.sizeDelta = new Vector2(FloorWidth, FloorHeight);
        Floors.Add(floor_image);
    }

    void SpawnRoomUI(RoomPickup room_pickup)
    {
        //Get floor info
        int num_floors = PlayerInventory.GetRoomCount(RoomTypes.Floor);
        //Subtract one because the inventory has already marked this room as added, but we want to know the pre-add numbers
        int num_all_non_floor_rooms = PlayerInventory.GetCountAllRooms() - num_floors - 1;
        int max_num_rooms_per_floor = PlayerInventory.GetMaxNumRoomsPerFloor();
        Image floor = Floors[num_all_non_floor_rooms / max_num_rooms_per_floor];
        RectTransform floor_rect_transform = floor.GetComponent<RectTransform>();

        //Make this room
        GameObject room_obj = new GameObject();
        Image room_image = room_obj.AddComponent<Image>();
        room_image.sprite = room_pickup.GetRoomSprite();
        RectTransform room_rect_transform = room_obj.GetComponent<RectTransform>();

        //Set the rooms location
        room_rect_transform.SetParent(floor.transform);
        room_rect_transform.sizeDelta = new Vector2(floor_rect_transform.sizeDelta.x / max_num_rooms_per_floor, floor_rect_transform.sizeDelta.y);
        //Set the pivot position to the bottom left corner
        room_rect_transform.anchorMin = new Vector2(0.0f, 0.0f);
        room_rect_transform.anchorMax = new Vector2(0.0f, 0.0f);
        room_rect_transform.pivot = new Vector2(0.0f, 0.0f);
        room_rect_transform.anchoredPosition = new Vector2(num_all_non_floor_rooms % max_num_rooms_per_floor * room_rect_transform.sizeDelta.x, 0);
    }
}
