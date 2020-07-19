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
            Instantiate(Vegetable, new Vector3(X, 0.5f, Z), Quaternion.identity);
        }

        // Конструктор класса
        public Point(float X_Coordinate,float Z_Coordinate)
        {
            X = X_Coordinate;
            Z = Z_Coordinate;
        }
    }

    [Header("Доступные виды растений")]
    public GameObject Vertex; // Вершина
    public GameObject TreePrototype; // Прототип какого-то дерева
    public GameObject Wall; // Чем отрисовывать контур полигона

    [Header("Настройки генератора")]

    // Строим ли вершины?
    [Tooltip("Нужно ли строить отдельно вершины?")]
    public bool ExistVertex;

    // Плотность заполнения 
    [Tooltip("Параметр заполненности полигона, не используется в случае случайной генерации координат, используется при построении контура!")]
    public int Step;
    
    // Случайный ли координаты?
    [Tooltip("Параметр рандомизации координат")]
    public bool isRandom;
    
    // Если используется рандом, кол-во сгенерированных деревьев
    [Tooltip("Требуемое кол-во деревьев, используется только при случайной генерации координат!")]
    public int CountOfRandomTrees;

    // Строим ли контур?
    [Tooltip("Нужно ли строить контур полигона, шаг контура совпадает с шагом генерации (Step)")]
    public bool ExistCircuit;


    // Список вершин
    public List <Point> Vegetables = new List<Point>();
    // Количество вершин в полигоне
    int NumberOfVegetables = 5;
    
    // Функция проверки, есть ли такая координата в списке, требуется для исключения дублей
    bool CheckCoordinate(float X,float Y)
    {
        foreach (Point p in Vegetables)
        {
            if (p.X == X && p.Z == Y)
            {
                return false;
            }
        }
        return true;
    }

    // Алгоритм Брезенхэма
    void MakeCircuit(int NumberOfVegetables)
    {
        for(int i = 0;i < NumberOfVegetables;i++)
        {
            int counter = 0; // Счётчик, чтобы не отрисовывать вершины 
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
                if(counter != 0)
                {
                    Vegetables.Add(new Point(x1,y1));
                    Instantiate(Wall, new Vector3(x1, 0.5f, y1), Quaternion.identity);
                    Debug.Log(x1 + " " + y1);
                }
                else counter++;
                
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
        }
        return (int)Prod;
    }

    // Функция построения леса по жёстким координатам
    void MakeForest(float Xmax,float Xmin,float Ymax, float Ymin)
    {
        for(int i = (int)Xmin;i < (int)Xmax;i += Step)
        {
            for(int j = (int)Ymin;j < (int)Ymax;j += Step)
            {
                int Triger = CheckInsidePolygon(i,j);
                if(Triger == -1)
                {
                    if(CheckCoordinate(i,j) == true)
                    {
                        Instantiate(TreePrototype, new Vector3(i, 0.5f, j), Quaternion.identity);       
                        Vegetables.Add(new Point(i,j));
                    }
                }
            }
        }
    }

    // Функция построения леса на основе случайных координат
    void MakeRandomForest(float Xmax,float Xmin,float Ymax, float Ymin)
    {
        for(int i = 0; i < CountOfRandomTrees; i++)
        {
            float X_Coordinate = Mathf.RoundToInt(Random.Range(Xmin,Xmax));
            float Y_Coordinate = Mathf.RoundToInt(Random.Range(Ymin,Ymax));
            int Triger = CheckInsidePolygon(X_Coordinate,Y_Coordinate);
            if(Triger == -1)
            {
                if(CheckCoordinate(X_Coordinate,Y_Coordinate) == true)
                {
                    Instantiate(TreePrototype, new Vector3(X_Coordinate, 0.5f, Y_Coordinate), Quaternion.identity); 
                    Vegetables.Add(new Point(X_Coordinate,Y_Coordinate));
                }
            }
        }
    }

    void Start()
    {
        // Задаем тестовый список координат (выпуклый многоугольник), временно!
        Vegetables.Add(new Point(4,7));
        Vegetables.Add(new Point(10,10));
        Vegetables.Add(new Point(15,4));
        Vegetables.Add(new Point(11,1));
        Vegetables.Add(new Point(6,1));

        // Для генерации деревьев необходимо ограничить территорию
        // Найдём "крайние вершины" на основе их будем генерировать координаты
        float Xmin= 100,Xmax = 0,Ymin = 100,Ymax = 0;

        // Найдём длину ребра по X, может быть понадобится в будущем
        float LengthX = Xmax - Xmin;
        float LengthY = Ymax - Ymin;

        // Находим максимальную вершину 
        foreach(Point p in Vegetables)
        {
            if(p.X < Xmin) Xmin = p.X;
            if(p.X > Xmax) Xmax = p.X;
            if(p.Z < Ymin) Ymin = p.Z;
            if(p.Z > Ymin) Ymax = p.Z;
        }

        // Строим вершины
        if(ExistVertex == true)
        {
            foreach(Point p in Vegetables)
            {
                p.GeneratePolygon(Vertex);
            }
        }

        // С помощью алгоритма Брезенхэма строим контур, если требуется (пока что степ не написан)
        if(ExistCircuit == true)
        {
            MakeCircuit(NumberOfVegetables);
        }

        // Вызываем функцию построения деревьев в полигоне
        if(isRandom == true)
        {
            MakeRandomForest(Xmax,Xmin,Ymax,Ymin);
        }
        else
        {
            MakeForest(Xmax,Xmin,Ymax,Ymin);
        }
    }
}