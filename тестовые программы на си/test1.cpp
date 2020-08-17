#include <iostream>

using namespace std;

int IsParall(double A1, double A2, double B1, double B2)
{
    if(A1*B2 - A2*B1 != 0)return 1;
    else return 0;
}

void Intersect(double a1, double a2, double b1, double b2, double c1, double c2, double& x, double& y)
{
    double det = a1 * b2 - a2 * b1;
    x = (b1 * c2 - b2 * c1) / det;
    y = (a2 * c1 - a1 * c2) / det;
}

int main()
{
    double A1, A2, B1, B2, C1, C2; // Коэфициенты для уравнения прямой
    double x, y, x11, y11, x12, y12, x21, y21, x22, y22; // Координаты отрезков
    cout<<"Введите координаты 1-ой точки: "; cin >> x11 >> y11 >> x12 >> y12;
    cout<<"Введите координаты 2-ой точки: "; cin >> x21 >> y21 >> x22 >> y22;


    // Расчёт коэфициентов уравений прямой
    A1 = y11 - y12;
    B1 = x12 - x11;
    C1 = x11 * y12 - x12 * y11;

    A2 = y21 - y22;
    B2 = x22 - x21;
    C2 = x21 * y22 - x22 * y21;

    cout<<"Уравнение первой прямой:"<<A1<<"x + "<<B1<<"y + "<<C1<<endl;
    cout<<"Уравнение второй прямой:"<<A2<<"x + "<<B2<<"y + "<<C2<<endl;

    // Паралельны прямые или пересеккаются?
    if(IsParall(A1, A2, B1, B2) == 1)
    {
        Intersect(A1, A2, B1, B2, C1, C2, x, y);
        cout<<"Точка пересечения: x ="<<x<<",y = "<<y<<endl;
    }
    return 0;
}