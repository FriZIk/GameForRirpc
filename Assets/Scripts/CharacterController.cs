using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterController: NetworkBehaviour 
{
    public Camera camera;
    public float Speed;
    public AudioClip IdleSound;
    AudioSource audio;
    
    void Awake()
    {
        camera.enabled = false;
    }

    public override void OnStartLocalPlayer()
    {
        camera.enabled = true;
    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(IdleSound);
    }

    void FixedUpdate()
    {
        if(this.isLocalPlayer)
        {
            if(Input.GetKey(KeyCode.W))
            {
                transform.position = transform.position + Vector3.forward * Speed;
                Debug.Log("Вперёд!");
            }
            if( Input.GetKey(KeyCode.A))
            {
                Debug.Log("Влево!"); 
                this.transform.position = transform.position + Vector3.left * Speed;
            }
            if( Input.GetKey(KeyCode.S))
            {
                Debug.Log("Назад!"); 
                transform.position = transform.position + Vector3.back * Speed;
            }
            if( Input.GetKey(KeyCode.D))
            {
                Debug.Log("Вправо!");
                transform.position = transform.position + Vector3.right * Speed;
            }
        }
    }
}
