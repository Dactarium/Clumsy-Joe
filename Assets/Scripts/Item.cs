using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    /*bool hasParent = false;
    bool isRunning = false;*/
    void Awake(){
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume", 1f);
    }
    void OnCollisionEnter(Collision other){
        if(!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Item")) GetComponent<AudioSource>().Play();
        /*if(!other.gameObject.CompareTag("Player")) return;
        
        
        transform.SetParent(other.transform);
        hasParent = true;*/
    }

    /*void OnCollisionExit(Collision other){
        if(!other.gameObject.CompareTag("Player")) return;
        
        hasParent = false;
        if(!isRunning)StartCoroutine(ExitParent());
        
    }

    IEnumerator ExitParent(){
        isRunning = true;
        yield return new WaitForSeconds(.2f);

        if(!hasParent) transform.SetParent(null);

        isRunning = false;
    }*/
}
