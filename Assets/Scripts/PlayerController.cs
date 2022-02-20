using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _angleLimit;

    [SerializeField] private ConfigurableJoint _armRight;
    [SerializeField] private Vector3 _passiveClumsinessLimit;
    [SerializeField] private Vector3 _activeClumsinessLimit;
    [SerializeField] private float _clumsyMinTime;
    [SerializeField] private float _clumsyMaxTime;
    [SerializeField] private float _strength;

    [SerializeField] private AudioClip _deliveryClip;
    [SerializeField] private AudioClip _wrongDoor;
    private CharacterController _characterController;
    private Camera _camera;
    private float _angleY;
    private float _angleX;
    private float _sensitivity;
    private float _gravityValue = -9.81f;

    private Vector3 _passiveClumsiness;
    private Vector3 _activeClumsiness;
    private bool _isClumsy = false;
    private bool _isMoving = false;
    private AudioSource _audioSource;
    private RaycastHit _hit;
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        _camera = GetComponentInChildren<Camera>();

        JointDrive jointDrive = _armRight.angularYZDrive;
        jointDrive.positionSpring = _strength;
        _armRight.angularYZDrive = jointDrive;

        _sensitivity = PlayerPrefs.GetFloat("sensitivity", .5f);
        _audioSource.volume = PlayerPrefs.GetFloat("volume", .5f);
    }

    void Update()
    {
        Move();
        Rotate();
        
        //Unbalance
        if(!_isClumsy) StartCoroutine(UnbalanceArm(Random.Range(_clumsyMinTime, _clumsyMaxTime)));

        Balance();

        if(_isMoving)_armRight.targetRotation = Quaternion.Euler(_passiveClumsiness.x + _activeClumsiness.x, _passiveClumsiness.y + _activeClumsiness.y, _passiveClumsiness.z + _activeClumsiness.z);
        else _armRight.targetRotation = Quaternion.identity;

        Interact();
    
    }

    void Move(){
        Vector2 movement = GameManager.Instance.InputManager.Movement;
        Vector3 motion = (transform.forward * movement.y + transform.right * movement.x) * _moveSpeed;
        motion.y = _gravityValue;
        motion *= Time.deltaTime;

        _activeClumsiness.x = movement.y * _activeClumsinessLimit.x;
        _activeClumsiness.z = movement.x * _activeClumsinessLimit.z;
        _characterController.Move(motion);

        _isMoving = movement.magnitude > .1f;
    }

    void Rotate(){
        // Rotate Character
        _angleY += GameManager.Instance.InputManager.Mouse.x * Time.deltaTime * _rotateSpeed * _sensitivity; 
        transform.eulerAngles = _angleY * Vector3.up;
        
        // Rotate Camera
        _angleX += GameManager.Instance.InputManager.Mouse.y * Time.deltaTime * _rotateSpeed * _sensitivity;
        if(Mathf.Abs(_angleX) > _angleLimit) _angleX = Mathf.Sign(_angleX) * _angleLimit;
        _camera.transform.localEulerAngles = _angleX * Vector3.left;
    }

    IEnumerator UnbalanceArm(float second){
        _isClumsy = true;

        _passiveClumsiness.x += Random.Range(-.5f, .5f) * _passiveClumsinessLimit.x;
        _passiveClumsiness.x = Mathf.Clamp(_passiveClumsiness.x, -_passiveClumsinessLimit.x, _passiveClumsinessLimit.x);

        _passiveClumsiness.y += Random.Range(-.5f, .5f) * _passiveClumsinessLimit.y;
        _passiveClumsiness.y = Mathf.Clamp(_passiveClumsiness.y, -_passiveClumsinessLimit.y, _passiveClumsinessLimit.y);

        _passiveClumsiness.z += Random.Range(-.5f, .5f) * _passiveClumsinessLimit.z;
        _passiveClumsiness.z = Mathf.Clamp(_passiveClumsiness.z, -_passiveClumsinessLimit.z, _passiveClumsinessLimit.z);
        yield return new WaitForSeconds(second);

        _isClumsy = false;
    }

    void Balance(){
         _activeClumsiness.y = -GameManager.Instance.InputManager.Balance * _activeClumsinessLimit.y;
    }

    void Interact(){
        if(!GameManager.Instance.InputManager.Use) return;

        if(Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward), out _hit, 4f)){
            if(_hit.transform.CompareTag("Item")){
                Vector3 spawnPos = GameManager.Instance.GameController.Tray.transform.position;

                spawnPos.x += Random.Range(-.3f, .3f);
                spawnPos.z += Random.Range(-.3f, .3f);

                spawnPos.y += .1f; 
                _hit.transform.eulerAngles = Vector3.zero;
                _hit.transform.position = spawnPos;

                _hit.transform.SetParent(GameManager.Instance.GameController.Tray.transform.parent);
            }

            if(_hit.transform.CompareTag("Door")){
                Door door = _hit.transform.GetComponent<Door>();

                if(GameManager.Instance.GameController.RoomNumber.Equals(door.RoomNumber)){
                    bool isDelivered = GameManager.Instance.GameController.Deliver();
                    if(isDelivered) PlaySound(_deliveryClip);
                }else PlaySound(_wrongDoor);
            }

        }
    }

    void PlaySound(AudioClip clip){
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
