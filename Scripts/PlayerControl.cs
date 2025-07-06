using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(NavMeshAgent))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 10f;
    public Animator animator;
    private NavMeshAgent _navMeshAgent;
    private Joystick _joystick;
    private Rigidbody _rigidbody;
    private Camera _playerCamera; 
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
            return;
        }
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = speed; 
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updatePosition = true;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        if (_joystick == null)
        {
            _joystick = FindObjectOfType<FloatingJoystick>();
        }
    }
    void FixedUpdate()
    {
        if (_joystick == null || _playerCamera == null) return; 
        Vector3 directionInput = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;

        if (directionInput.magnitude >= 0.1f)
        {
            Vector3 cameraForward = _playerCamera.transform.forward;
            Vector3 cameraRight = _playerCamera.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();
            Vector3 moveDirection = (cameraForward * directionInput.z + cameraRight * directionInput.x).normalized;

            speed = 7.0f; 
            _navMeshAgent.isStopped = false;
            _navMeshAgent.Move(moveDirection * speed * Time.deltaTime); 
            transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            animator.SetBool("isRun", true);
            if (!audioManager.SFXSource.isPlaying || audioManager.SFXSource.clip != audioManager.RunningSound)
            {
                audioManager.triggerRun();
            }
        }
        else
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;
            animator.SetBool("isRun", false);
            audioManager.stopRun();
        }
    }
    public void SetJoystick(Joystick joystick)
    {
        _joystick = joystick;
    }
    public void SetPlayerCamera(Camera camera)
    {
        _playerCamera = camera;
    }
}