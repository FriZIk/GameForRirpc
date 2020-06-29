using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Brick;//Для префаба обычной стены 

    void Start()
    {
        int CountOfWalls = 10;
        int NumberDimension = 2;
        int[,] RandomArray = new int[NumberDimension, CountOfWalls];   

        for(int i = 0;i < NumberDimension;i++)
            for(int j = 0;j < CountOfWalls;j++)
                RandomArray[i,j] = Random.Range(1,5);

         Debug.Log("Случайный массив координат:");
        for(int i = 0;i < NumberDimension;i++)
            for(int j = 0;j < CountOfWalls;j++)
                Debug.Log(RandomArray[i,j]);


        for (int i = 0;i < NumberDimension;i++) 
        {
            for (int j = 0;j < CountOfWalls;j++) 
            {
                Instantiate(Brick, new Vector3(RandomArray[0,j], RandomArray[1,j], 0), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
