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
    public int NumberOfVegetables;
    
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

    bool CheckInsidePolygon(float X_Coordinate,float Y_Coordinate)
    {
        float X = X_Coordinate + 1,Y = Y_Coordinate; // Дополнительные точки для построения уравнения прямых
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
            if(p.z > Ymax) Ymax = p.z;
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
                Debug.Log("Точка пересечения:" + x + " " + y);

                int j;
                float VerPointX = 0f,VerPointY = 0f;
                for(j = 0;j < NumberOfVegetables;j++)
                {
                    if(x == Vegetables[j].x && y == Vegetables[j].z)
                    {
                        Debug.Log("Вершина!!!");
                        VerPointX = Vegetables[j].x;
                        VerPointY = Vegetables[j].z;
                        break;
                    }
                }
                Debug.Log("Найдено совпадений " + j + " из " + NumberOfVegetables);
                // Необходимо ограничить кооридинату для того чтобы прямые были так же лучами и шли только вперёд
                // x - координата точки пересечения
                // X_Coordinate - начальная точка для которой мы определяем пересекает ли луч проведённый из неё прямую
                // Vegetables[i].x - начальная точка для отрезка вершины
                // Возможно стоит использвоать другой способ зарисовки многоугольника, например прямой по точкам пересечения

                if(j != NumberOfVegetables && x > Xmin && y > Ymin && X_Coordinate < VerPointX) return true;
                else if(x <= Xmax && x > Xmin && y > Ymin && y < Ymax && x > X_Coordinate) // Прямая вправо
                {
                    Debug.Log("Точка удовлетворяет условию!");
                    CounterOfIntersects++; // Если число пересечений чётно, то точка снаружи, если нечётно, то внутри
                }
            }
        }

        Debug.Log("Количетсво пересечений:" + CounterOfIntersects);
        // Финальное сравнение
        if(CounterOfIntersects % 2 != 0) return true;
        else return false;
    }



    // Пробуем сделать с помощью прямой
    public void GeneratorTest()
    {
         // Находим максимальную вершину
        float Xmin= 100,Xmax = 0,Ymin = 100,Ymax = 0;
        foreach(Vector3 p in Vegetables)
        {
            if(p.x < Xmin) Xmin = p.x;
            if(p.x > Xmax) Xmax = p.x;
            if(p.z < Ymin) Ymin = p.z;
            if(p.z > Ymax) Ymax = p.z;
        }

        // Список ззранящий значения всех точек пересечения
        List <Vector3> PointsOfIntersection = new List<Vector3>();
        
        // Точки прямой пересекающей стороны
        float X = Xmin;
        float X1 = Xmax;
        float Y,Y1;

        int CounterOfIntersects = 0; // Счётчик количества точек пересечения 
        // Проходимся по всему массиву и находим все точки пересечения точек сдвигая прямую по оси Y(Z)
        for(int i = (int)Ymax - 1;i > (int)Ymin;i--)
        {
            Y = i;Y1 = Y;
            Debug.Log("Строим прямую с началов в точке: x=" + X + " y=" + Y);
            // Точка X не меняется, изменяется только Y
            float A1, B1, C1;// Коэфициенты для уравнения прямой пересекающей стороны
            A1 = Y - Y1;
            B1 = X1 - X;
            C1 = X * Y1 - X1 * Y;

            // Находим точки пересечения этйо прямой и всех сторон
            for(int j = 0;j < NumberOfVegetables;j++)
            {
                // Высчитывание коэфициентов для сторон многоугольника
                float A2, B2, C2;
                if(j == NumberOfVegetables - 1)
                {
                    A2 = Vegetables[j].z - Vegetables[0].z;
                    B2 = Vegetables[0].x - Vegetables[j].x;
                    C2 = Vegetables[j].x * Vegetables[0].z - Vegetables[0].x * Vegetables[j].z;
                }
                else
                {
                    A2 = Vegetables[j].z - Vegetables[j + 1].z;
                    B2 = Vegetables[j + 1].x - Vegetables[j].x;
                    C2 = Vegetables[j].x * Vegetables[j + 1].z - Vegetables[j + 1].x * Vegetables[j].z;
                } 

                if(IsParall(A1,A2,B1,B2) == 1)
                {
                    Intersect(A1,A2,B1,B2,C1,C2); // Получаем точку пересечения
                    if(x >= Xmin && x <= Xmax)
                    {
                        CounterOfIntersects++;
                        PointsOfIntersection.Add(new Vector3(x,0.5f,y)); // Добавляем точку в список всех точек пересечения
                        Debug.Log("Точка пересечения с прямой " + (j + 1) + ": x=" + x + " y=" +y);
                    }
                }
            }
        }
        Debug.Log("Количество пересечений:" + CounterOfIntersects);

        // Выведем для дебага что у нас хрониться внутри
        foreach(Vector3 p in PointsOfIntersection)
        {
            Debug.Log(p.x + " " + p.z);
        }

        // В найденные промежутки между координатами, которые хранятся в списке, отрисовываем прямые
        for(int i = 0;i < CounterOfIntersects - 1;i++)
        {
            float FinishX = PointsOfIntersection[i].x;
            float StartX = PointsOfIntersection[i + 1].x;
            float ConstY = PointsOfIntersection[i].z;
            Debug.Log("Стартовая координата " + StartX + " " + ConstY + ", финишная координата " + FinishX + " " +ConstY);

            for(int j = (int)StartX+1;j <= (int)FinishX;j++)
            {
                if(CheckCoordinate(j,ConstY,Vegetables) == true)
                {
                    if(isRandom == true)
                    {
                        Debug.Log(StartX-FinishX);
                        Instantiate(TreePrototype, new Vector3(j, 0.5f, ConstY), Quaternion.identity);
                        Vegetables.Add(new Vector3(j,0.5f,ConstY));      
                    }
                    else
                    {
                        int HowIsiT = Mathf.RoundToInt(Random.Range(0,100));
                        if(HowIsiT > 50)
                        {
                            Instantiate(TreePrototype, new Vector3(j, 0.5f, ConstY), Quaternion.identity);
                            Vegetables.Add(new Vector3(j,0.5f,ConstY));  
                        }
                    }
                }
            }
                // Тут происходит какой-то неадекват, переписать и проверить ещё раз
        }
    }

    // Функция построения леса по жёстким координатам
    void MakeForest(float Xmax,float Xmin,float Ymax, float Ymin)
    {
        for(int i = (int)Xmin + 1;i < (int)Xmax;i += Step)
        {
            for(int j = (int)Ymin;j < (int)Ymax;j += Step)
            {
                Debug.Log("Обрабатываем точка: " + i + " " + j);
                if(CheckInsidePolygon(i,j) == true)
                {
                    Debug.Log("Точка внутри полигона!");
                    if(CheckCoordinate(i,j,Vegetables) == true)
                    {
                        Instantiate(TreePrototype, new Vector3(i, 0.5f, j), Quaternion.identity);       
                        Vegetables.Add(new Vector3(i,0.5f,j));
                    }
                }
                else
                {
                    Debug.Log("Точка снаружи полигона!");
                }
            }
        }
    }

    // Функция построения леса на основе случайных координат
    void MakeRandomForest(float Xmax,float Xmin,float Ymax, float Ymin)
    {
        for(int i = 0; i < CountOfRandomTrees; i++)
        {
            float X_Coordinate = Mathf.RoundToInt(Random.Range(Xmin + 1,Xmax - 1));
            float Y_Coordinate = Mathf.RoundToInt(Random.Range(Ymin + 1,Ymax - 1));
            if(CheckInsidePolygon(X_Coordinate,Y_Coordinate) == true);
            {
                if(CheckCoordinate(X_Coordinate,Y_Coordinate,Vegetables) == true)
                {
                    Instantiate(TreePrototype, new Vector3(X_Coordinate, 0.5f, Y_Coordinate), Quaternion.identity); 
                    Vegetables.Add(new Vector3(X_Coordinate,0.5f,Y_Coordinate));
                }
            }
        }
    }

    public void GenerateForest()
    {
        // Для генерации деревьев необходимо ограничить территорию
        // Найдём "крайние вершины" на основе их будем генерировать координаты
        float Xmin= 100,Xmax = 0,Ymin = 100,Ymax = 0;

        // Находим максимальную вершину 
        foreach(Vector3 p in Vegetables)
        {
            if(p.x < Xmin) Xmin = p.x;
            if(p.x > Xmax) Xmax = p.x;
            if(p.z < Ymin) Ymin = p.z;
            if(p.z > Ymax) Ymax = p.z;
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


        GeneratorTest();
        // Вызываем функцию построения деревьев в полигоне
        // if(isRandom == true)
        // {
        //     MakeRandomForest(Xmax,Xmin,Ymax,Ymin);
        // }
        // else
        // {
        //     GeneratorTest();
        //     //MakeForest(Xmax,Xmin,Ymax,Ymin);
        // }
    }
}