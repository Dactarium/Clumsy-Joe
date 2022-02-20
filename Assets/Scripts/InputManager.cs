using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 Movement {get; private set;}
    public Vector2 Mouse {get; private set;}

    public bool Use {get; private set;} = false;
    public bool Pause {get; private set;} = false;


    public float Balance {get; private set;}

    private Vector2 _movement;
    private Vector2 _mouse;
    private float _balance;
    bool isLeftBalancePressed = false;
    bool isRightBalancePressed = false;
    void Update(){
        if(GameManager.Instance.GameController.isGameEnded){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Movement = Vector2.zero;
            Mouse = Vector2.zero;
            return;
        }
        //Pause Input

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab)){
            Pause = !Pause;
        }
    
        if(Pause){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Movement = Vector2.zero;
            Mouse = Vector2.zero;
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;  
        }

        GameManager.Instance.UIController.showPauseMenu(Pause);

        if(Pause) return;

        //Move Input

        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");

        Movement = _movement;

        //Mouse Input

        _mouse.x = Input.GetAxis("Mouse X");
        _mouse.y = Input.GetAxis("Mouse Y");

        Mouse = _mouse;

        //Use Input

        if(Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0)){
            Use = true;
        }else{
            Use = false;
        }

        //Balance Input

        if(Input.GetKeyDown(KeyCode.Q)){
            isLeftBalancePressed = true;
        }

        if(Input.GetKeyUp(KeyCode.Q)){
            isLeftBalancePressed = false;
        }

        if(Input.GetKeyDown(KeyCode.E)){
            isRightBalancePressed = true;
        }

        if(Input.GetKeyUp(KeyCode.E)){
            isRightBalancePressed = false;
        }

        if(isLeftBalancePressed){
            _balance -= Time.deltaTime * 4;
        }

        if(isRightBalancePressed){
            _balance += Time.deltaTime * 4;
        }

        if(Mathf.Abs(_balance)>1) _balance = Mathf.Sign(_balance);

        Balance = _balance;
    }

    
}
