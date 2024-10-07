using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)]
    private float musicLayeringTimer = 0f;
    [SerializeField]
    private int musicLayeringThreshold = 10;
    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference gameplayEvent;

    // Singleton instance
    public static MusicManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        instance.setParameterByName("Low Pass", 0f);
        
        // setting up FMOD event for music playback
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
            return;
        }

        instance = FMODUnity.RuntimeManager.CreateInstance(gameplayEvent);
        instance.start();
        
        GameObject LevelManager = GameObject.Find("LevelManager");
        //Debug.Log("among us");
    }

    // Update is called once per frame
    void Update()
    {
        musicLayeringTimer += Time.deltaTime;
        if (musicLayeringTimer >= musicLayeringThreshold) {
            instance.setParameterByName("Intensity", 1f);
        }

        if (LevelManager.isGameOver == true) {
            instance.setParameterByName("Intensity", 0f);
        }
    }

    public void OnGameStart() {
        instance.setParameterByName("Low Pass", 1f);
    }

    public void OnGameOver() {
        instance.setParameterByName("Low Pass", 1f);
    }
}
