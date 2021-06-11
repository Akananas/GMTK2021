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

    // Start is called before the first frame update
    void Awake()
    {
        controls = new DogControls();
        controls.GamePlay.Bark.performed += ctx => Debug.Log("WAF!!");
    }

    // Update is called once per frame
    void Update()
    {   
        Vector2 mouseScreenPosition = controls.GamePlay.MousePosition.ReadValue<Vector2>();
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 targetDirection = mouseWorldPosition - rb.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion .Euler(new Vector3(0f,0f,angle));

        Vector3 movement = controls.GamePlay.Run.ReadValue<Vector2>() * speedDog;
        transform.position += movement * Time.deltaTime;
    }

    void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    void OnDisable()
    {
        controls.GamePlay.Disable();
    }
}
