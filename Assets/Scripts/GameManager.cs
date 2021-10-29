using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private List<CowScript> _vaches = new List<CowScript>();
    [SerializeField]
    private DogScript _dog;
    private int _currentLevel;
    public static GameManager Instance;
    public bool isPlaying { get; private set; } = true;
    [SerializeField]
    private List<LevelObject> _levels;
    private GameObject _currentLoadedLevel;
    [SerializeField]
    private GameObject _vachePrefab;
    public Animator Animation;
    public Animator TitleAnim;
    [SerializeField]
    private GameObject _sign;
    [SerializeField]
    private TextMesh _nbVaches;
    private float _nbScore = 0;
    [SerializeField]
    private Camera _mainCam;
    [SerializeField]
    private GameObject _gameOverCanvas;
    [SerializeField]
    private GameObject _objectiveText;
    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } 
        else {
            Destroy(this);
        }
        _currentLevel = 0;
        LoadNextLevel();
        StopGame();
    }
    public void VacheOutOfBound(){
        StopGame();
    }

    public void CheckWinLevel(){
        bool won = true;
        _nbScore +=1; 
        GameObject confetti = GameObject.FindGameObjectWithTag("Confetti");
        confetti.GetComponent<ParticleSystem>().Play();
        confetti.GetComponent<AudioSource>().Play();
        _nbVaches.text = _nbScore.ToString() + "/" +_vaches.Count;
        foreach(CowScript vache in _vaches){
            if(!vache.IsDone){
                won = false;
            }
        }
        if(won){
            _currentLevel++;
            foreach(CowScript vache in _vaches){
                vache.Direction = Vector3.zero;
            }
            if(_currentLevel < _levels.Count){
                StartCoroutine("Fade");
            }else{
                StartCoroutine(EndCoroutine());
            }
        }

    }

    private void StopGame(){
        isPlaying = false;
    }
    private void RestartGame(){
        isPlaying = true;
    }
    public void StartGame(){
        RestartGame();
        _objectiveText.SetActive(true);
    }
    private void LoadNextLevel(){
        Destroy(_currentLoadedLevel);
        LevelObject newLevel = _levels[_currentLevel];
        _currentLoadedLevel = Instantiate(newLevel.level);
        _dog.transform.position = newLevel.DogSpawn;
        if(_vaches.Count > newLevel.nbrVaches){
            for(int i = _vaches.Count - 1; i >= newLevel.nbrVaches;i--){
                Destroy(_vaches[i].gameObject);
                _vaches.RemoveAt(i);
            }
        }else if (_vaches.Count < newLevel.nbrVaches){
            for(int i = _vaches.Count; i < newLevel.nbrVaches; i++){
                var go = Instantiate(_vachePrefab,Vector3.zero, Quaternion.identity);
                _vaches.Add(go.GetComponent<CowScript>());
            }
        }
        for(int i = 0; i < _vaches.Count; i++){
            _vaches[i].Reset(_levels[_currentLevel].VacheSpawn[i]);
        }
        PlaceSign(newLevel.signPos);

    }

    private void PlaceSign(Vector3 signPos){
        _sign.transform.position = signPos;
        _nbScore = 0;
        _nbVaches.text = _nbScore.ToString() + "/" +_vaches.Count;
    }
    private IEnumerator Fade()
    {
        StopGame();
        yield return new WaitForSecondsRealtime(1.5f);
        Animation.SetBool("fade", true);
        yield return new WaitForSecondsRealtime(1);
        Animation.SetBool("fade",false);
        LoadNextLevel();
        RestartGame();
    }

    private IEnumerator EndCoroutine()
    {
        StopGame();
        _objectiveText.SetActive(false);
        yield return new WaitForSecondsRealtime(1.5f);
        Animation.SetBool("fade", true);
        yield return new WaitForSecondsRealtime(1);
        _gameOverCanvas.SetActive(true);
    }
}
