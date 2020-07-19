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


// var i:integer;

// (* функция определеяет положение точки относительно вектора               *)
// Function WherePoint(ax,ay,bx,by,px,py:real):integer;
// var s :real;
// begin
//     s:=(bx-ax)*(py-ay)-(by-ay)*(px-ax);
//     if s > 0 then WherePoint:=1
//     else if s < 0 then WherePoint:=-1
//     else WherePoint:=0;
// end;

// Begin (* Тело основной программы *)
//    i:=WherePoint(1,1,8,8,2,5);
//    if i > 0 then writeln('точка слева от вектора')
//    else if i < 0 then writeln('точка справа от вектора')
//    else writeln('на векторе, прямо по вектору или сзади вектора');
// End.