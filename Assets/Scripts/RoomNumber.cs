using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomNumber : MonoBehaviour
{
    public TextMeshPro[] RoomNumbers;
    [SerializeField] private Door[] _doors;

    void Start(){
        for(int i = 0; i < RoomNumbers.Length; i++){
            _doors[i].RoomNumber = RoomNumbers[i].text;
        }
    } 
}
