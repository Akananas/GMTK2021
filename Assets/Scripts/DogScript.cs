using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;   

public class DogScript : MonoBehaviour
{
    public DogControls Controls { get; private set; }
    private Vector2 _move;
    [SerializeField]
    private float _speedDog;
    private Rigidbody2D _rb2D;
    [SerializeField]
    private CameraShake _cameraShake;
    public Vector2 TargetDirection { get; private set; }
    [SerializeField]
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    public Animator Animation;
    // Start is called before the first frame update
    void Awake(){
        _audioSource = GetComponent<AudioSource>();
        Controls = new DogControls();
        _rb2D = GetComponent<Rigidbody2D>();
        Controls.GamePlay.Bark.performed += ctx => Bark();
        Controls.GamePlay.Bark.canceled += ctx => gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
        Controls.GamePlay.Start.performed += ctx => Animation.SetBool("title",true);
        Controls.GamePlay.Start.performed += ctx => GameManager.Instance.StartGame();
        Controls.GamePlay.Start.performed += ctx => Controls.GamePlay.Start.Disable();
    }

    // Update is called once per frame
    void Update(){ 
        if(GameManager.Instance.isPlaying){
            Vector2 mouseScreenPosition = Controls.GamePlay.MousePosition.ReadValue<Vector2>();
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            TargetDirection = mouseWorldPosition - _rb2D.position;
            float angle = Mathf.Atan2(TargetDirection.y, TargetDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion .Euler(new Vector3(0f,0f,angle));

            Vector3 movement = Controls.GamePlay.Run.ReadValue<Vector2>() * _speedDog;
            transform.position += movement * Time.deltaTime;
        }
    }

    void OnEnable(){
        Controls.GamePlay.Enable();
    }

    void OnDisable(){
        Controls.GamePlay.Disable();
    }


    private void Bark(){
        if(GameManager.Instance.isPlaying){
            _cameraShake.StartShaking(0.35f);
            gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
            _particleSystem.transform.position = transform.position;
            _particleSystem.transform.rotation = transform.rotation;
            _particleSystem.Play();
            _audioSource.Play();
        }
    }
}
