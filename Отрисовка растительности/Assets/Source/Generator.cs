using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    // Класс для координат
    public class Point
    {
        public float X,Z;
        private float Y  = 0.5f;

        public void GenerateTerrain(GameObject Vegetable)
        {   
            Instantiate(Vegetable, new Vector3(X, Y, Z), Quaternion.identity);
        }

        // Конструктор класса
        public Point(float X_Coordinate,float Z_Coordinate)
        {
            X = X_Coordinate;
            Z = Z_Coordinate;
        }
    }

    // Необходимые префабы для генерации
    public GameObject Tree_1,Tree_2,Bush,Grass;
    public List <Point> Vegetables = new List<Point>();

    void Start()
    {
        int NumberOfVegetables = 50;
        for(int i = 0;i < NumberOfVegetables;i++)
        {
            Vegetables.Add(new Point(Random.Range(1,100),Random.Range(1,70)));
        }


        // Генерация карты
        foreach(Point p in Vegetables)
        {
            int KindOfVegetables = Random.Range(1,4);
            switch(KindOfVegetables)
            {
                case 1:p.GenerateTerrain(Tree_1);break;
                case 2:p.GenerateTerrain(Tree_2);break;
                case 3:p.GenerateTerrain(Bush);break;
                case 4:p.GenerateTerrain(Grass);break;
            }
        }
    }
}
