using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public RectTransform sign, signCanvas;
    public Text NbVaches;
    private float NbScore = 0;
    [SerializeField]
    private Camera mainCam;

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
        NbScore +=1; 
        GameObject confetti = GameObject.FindGameObjectWithTag("Confetti");
        confetti.GetComponent<ParticleSystem>().Play();
        confetti.GetComponent<AudioSource>().Play();
        NbVaches.text = NbScore.ToString() + "/" +vaches.Count;
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
            PlaceSign(newLevel.signPos);
            NbScore = 0;
            NbVaches.text = NbScore.ToString() + "/" +vaches.Count;
        }

    }

    private void PlaceSign(Vector3 signPos){
 
  
        Vector2 ViewportPosition=mainCam.WorldToViewportPoint(signPos);
        Vector2 WorldObject_ScreenPosition=new Vector2(
        ((ViewportPosition.x*signCanvas.sizeDelta.x)-(signCanvas.sizeDelta.x*0.5f)),
        ((ViewportPosition.y*signCanvas.sizeDelta.y)-(signCanvas.sizeDelta.y*0.5f)));

        sign.anchoredPosition=WorldObject_ScreenPosition;
    }
    private IEnumerator Fade()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        animation.SetBool("fade", true);
        yield return new WaitForSecondsRealtime(1);
        animation.SetBool("fade",false);
        LoadNextLevel();
    }
}
