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
        controls.GamePlay.Bark.performed += ctx => gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
        controls.GamePlay.Bark.performed += ctx => particleSystem.transform.position = transform.position;
        controls.GamePlay.Bark.performed += ctx => particleSystem.transform.rotation = transform.rotation;
        controls.GamePlay.Bark.performed += ctx => particleSystem.Play();
        controls.GamePlay.Bark.performed += ctx => audioSource.Play();
        controls.GamePlay.Bark.canceled += ctx => gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {   if(start){
        if(GameManager.inst.isPlaying){
            Vector2 mouseScreenPosition = controls.GamePlay.MousePosition.ReadValue<Vector2>();
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            targetDirection = mouseWorldPosition - rb.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion .Euler(new Vector3(0f,0f,angle));

            Vector3 movement = controls.GamePlay.Run.ReadValue<Vector2>() * speedDog;
            transform.position += movement * Time.deltaTime;
        }
        }else{
            controls.GamePlay.Start.performed += ctx => animation.SetBool("title",true);
            controls.GamePlay.Start.performed += ctx => start = true;
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
        controls.GamePlay.Disable();
    }
}
