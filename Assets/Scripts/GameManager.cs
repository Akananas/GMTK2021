using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<VacheScript> vaches;
    public float nextMove;
    private float currentTimer;
    public static GameManager inst;
    bool isPlaying = true;
    void Awake(){

        if (inst == null){

            inst = this;
            DontDestroyOnLoad(this.gameObject);
    
            //Rest of your Awake code
    
        } else {
            Destroy(this);
        }
    }

    private void Update() {
        if(isPlaying){
            currentTimer += Time.deltaTime;
            if(currentTimer >= nextMove && VacheAvailable()){
                List<VacheScript> availableVaches = GetAvailable();
                availableVaches[Random.Range(0,availableVaches.Count-1)].StartMoving();
                currentTimer = 0;
            }
            else if(currentTimer >= nextMove){
                currentTimer = 0;
            }
        }
    }

    private bool VacheAvailable(){
        foreach(VacheScript vache in vaches){
            if(vache.available){
                return true;
            }
        }
        return false;
    }

    private List<VacheScript> GetAvailable(){
        List<VacheScript> tmp = new List<VacheScript>();
        foreach(VacheScript v in vaches){
            if(v.available){
                tmp.Add(v);
            }
        }
        return tmp;
    }

    public void CheckLose(VacheScript vache){
        vaches.Remove(vache);
        if(vaches.Count <=2){
            Debug.Log("Lose");
            isPlaying = false;
        }
    }
}
