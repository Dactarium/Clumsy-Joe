using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItemOnTray : MonoBehaviour
{
    public int itemCount {get; private set;}

    void OnTriggerEnter(Collider other){
        if(!other.transform.CompareTag("Item")) return;
        other.transform.SetParent(transform.parent);
        itemCount++;
    }

    void OnTriggerExit(Collider other){
        if(!other.transform.CompareTag("Item")) return;
        other.transform.SetParent(null);
        itemCount--;
    }

    public void Delivered(){
        itemCount = 0;
    }
}
