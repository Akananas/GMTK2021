using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;   

public class Dog : MonoBehaviour
{
    public DogControls controls;
    Vector2 move;
    public float speedDog;
    public Rigidbody2D rb;
    [SerializeField]
    private CameraShake cameraShake;
    public Vector2 targetDirection;
    public ParticleSystem particleSystem;
    private AudioSource audioSource;
    private bool start;
    public Animator animation;
    // Start is called before the first frame update
    void Awake()
    {
        start = false;
        audioSource = GetComponent<AudioSource>();
        controls = new DogControls();
        controls.GamePlay.Bark.performed += ctx => Bark();
        controls.GamePlay.Bark.canceled += ctx => gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
        controls.GamePlay.Start.performed += ctx => animation.SetBool("title",true);
        controls.GamePlay.Start.performed += ctx => GameManager.inst.StartGame();
        controls.GamePlay.Start.performed += ctx => controls.GamePlay.Start.Disable();
    }

    // Update is called once per frame
    void Update()
    { 
        if(GameManager.inst.isPlaying){
            Vector2 mouseScreenPosition = controls.GamePlay.MousePosition.ReadValue<Vector2>();
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            targetDirection = mouseWorldPosition - rb.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion .Euler(new Vector3(0f,0f,angle));

            Vector3 movement = controls.GamePlay.Run.ReadValue<Vector2>() * speedDog;
            transform.position += movement * Time.deltaTime;
        }
    }

    void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    void OnDisable()
    {
        controls.GamePlay.Disable();
    }

    public void DisableInput(){
        controls.GamePlay.Bark.Disable();
    }

    public void EnableInput(){
        controls.GamePlay.Bark.Enable();
    }

    private void Bark(){
        cameraShake.StartShaking(0.35f);
        gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
        particleSystem.transform.position = transform.position;
        particleSystem.transform.rotation = transform.rotation;
        particleSystem.Play();
        audioSource.Play();
    }
}
