using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonDialogue : MonoBehaviour
{

    [System.Serializable]
    public struct Assets
    {
        public Sprite Bathroom;
        public Sprite Bedroom;
        public Sprite Kitchen;
        public Sprite Office;
        public Sprite Floor;
    }

    [SerializeField]
    Assets SpriteAssets;
    [SerializeField]
    Person AttachedPerson;
    [SerializeField]
    Transform Checkmark;
    [SerializeField]
    Transform X;


    Sprite TypeToSprite(RoomTypes roomTypes)
    {
        switch (roomTypes)
        {
            case RoomTypes.Bathroom:
                return SpriteAssets.Bathroom;
            case RoomTypes.Bedroom:
                return SpriteAssets.Bedroom;
            case RoomTypes.Kitchen:
                return SpriteAssets.Kitchen;
            case RoomTypes.Office:
                return SpriteAssets.Office;
            case RoomTypes.Floor:
                return SpriteAssets.Floor;
            default:
                return null;
        }
    }

    void AttachDialogueSprite(RoomTypes[] types, Transform parent, string name)
    {
        Sprite spr = TypeToSprite(types[0]);
        GameObject spawned_sprite = new GameObject(name);
        SpriteRenderer spawned_renderer = spawned_sprite.AddComponent<SpriteRenderer>();
        spawned_renderer.sprite = spr;
        spawned_sprite.transform.parent = parent;
        spawned_sprite.transform.localPosition = Vector3.zero;
        spawned_sprite.transform.localScale = Vector3.one * 2;
        Vector3 pos = spawned_sprite.transform.position;
        pos.y = transform.position.y + spawned_renderer.bounds.extents.y / 2;
        spawned_sprite.transform.position = pos;
    }

	// Use this for initialization
	void Start ()
    {
        if (AttachedPerson.Likes.Length > 0)
        {
            AttachDialogueSprite(AttachedPerson.Likes, Checkmark, "Like");
        }
        if (AttachedPerson.Dislikes.Length > 0)
        {
            AttachDialogueSprite(AttachedPerson.Dislikes, X, "Dislike");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
