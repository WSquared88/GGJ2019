﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RoomTypes
{
    Bathroom,
    Bedroom,
    Kitchen,
    Office,
    Floor,

    RoomTypesCount
}

/// <summary>
/// Extension of the PickupComponent which represents any pickup-able component that is part of a room.
/// </summary>
public class RoomPickup : PickupComponent
{
    [SerializeField]
    RoomTypes RoomType;
}
