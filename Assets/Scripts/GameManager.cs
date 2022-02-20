using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}

    public GameController GameController{get; private set;}
    public InputManager InputManager{get; private set;}
    public InGameUIController UIController{get; private set;}
    public MapGenerator MapGenerator{get; private set;}
    public ItemSpawner ItemSpawner{get; private set;}
    void Awake(){
        Instance = this;

        Random.InitState(PlayerPrefs.GetString("seed").GetHashCode());

        GameController = GetComponentInChildren<GameController>();
        InputManager = GetComponentInChildren<InputManager>();
        UIController = GetComponentInChildren<InGameUIController>();
        MapGenerator = GetComponentInChildren<MapGenerator>();
        ItemSpawner = GetComponentInChildren<ItemSpawner>();
    }
}
