using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacheScript : MonoBehaviour
{
    private Vector2 direction;
    private Vector2 currentDirection;
    public float speed;
    private void Awake() {
        //Donner une vitesse de base?
    }
    private void Update() {
        if(GameManager.inst.isPlaying){
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enclos"){
            GameManager.inst.WinLevel();
        }
        else if(other.tag == "WAF"){
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
}
