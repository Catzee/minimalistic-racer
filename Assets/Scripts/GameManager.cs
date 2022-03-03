using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    //Game State
    private static int gameState = 1;
    public const int GAME_STATE_RACING = 0;
    public const int GAME_STATE_PAUSED = 1;

    //References
    private static GameManager instance;

    //Track Management
    private GameObject currentTrack = null;
    private int currentTrackIndex = -1;
    private AsyncOperationHandle<GameObject> trackLoadHandle;
    private bool trackIsLoading = false;
    public Transform spawnPosition;

    //Car Management
    public CarPhysics carPhysics;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnloadCurrentTrack();
        }
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public static int GetGameState()
    {
        return gameState;
    }

    private void SetGameState(int newState)
    {
        gameState = newState;
        if(gameState == GAME_STATE_PAUSED)
        {
            carPhysics.ResetCar();
        }
    }

    private void UnloadCurrentTrack()
    {
        currentTrackIndex = -1;
        if (currentTrack != null)
        {
            Destroy(currentTrack);
            Addressables.Release(trackLoadHandle);
        }
    }

    public void LoadTrack(int trackIndex)
    {
        if (!trackIsLoading)
        {
            SetGameState(GAME_STATE_PAUSED);
            UnloadCurrentTrack();
            trackIsLoading = true;
            currentTrackIndex = trackIndex;
            string path = "Track" + trackIndex.ToString();
            trackLoadHandle = Addressables.LoadAssetAsync<GameObject>(path);
            trackLoadHandle.Completed += LoadTrackComplete;
        }
    }

    private void LoadTrackComplete(AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            InstantiateTrack(operation.Result);
            RestartRace();
        }
        else
        {
            Debug.LogError("Track failed loading!");
        }
        trackIsLoading = false;
    }

    private void InstantiateTrack(GameObject track)
    {
        currentTrack = Instantiate(track, Vector3.zero, Quaternion.identity);
        CheckpointManager.trackCheckpoints = currentTrack.GetComponent<TrackCheckpoints>().checkpoints;
    }

    private void RestartRace()
    {
        SetGameState(GAME_STATE_RACING); //continue racing after track loading
        carPhysics.transform.position = spawnPosition.position;
        UIManager.SetHighscoreValue(HighscoreManager.GetHighscore(currentTrackIndex));
    }

    public static int GetCurrentTrackIndex()
    {
        return instance.currentTrackIndex;
    }
}
