using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float Speed;

    public GameObject Menu;
    bool Triger = false;

    public void ContinueFu()
    {
        Menu.SetActive(false);
    }

    public void ExitToWindows()
    {
        Application.Quit();
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + Vector3.forward * Speed;
            Debug.Log("Вперёд!");
        }
        if(Input.GetKey(KeyCode.A))
        {
            Debug.Log("Влево!"); 
            this.transform.position = transform.position + Vector3.left * Speed;
        }
        if(Input.GetKey(KeyCode.S))
        {
            Debug.Log("Назад!"); 
            transform.position = transform.position + Vector3.back * Speed;
        }
        if(Input.GetKey(KeyCode.D))
        {
            Debug.Log("Вправо!");
            transform.position = transform.position + Vector3.right * Speed;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Пауза");
            if(Triger == true)
            {
                Menu.gameObject.SetActive(false); 
                Triger = false;
            }
            else
            { 
                Menu.gameObject.SetActive(true); 
                Triger = true;
            }
        }
    }
}
