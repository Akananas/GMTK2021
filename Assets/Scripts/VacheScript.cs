using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacheScript : MonoBehaviour
{
    private Vector2 basePos;
    [SerializeField]
    private Vector2 endPos;
    private Vector2 direction;
    private Vector2 currentDirection;
    public float speed;
    public GameObject Dog;
    public bool available {get;private set;} = true;
    private void Awake() {
        basePos = this.transform.position;
        direction = (endPos - basePos).normalized;
    }
    private void Update() {
        if(!available){
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    public void StartMoving(){
        currentDirection = direction;
        available = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enclos" && !available){
            available = true;
            direction = (endPos - basePos).normalized;
        }
        else if(other.tag == "WAF" && !available){
            Vector2 directionWaf = other.GetComponentInParent<Dog>().targetDirection.normalized;
            direction = directionWaf;
            //available = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Delete"){
            GameManager.inst.CheckLose(this);
            Destroy(this.gameObject);
        }
    }
}
