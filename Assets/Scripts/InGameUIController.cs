using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameUIController : MonoBehaviour
{   
    [SerializeField] private GameObject InGameUI;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject EndMenu;
    [SerializeField] private TextMeshProUGUI RoomNumber;
    [SerializeField] private TextMeshProUGUI Timer;
    [SerializeField] private TextMeshProUGUI Money;
    [SerializeField] private TextMeshProUGUI Salary;
    [SerializeField] private TextMeshProUGUI Total;

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMainMenu(){
        SceneManager.LoadScene(0);
    }

    public void showPauseMenu(bool show){
        InGameUI.SetActive(!show);
        PauseMenu.SetActive(show);
    }

    public void showEndMenu(){
        InGameUI.SetActive(false);
        PauseMenu.SetActive(false);
        EndMenu.SetActive(true);
    }

    public void SetRoomNumber(string number){
        RoomNumber.text = "Room: " + number;
    }

    public void SetTimer(float time){
        string timeText = TimeSpan.FromSeconds((double)time).ToString(@"mm\:ss");
        Timer.text = timeText;
    }

    public void SetMoney(int money){
        Money.text = money + " $";
    }

    public void setSalary(int salary){
        Salary.text = "+" + salary+ " $ Salary";

        Total.text = "Total: "+ (salary - 900) + " $";
    }

}
