using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowScript : MonoBehaviour
{
    public Vector2 Direction = new Vector2();
    private Vector2 _currentDirection;
    [SerializeField]
    private float _speed;
    public bool IsDone { get; private set; }

    private void Update() {
        if(GameManager.Instance.isPlaying){
            transform.Translate(Direction * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enclos" && !IsDone){
            IsDone = true;
            Direction = Vector2.zero;
            GameManager.Instance.CheckWinLevel();
        }
        else if(other.tag == "WAF" && !IsDone){
            Vector2 directionWaf = other.GetComponentInParent<DogScript>().TargetDirection.normalized;
            Direction = directionWaf;
            //available = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Delete"){
            GameManager.Instance.VacheOutOfBound();
            Destroy(this.gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        var normal = other.contacts[0].normal;
        if(normal.x != 0){
            Direction.x = -Direction.x * 0.5f;
        }
        if(normal.y != 0){
            Direction.y = -Direction.y * 0.5f;
        }
    }
    public void Reset(Vector3 pos){
        IsDone = false;
        transform.position = pos;
        Direction = Random.insideUnitCircle;
    }
}
