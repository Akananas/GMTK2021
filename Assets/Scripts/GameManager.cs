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
    [SerializeField]
    private List<LevelObject> levels;
    private GameObject currentLoadedLevel;
    void Awake(){
        if (inst == null){

            inst = this;
            DontDestroyOnLoad(this.gameObject);
    
            //Rest of your Awake code
    
        } else {
            Destroy(this);
        }
        currentLevel = -1;
        LoadNextLevel();
    }


    public void VacheOutOfBound(){
        StopGame();
    }

    public void WinLevel(){
        Debug.Log("Win");
        //StopGame(); Remettre plus tard
        vache.direction = Vector3.zero;
        LoadNextLevel();
    }

    private void StopGame(){
        isPlaying = false;
        dog.DisableInput();
    }
    private void LoadNextLevel(){
        currentLevel++;
        if(currentLevel < levels.Count){
            Destroy(currentLoadedLevel);
            currentLoadedLevel = Instantiate(levels[currentLevel].level);
            dog.transform.position = levels[currentLevel].DogSpawn;
            vache.transform.position = levels[currentLevel].VacheSpawn;
        }
    }
}
