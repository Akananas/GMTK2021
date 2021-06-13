using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacheScript : MonoBehaviour
{
    public Vector2 direction;
    private Vector2 currentDirection;
    public float speed;
    public bool isDone;
    private void Awake() {
        //Donner une vitesse de base?
    }
    private void Update() {
        if(GameManager.inst.isPlaying){
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enclos" && !isDone){
            isDone = true;
            GameManager.inst.CheckWinLevel();
        }
        else if(other.tag == "WAF" && !isDone){
            Vector2 directionWaf = other.GetComponentInParent<Dog>().targetDirection.normalized;
            direction = directionWaf;
            //available = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Delete"){
            GameManager.inst.VacheOutOfBound();
            Destroy(this.gameObject);
        }
    }

    public void Reset(Vector3 pos){
        isDone = false;
        transform.position = pos;
        direction = Random.insideUnitCircle;
    }
}
