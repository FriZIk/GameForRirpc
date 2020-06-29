using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [HelpURL("https://mirror-networking.com/docs/Components/NetworkManagerHUD.html")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        NetworkManager manager;
        // Дополнительные меню для каждого из понпунктов основного
        GameObject CreateHostWithClientMenu,ConnectToServerMenu;
        // Основное меню
        GameObject MainMenuObject;
        public GameObject PauseMenuObject;
        public InputField IpAddressField;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
            CreateHostWithClientMenu = GameObject.Find("HostAndClient");
            ConnectToServerMenu = GameObject.Find("ConnectToHostMenu");
            MainMenuObject = GameObject.Find("Main");
        }

         // Функция инвертирует значения поочерёдно включая и отключая элементы интерфейса
        void ResetMain(bool triger,GameObject Obj)
        {
            switch(triger)
            {
                case true: 
                    MainMenuObject.gameObject.SetActive(false); 
                    Obj.gameObject.SetActive(true);
                break;
                case false: 
                    Obj.gameObject.SetActive(false);
                    MainMenuObject.gameObject.SetActive(true); 
                break;
            }
        }

        void OnGUI()
        {
            if (NetworkClient.isConnected && !ClientScene.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    ClientScene.Ready(NetworkClient.connection);

                    if (ClientScene.localPlayer == null)
                    {
                        ClientScene.AddPlayer(NetworkClient.connection);
                    }
                }
            }
        }

        // Функция для создания хоста и клиента
        public void  CreateHostAndClient()
        {
            //ResetMain(true,CreateHostWithClientMenu);
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    manager.StartHost();
                }
            }
        }

        // Функция для создания только сервера
        public void StartOnlyServer()
        {
            if (!NetworkClient.active)
            {
                manager.StartServer();
            }
        }

        // Клиент без сервера
        public void ClientSettings()
        {
            manager.networkAddress = IpAddressField.text;
            Debug.Log(manager.networkAddress);
            manager.StartClient();
        }
        
        // Открытие дополнительного меню для ввода ip адреса хоста
        public void StartClientMenu()
        {
            ResetMain(true,ConnectToServerMenu);
            ClientSettings();
        }

        public void InGameMenuResume()
        {
            PauseMenuObject.SetActive(false);
        }

        public void InGameMenuExit()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                manager.StopHost();
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            { 
                manager.StopClient();
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                manager.StopServer();
            }
        }

        void FixedUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Пауза!!!");
                PauseMenuObject.gameObject.SetActive(!PauseMenuObject.gameObject.activeSelf);        
            }        
        }
    }
}
