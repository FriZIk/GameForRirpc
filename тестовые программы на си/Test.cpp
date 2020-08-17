#include <iostream>
 
using namespace std;

// Прототип координаты
struct Point
{
    double X,Z;
};
typedef struct Point Point;

// Функция проверки на пересечение прямых
int IsParall(double A1, double A2, double B1, double B2)
{
    if(A1*B2 - A2*B1 != 0)return 1;
    else return 0;
}

double x,y; // Точки пересечения

// Функция поиска точки пересечения если прямые не параллельны
void Intersect(double a1, double a2, double b1, double b2, double c1, double c2)
{
    double det = a1 * b2 - a2 * b1;
    x = (b1 * c2 - b2 * c1) / det;
    y = (a2 * c1 - a1 * c2) / det;
}

int main()
{
    int CountOfVertex = 5;
    // Инициализируем многоугольник
    Point *p = new Point[CountOfVertex];
    p[0].X = 4;  p[0].Z = 7;
    p[1].X = 10; p[1].Z = 10;
    p[2].X = 15; p[2].Z = 4;
    p[3].X = 11; p[3].Z = 1;
    p[4].X = 6;  p[4].Z = 1;

    // Задание 2 точек внутри и снаружи многоугольника
    Point M1,M2; // Непосредственно точки
    cout<<"Введите координаты точки:";
    cin>>M1.X>>M1.Z;
    M2.X = M1.X + 3; M2.Z = M1.Z;
    
    double A1, B1, C1;// Коэфициенты для уравнения прямой

    // Расчёт коэфициентов прямой для первой точки
    A1 = M1.Z - M2.Z;
    B1 = M2.X - M1.X;
    C1 = M1.X * M2.Z - M2.X * M1.Z; 
    cout<<"Уравнение первой прямой:"<<A1<<"x + "<<B1<<"y + "<<C1<<endl;

    // Находим максимальную вершину
    float Xmin= 100,Xmax = 0,Ymin = 100,Ymax = 0;
    for(int i = 0;i < CountOfVertex;i++)
    {
        if(p[i].X < Xmin) Xmin = p[i].X;
        if(p[i].X > Xmax) Xmax = p[i].X;
        if(p[i].Z < Ymin) Ymin = p[i].Z;
        if(p[i].Z > Ymin) Ymax = p[i].Z;
    }

    // Проверка внтури ли точки?
    int CounterOfIntersects = 0;
    for(int i = 0;i < CountOfVertex;i++)
    {
        // Высчитывание коэфициентов для сторон многоугольника
        double A2, B2, C2;
        if(i == CountOfVertex - 1)
        {
            A2 = p[i].Z - p[0].Z;
            B2 = p[0].X - p[i].X;
            C2 = p[i].X * p[0].Z - p[0].X * p[i].Z;
        }
        else
        {
           A2 = p[i].Z - p[i + 1].Z;
           B2 = p[i + 1].X - p[i].X;
           C2 = p[i].X * p[i + 1].Z - p[i + 1].X * p[i].Z;
        } 

        // Вывод уравнений прямых
        cout<<"Уравнение второй прямой:"<<A2<<"x + "<<B2<<"y + "<<C2<<endl;

        if(IsParall(A1,A2,B1,B2) == 1)
        {
            Intersect(A1,A2,B1,B2,C1,C2);
            if(x > M1.X && x < Xmax && M1.X > Xmin) // Менять при изменении точки
            {
                CounterOfIntersects++; // Если число пересечений чётно, то точка снаружи, если нечётно, то внутри
                cout<<"Точка пересечения с прямой "<<i + 1<<": X = "<<x<<"; Y = "<<y<<endl;
            }
            else cout<<"Прямая и отрезок не пересекаются в заданном диапозоне! Точка пересечения с прямой "<<i + 1<<": X = "<<x<<"; Y = "<<y<<endl;
        }
        else
        {
            cout<<"Прямые параллельны!"<<endl;
        }
        cout<<endl;
    }

    // Финальное сравнение
    if(CounterOfIntersects % 2 != 0) cout<<"Точка внутри полигона"<<endl;
    else cout<<"Точка снаружи полигона!"<<endl;

    return 0;
}