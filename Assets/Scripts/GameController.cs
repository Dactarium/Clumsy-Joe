using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{   
    public bool isGameEnded {get; private set;} = false;
    public GameObject Tray;

    [SerializeField] private float _timeLimit;
    [SerializeField] private float _timeGain;
    [SerializeField] private int _baseMoneyGain;
    [SerializeField] private int _timeBonus;

    [SerializeField] private AudioClip _lastTimes;
    [SerializeField] private AudioClip _gameEnd;

    public string RoomNumber {get; private set;}
    private List<string> _roomNumbers;
    private int _orderCount;
    private CheckItemOnTray _checkItemOnTray;
    private int _totalMoney = 0;
    private float _timer;
    private AudioSource _audioSource;
    public void Setup(){
        _checkItemOnTray = Tray.GetComponentInChildren<CheckItemOnTray>();

        _roomNumbers = GameManager.Instance.MapGenerator.RoomNumbers;

        PickRoom();

        _timer = _timeLimit;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = PlayerPrefs.GetFloat("volume", .5f);
    }

    void Update(){
        if(_timer >= 0) GameManager.Instance.UIController.SetTimer(_timer);
        else if(!isGameEnded) EndGame();

        if(isGameEnded) return;
        
        _timer -= Time.deltaTime;

        if(_timer <= 10 && !_audioSource.isPlaying){
            _audioSource.loop = true;
            _audioSource.clip = _lastTimes;
            _audioSource.Play();
        }else if(_timer > 10 && _audioSource.isPlaying){
            _audioSource.Stop();
        }
    }

    void PickRoom(){
        RoomNumber = _roomNumbers[Random.Range(0, _roomNumbers.Count)];
        GameManager.Instance.UIController.SetRoomNumber(RoomNumber);
        _orderCount = GameManager.Instance.ItemSpawner.spawn(Tray.transform);

        _timer += _timeGain;
        if(_timer > _timeLimit) _timer = _timeLimit;
    }

    public bool Deliver(){
        _totalMoney += (_baseMoneyGain + Mathf.FloorToInt(_timeBonus * _timer / _timeLimit) ) * _checkItemOnTray.itemCount / _orderCount;
            
        GameManager.Instance.UIController.SetMoney(_totalMoney);

        _checkItemOnTray.Delivered();
        PickRoom();
        return true;
    }

    void EndGame(){
        isGameEnded = true;

        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = _gameEnd;
        _audioSource.Play();

        GameManager.Instance.UIController.showEndMenu();
        GameManager.Instance.UIController.setSalary(_totalMoney);
    }
}
