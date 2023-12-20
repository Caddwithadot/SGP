/*******************************************************************************
Author: Taylor
State: Complete
Description:
Keeps track of all current chatters and their seats.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatterManager : MonoBehaviour
{
    public List<int> seatList = new List<int>();
    public List<Vector3> positionList = new List<Vector3>();
}
