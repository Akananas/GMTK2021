using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public VacheScript vache;
    public Dog dog;
    public float nextMove;
    private int currentLevel;
    public static GameManager inst;
    public bool isPlaying = true;
    void Awake(){
        if (inst == null){

            inst = this;
            DontDestroyOnLoad(this.gameObject);
    
            //Rest of your Awake code
    
        } else {
            Destroy(this);
        }
    }


    public void VacheOutOfBound(){
        StopGame();
    }

    public void WinLevel(){
        Debug.Log("Win");
        StopGame();
    }

    private void StopGame(){
        isPlaying = false;
        dog.DisableInput();
    }
}
