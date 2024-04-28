using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scipts
{
    public class SceneTransitionHandler : NetworkBehaviour
    {
        public static SceneTransitionHandler Instance { get; internal set; }
        
        [HideInInspector]
        public delegate void OnSceneStateChangedDelegateHandler(SceneState newState);
        [HideInInspector]
        public event OnSceneStateChangedDelegateHandler OnSceneStateChanged;
        
        [HideInInspector]
        public delegate void OnClientLoadedSceneDelegateHandler(ulong clientId);
        [HideInInspector]
        public event OnClientLoadedSceneDelegateHandler OnClientLoadedScene;
        
        [SerializeField] private string MainMenuScene = "StartMenu";
        
        private SceneState sceneState;
        private int loadedClients;
        
        public enum SceneState
        {
            Init,
            Start,
            Lobby,
            InGame
        }

        public SceneState GetCurrentSceneState()
        {
            return sceneState;
        }

        public void SwitchScene(string sceneName)
        {
            if (NetworkManager.Singleton.IsListening)
            {
                loadedClients = 0;
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneName);
            }
        }

        public void ExitToStartMenu()
        {
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null)
            {
                NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
            }

            OnClientLoadedScene = null;
            SetSceneState(SceneState.Start);
            SceneManager.LoadScene(0);
        }
        
        public bool AllClientsLoaded()
        {
            return loadedClients == NetworkManager.Singleton.ConnectedClients.Count;
        }
        
        public void RegisterCallbacks()
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
        }
        
        private void Awake()
        {
            if (Instance != this && Instance != null && Instance.IsOwner)
            {
                Destroy(Instance.gameObject);
            }
            
            Instance = this;
            SetSceneState(SceneState.Init);
            DontDestroyOnLoad(this);
        }

        private void SetSceneState(SceneState state)
        {
            sceneState = state;
            OnSceneStateChanged?.Invoke(sceneState);
        }

        private void Start()
        {
            if (sceneState == SceneState.Init)
            {
                SceneManager.LoadScene(MainMenuScene);
            }
        }

        private void OnLoadComplete(ulong clientid, string scenename, LoadSceneMode loadscenemode)
        {
            loadedClients++;
            OnClientLoadedScene?.Invoke(clientid);
        }
    }
}
