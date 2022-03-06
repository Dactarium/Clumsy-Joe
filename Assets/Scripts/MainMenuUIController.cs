using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{   
    // Play Setting
    [SerializeField] private Slider _mapSizeMultiplierSlider;
    [SerializeField] private Slider _passiveClumsinessMultiplierSlider;
    [SerializeField] private Slider _activeClumsinessMultiplierSlider;

    // Settings
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _volumeSlider;

    // Menus
    [SerializeField] private GameObject _howToPlay;
    [SerializeField] private GameObject _playSettings;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _credits;

    // Twitch
    private bool _twitchPlay;
    private string _twitchName;

    void Awake(){
        // Play Settings
        PlayerPrefs.SetString("seed", RandomString(10));
        _mapSizeMultiplierSlider.value = PlayerPrefs.GetFloat("MapSizeMultiplier", 1f);
        _passiveClumsinessMultiplierSlider.value = PlayerPrefs.GetFloat("PassiveClumsinessMultiplier", 1f);
        _activeClumsinessMultiplierSlider.value = PlayerPrefs.GetFloat("ActiveClumsinessMultiplier", 1f);

        // Game Settings
        _sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", .1f);
        _volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
    }

    void Update(){
       // Game Settings
        PlayerPrefs.SetFloat("sensitivity", _sensitivitySlider.value);
        PlayerPrefs.SetFloat("volume", _volumeSlider.value);
    }

    public void Play(){
        // Play Settings
        PlayerPrefs.SetFloat("MapSizeMultiplier", _mapSizeMultiplierSlider.value);
        PlayerPrefs.SetFloat("PassiveClumsinessMultiplier", _passiveClumsinessMultiplierSlider.value);
        PlayerPrefs.SetFloat("ActiveClumsinessMultiplier", _activeClumsinessMultiplierSlider.value);
        
        if(_twitchPlay){
            PlayerPrefs.SetString("TwitchName", _twitchName);
            SceneManager.LoadScene(2);
        }else{
            PlayerPrefs.SetString("TwitchName", "");
            SceneManager.LoadScene(1);
        }
        
    }
    public void ShowPlaySettings(){
        if(_playSettings.activeSelf){
            _howToPlay.SetActive(true);
            _playSettings.SetActive(false);
            _settings.SetActive(false);
            _credits.SetActive(false);

            return;
        }
        _howToPlay.SetActive(false);
        _playSettings.SetActive(true);
        _settings.SetActive(false);
        _credits.SetActive(false);
    }

    public void ShowSettings(){
        if(_settings.activeSelf){
            _howToPlay.SetActive(true);
            _playSettings.SetActive(false);
            _settings.SetActive(false);
            _credits.SetActive(false);

            return;
        }
        _howToPlay.SetActive(false);
        _playSettings.SetActive(false);
        _settings.SetActive(true);
        _credits.SetActive(false);
    }

    public void ShowCredits(){
        if(_credits.activeSelf){
            _howToPlay.SetActive(true);
            _playSettings.SetActive(false);
            _settings.SetActive(false);
            _credits.SetActive(false);

            return;
        }
        _howToPlay.SetActive(false);
        _playSettings.SetActive(false);
        _settings.SetActive(false);
        _credits.SetActive(true);
    }

    public void ShowWeb(){
        Application.OpenURL("https://dactarium.dev/");
    }

    public void SetSeed(string seed){
        if(seed.Equals("") || seed == null) seed = RandomString(10);
        PlayerPrefs.SetString("seed", seed);
    }

    public void SetTwitchPlay(bool isTwitch){
        _twitchPlay = isTwitch;
    }

    public void SetTwitchName(string name){
        _twitchName = name.ToLower();
    }

    string RandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrsuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        string randomString = "";

        for(int i = 0; i < Random.Range(3, length); i++){
            randomString += chars[Random.Range(0, chars.Length)];
        }
        
        return randomString;
    }
}
