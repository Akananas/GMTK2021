using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<VacheScript> vaches;
    public Dog dog;
    public float nextMove;
    private int currentLevel;
    public static GameManager inst;
    public bool isPlaying = true;
    [SerializeField]
    private List<LevelObject> levels;
    private GameObject currentLoadedLevel;
    public GameObject vachePrefab;

    public Animator animation;
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

    public void CheckWinLevel(){
        bool won = true;
        //StopGame(); Remettre plus tard
        foreach(VacheScript vache in vaches){
            if(!vache.isDone){
                won = false;
            }
        }
        if(won){
            foreach(VacheScript vache in vaches){
                vache.direction = Vector3.zero;
            }
            StartCoroutine("Fade");
        }

    }

    private void StopGame(){
        isPlaying = false;
        dog.DisableInput();
    }
    private void LoadNextLevel(){
        currentLevel++;
        if(currentLevel < levels.Count){
            Destroy(currentLoadedLevel);
            LevelObject newLevel = levels[currentLevel];
            currentLoadedLevel = Instantiate(newLevel.level);
            dog.transform.position = newLevel.DogSpawn;
            if(vaches.Count > newLevel.nbrVaches){
                for(int i = vaches.Count - 1; i >= newLevel.nbrVaches;i--){
                    Destroy(vaches[i].gameObject);
                    vaches.RemoveAt(i);
                }
            }else if (vaches.Count < newLevel.nbrVaches){
                for(int i = vaches.Count; i < newLevel.nbrVaches; i++){
                    var go = Instantiate(vachePrefab,Vector3.zero, Quaternion.identity);
                    vaches.Add(go.GetComponent<VacheScript>());
                }
            }
            for(int i = 0; i < vaches.Count; i++){
                vaches[i].Reset(levels[currentLevel].VacheSpawn[i]);
            }
        }
    }
    private IEnumerator Fade()
    {
        animation.SetBool("fade", true);
        yield return new WaitForSecondsRealtime(1);
        animation.SetBool("fade",false);
        LoadNextLevel();
    }
}
