using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
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

    // Список координат деревеьев внутри многоугольника
    public List <Vector3> VertexPoint = new List<Vector3>();
    // Список всех деревьев на карте
    public List <Vector3> Vegetables = new List<Vector3>();
    // Количество вершин в полигоне
    public int NumberOfVegetables = 6;
    // int NumberOfVegetables = 10;
    

    // Функция для построения вершин
    public void GeneratePolygon(GameObject Vegetable, Vector3 VertexCoordinate)
    {   
        Instantiate(Vegetable, VertexCoordinate, Quaternion.identity);
    }

    // Функция проверки, есть ли такая координата в списке, требуется для исключения дублей
    bool CheckCoordinate(float X,float Y, List <Vector3> Points)
    {
        foreach (Vector3 p in Points)
        {
            if (p.x == X && p.z == Y)
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

            x1 = Vegetables[i].x;
            y1 = Vegetables[i].z;

            if(i == NumberOfVegetables - 1)
            {
                x2 = Vegetables[0].x;
                y2 = Vegetables[0].z;
            }
            else
            {
                x2 = Vegetables[i+1].x;
                y2 = Vegetables[i+1].z;
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
                    Vegetables.Add(new Vector3(x1,0.5f,y1));
                    Instantiate(Wall, new Vector3(x1, 0.5f, y1), Quaternion.identity);
                    // Debug.Log(x1 + " " + y1);
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

    // Функция проверки на пересечение прямых
    int IsParall(float A1, float A2, float B1, float B2)
    {
        if(A1*B2 - A2*B1 != 0)return 1;
        else return 0;
    }

    float x,y; // Точки пересечения

    // Функция поиска точки пересечения если прямые не параллельны
    void Intersect(float a1, float a2, float b1, float b2, float c1, float c2)
    {
        float det = a1 * b2 - a2 * b1;
        x = (b1 * c2 - b2 * c1) / det;
        y = (a2 * c1 - a1 * c2) / det;
    }

    bool CheckInsidePolygonNV(float X_Coordinate,float Y_Coordinate)
    {
        float X = X_Coordinate + 3,Y = Y_Coordinate; // Дополнительные точки для построения уравнения прямых
        float A1, B1, C1;// Коэфициенты для уравнения прямой

        // Расчёт коэфициентов прямой для первой точки
        A1 = Y_Coordinate - Y;
        B1 = X - X_Coordinate;
        C1 = X_Coordinate * Y - X * Y_Coordinate;

         // Находим максимальную вершину
        float Xmin= 100,Xmax = 0,Ymin = 100,Ymax = 0;
        foreach(Vector3 p in Vegetables)
        {
            if(p.x < Xmin) Xmin = p.x;
            if(p.x > Xmax) Xmax = p.x;
            if(p.z < Ymin) Ymin = p.z;
            if(p.z > Ymin) Ymax = p.z;
        }

        // Проверка внтури ли точки?
        int CounterOfIntersects = 0;
        
        for(int i = 0;i < NumberOfVegetables;i++)
        {
            // Высчитывание коэфициентов для сторон многоугольника
            float A2, B2, C2;
            if(i == NumberOfVegetables - 1)
            {
                A2 = Vegetables[i].z - Vegetables[0].z;
                B2 = Vegetables[0].x - Vegetables[i].x;
                C2 = Vegetables[i].x * Vegetables[0].z - Vegetables[0].x * Vegetables[i].z;
            }
            else
            {
                A2 = Vegetables[i].z - Vegetables[i + 1].z;
                B2 = Vegetables[i + 1].x - Vegetables[i].x;
                C2 = Vegetables[i].x * Vegetables[i + 1].z - Vegetables[i + 1].x * Vegetables[i].z;
            } 

            if(IsParall(A1,A2,B1,B2) == 1)
            {
                Intersect(A1,A2,B1,B2,C1,C2); // Получаем точку пересечения

                int j;
                for(j = 0;j < NumberOfVegetables;j++)
                {
                    if(x == Vegetables[j].x && y == Vegetables[j].z)
                    {
                        Debug.Log("Вершина!!!");
                        break;
                    }
                }
                Debug.Log("Вот так" + j);

                if(j != NumberOfVegetables && x > X_Coordinate && X_Coordinate > Xmin && y > Ymin) return true;
                else if(x > X_Coordinate && x < Xmax && X_Coordinate > Xmin)
                {
                    CounterOfIntersects++; // Если число пересечений чётно, то точка снаружи, если нечётно, то внутри
                }
            }
        }

        Debug.Log("Количетсво пересечений:" + CounterOfIntersects);
        // Финальное сравнение
        if(CounterOfIntersects % 2 != 0) return true;
        else return false;
    }

    // Функция построения леса по жёстким координатам
    void MakeForest(float Xmax,float Xmin,float Ymax, float Ymin)
    {
        for(int i = (int)Xmin;i < (int)Xmax;i += Step)
        {
            for(int j = (int)Ymin;j < (int)Ymax;j += Step)
            {
                if(CheckInsidePolygonNV(i,j) == true)
                {
                    if(CheckCoordinate(i,j,Vegetables) == true)
                    {
                        Instantiate(TreePrototype, new Vector3(i, 0.5f, j), Quaternion.identity);       
                        Vegetables.Add(new Vector3(i,0.5f,j));
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
            if(CheckInsidePolygonNV(X_Coordinate,Y_Coordinate) == true);
            {
                if(CheckCoordinate(X_Coordinate,Y_Coordinate,Vegetables) == true)
                {
                    Instantiate(TreePrototype, new Vector3(X_Coordinate, 0.5f, Y_Coordinate), Quaternion.identity); 
                    Vegetables.Add(new Vector3(X_Coordinate,0.5f,Y_Coordinate));
                }
            }
        }
    }

    void Start()
    {
        // Задаем тестовый список координат (выпуклый многоугольник), временно!
        Vegetables.Add(new Vector3(4,0.5f,7));
        Vegetables.Add(new Vector3(7,0.5f,10));
        Vegetables.Add(new Vector3(10,0.5f,10));
        Vegetables.Add(new Vector3(15,0.5f,4));
        Vegetables.Add(new Vector3(11,0.5f,1));
        Vegetables.Add(new Vector3(6,0.5f,1));

        // Для генерации деревьев необходимо ограничить территорию
        // Найдём "крайние вершины" на основе их будем генерировать координаты
        float Xmin= 100,Xmax = 0,Ymin = 100,Ymax = 0;

        // Найдём длину ребра по X, может быть понадобится в будущем
        float LengthX = Xmax - Xmin;
        float LengthY = Ymax - Ymin;

        // Находим максимальную вершину 
        foreach(Vector3 p in Vegetables)
        {
            if(p.x < Xmin) Xmin = p.x;
            if(p.x > Xmax) Xmax = p.x;
            if(p.z < Ymin) Ymin = p.z;
            if(p.z > Ymin) Ymax = p.z;
        }

        // Строим вершины
        if(ExistVertex == true)
        {
            foreach(Vector3 p in Vegetables)
            {
                GeneratePolygon(Vertex,p);
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