using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainMenu : MonoBehaviour
{
    public int NumberOfWindow;

    // Дополнительные меню для каждого из понпунктов основного
    public GameObject CreateHostMenu,CreateHostWithClientMenu,ConnectToServerMenu; 
    // Основное меню
    public GameObject MainMenuObject;
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
    
    // Функция выхода из меню подключения к хосту
    public void BackButtonClient()
    {
        ResetMain(false,ConnectToServerMenu);
    }

    // Функция для отображения меню подключения к хосту
    public void StartClientMenu()
    {
        ResetMain(true,ConnectToServerMenu);
    }

    // Функция выхода из приложения
    public void GoToWindows()
    {
        Application.Quit();
    }
}
