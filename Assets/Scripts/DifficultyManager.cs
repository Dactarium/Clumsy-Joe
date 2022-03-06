using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static float MapSizeMultiplier {get; private set;}
    public static float PassiveClumsinessMultiplier {get; private set;}
    public static float ActiveClumsinessMultiplier {get; private set;}
    void Awake(){
        MapSizeMultiplier = PlayerPrefs.GetFloat("MapSizeMultiplier", 1f);
        PassiveClumsinessMultiplier = PlayerPrefs.GetFloat("PassiveClumsinessMultiplier", 1f);
        ActiveClumsinessMultiplier = PlayerPrefs.GetFloat("ActiveClumsinessMultiplier", 1f);
    }

}
