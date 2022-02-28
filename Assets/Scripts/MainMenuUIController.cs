using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private GameObject _howToPlay;
    [SerializeField] private GameObject _play;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _credits;

    void Awake(){
        _sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", .1f);
        _volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
        PlayerPrefs.SetString("seed", RandomString(10));
    }

    void Update(){
        PlayerPrefs.SetFloat("sensitivity", _sensitivitySlider.value);
        PlayerPrefs.SetFloat("volume", _volumeSlider.value);
    }

    public void Play(float difficulty){
        PlayerPrefs.SetFloat("difficulty", difficulty);
        SceneManager.LoadScene(1);
    }
    public void ShowPlay(){
        if(_play.activeSelf){
            _howToPlay.SetActive(true);
            _play.SetActive(false);
            _settings.SetActive(false);
            _credits.SetActive(false);

            return;
        }
        _howToPlay.SetActive(false);
        _play.SetActive(true);
        _settings.SetActive(false);
        _credits.SetActive(false);
    }

    public void ShowSettings(){
        if(_settings.activeSelf){
            _howToPlay.SetActive(true);
            _play.SetActive(false);
            _settings.SetActive(false);
            _credits.SetActive(false);

            return;
        }
        _howToPlay.SetActive(false);
        _play.SetActive(false);
        _settings.SetActive(true);
        _credits.SetActive(false);
    }

    public void ShowCredits(){
        if(_credits.activeSelf){
            _howToPlay.SetActive(true);
            _play.SetActive(false);
            _settings.SetActive(false);
            _credits.SetActive(false);

            return;
        }
        _howToPlay.SetActive(false);
        _play.SetActive(false);
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
