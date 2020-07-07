using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator1 : MonoBehaviour
{
     // Класс для координат
    public class Point
    {
        public float X,Z;
        private float Y  = 0.5f;

        // Функция для построения вершин
        public void GeneratePolygon(GameObject Vegetable)
        {   
            Instantiate(Vegetable, new Vector3(X, 2f, Z), Quaternion.identity);
        }

        // Конструктор класса
        public Point(float X_Coordinate,float Z_Coordinate)
        {
            X = X_Coordinate;
            Z = Z_Coordinate;
        }
    }

    public GameObject Vertex,TreePrototype,Circuit,Wall;

    public List <Point> Vegetables = new List<Point>();
    // Количество вершин в полигоне
    int NumberOfVegetables = 5;

    // Алгоритм Брезенхэма
    void MakeCircuit(int NumberOfVegetables)
    {
        for(int i = 0;i < NumberOfVegetables;i++)
        {
            float x1,y1,x2,y2; // Переменные для удобной обработки координат вершин

            x1 = Vegetables[i].X;
            y1 = Vegetables[i].Z;

            if(i == NumberOfVegetables - 1)
            {
                x2 = Vegetables[0].X;
                y2 = Vegetables[0].Z;
            }
            else
            {
                x2 = Vegetables[i+1].X;
                y2 = Vegetables[i+1].Z;
            }
            float deltaX = Mathf.Abs(x2 - x1);
            float deltaY = Mathf.Abs(y2 - y1);
            float signX = x1 < x2 ? 1 : -1;
            float signY = y1 < y2 ? 1 : -1;
        
            float error = deltaX - deltaY;
        
            while(x1 != x2 || y1 != y2) 
            {
                Instantiate(Wall, new Vector3(x1, 1, y1), Quaternion.identity);
                float error2 = error * 2;
                
                if(error2 > -deltaY) 
                {
                    error -= deltaY;
                    x1 += signX;
                }
                if(error2 < deltaX) 
                {
                    error += deltaX;
                    y1 += signY;
                }
            }
        }
    }

    // Находим взаименое расположение точки и всех сторон многоугольника
    int CheckInsidePolygon(float X_Coordinate,float Y_Coordinate)
    {
        float Prod = 0;
        for(int i = 0;i < NumberOfVegetables;i++)
        {
            // Условие, чтобы зациклится на начальный элемент
            if(i == NumberOfVegetables - 1)
            {
                Prod = (Vegetables[0].X - Vegetables[i].X)*(Y_Coordinate - Vegetables[i].Z)-(Vegetables[0].Z - Vegetables[i].Z)*(X_Coordinate - Vegetables[i].X);
            }
            else
            {
                Prod = (Vegetables[i + 1].X - Vegetables[i].X)*(Y_Coordinate - Vegetables[i].Z)-(Vegetables[i + 1].Z - Vegetables[i].Z)*(X_Coordinate - Vegetables[i].X);
            }
            Debug.Log("Значение произведения:" + Prod);
            // Узнаем с какой стороны
            if(Prod > 0)
            {
                Prod = 1; // Справа
                break;
            } 
            else if(Prod < 0) Prod = -1; // Слева
            else if(Prod == 0) Prod =  0; // Лежит на стороне
            Debug.Log(Prod);
        }
        Debug.Log(Prod);
        return (int)Prod;
    }

    void Start()
    {
        // Задаем тестовый список координат (выпуклый многоугольник)
        Vegetables.Add(new Point(4,7));
        Vegetables.Add(new Point(10,10));
        Vegetables.Add(new Point(15,4));
        Vegetables.Add(new Point(11,1));
        Vegetables.Add(new Point(6,1));
        
        // Строим вершины
        foreach(Point p in Vegetables)
        {
            p.GeneratePolygon(Vertex);
        }

        // С помощью алгоритма Брезенхэма строим контур
        MakeCircuit(NumberOfVegetables);

        // Для генерации случайных деревьев необходимо ограничить территорию для рандома
        // Найдём "крайние вершины" на основе их будем генерировать случайные координаты
        float Xmin= 100,Xmax = 0,Ymin = 100,Ymax = 0;

        // Находим максимальную вершину 
        foreach(Point p in Vegetables)
        {
            if(p.X < Xmin) Xmin = p.X;
            if(p.X > Xmax) Xmax = p.X;
            if(p.Z < Ymin) Ymin = p.Z;
            if(p.Z > Ymin) Ymax = p.Z;
        }

        // Найдём длину ребра по X, может быть понадобится 
        float LengthX = Xmax - Xmin;
        float LengthY = Ymax - Ymin;
        
        // Строим случайный лес :)
        int CountOfRandomTrees = 20;

        for(int i = 0; i < CountOfRandomTrees; i++)
        {
            float X_Coordinate = Mathf.RoundToInt(Random.Range(Xmin,Xmax));
            float Y_Coordinate = Mathf.RoundToInt(Random.Range(Ymin,Ymax));
            int Triger = CheckInsidePolygon(X_Coordinate,Y_Coordinate);
            if(Triger == -1 || Triger == 0)
            {
                Instantiate(TreePrototype, new Vector3(X_Coordinate, 1f, Y_Coordinate), Quaternion.identity);       
            }
        }
    }
}