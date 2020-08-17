#include <stdio.h>

int HowMany(int A1,int B1,int C1,int A2,int B2,int C2)
{
    if(A1*B2 - A2*B1 != 0)return 1;
    else return 0;
}

void intersect(float a1, float a2, float b1, float b2, float c1, float c2, float *x, float *y)
{
    float det = a1 * b2 - a2 * b1;
    x = (b1 * c2 - b2 * c1) / det;
    y = (a2 * c1 - a1 * c2) / det;
}

int main(void)
{   
    // Первая прямая
    int x11,y11,x12,y12;
    printf("Введите координаты 1-ой точки:");
    scanf("%d %d",&x11,&y11);
    printf("Введите координаты 2-ой точки:");
    scanf("%d %d",&x12,&y12);

    // A = dy B = -dx C = dy*x1 - dx*y1
    int dx1 = x12 - x11;
    int dy1 = y12 - y11;

    int A1 = dy1;
    int B1 = -(x12 - x11);
    int C1 = dy1*x11 - dx1*y11; // Ax + By - C = 0

    // Вторая прямая
    int x21,y21,x22,y22;
    printf("Введите координаты 1-ой точки:");
    scanf("%d %d",&x21,&y21);
    printf("Введите координаты 2-ой точки:");
    scanf("%d %d",&x22,&y22);

    int dx2 = x22 - x21;
    int dy2 = y22 - y21;
    
    int A2 = dy2;
    int B2 = -(x22 - x21);
    int C2 = dy2 * x21 - dx1 * y21;


    printf("Уравнение первой прямой: %dx + %dy + %d\n",A1,B1,C1);
    printf("Уравнение второй прямой: %dx + %dy + %d\n",A2,B2,C2);

    if(HowMany(A1,B1,C1,A2,B2,C2) == 1) 
    {
        // Пересечение отрезков
        // Определить, находятся ли координаты точки пересечения в пределах координат каждого отрезка.
        printf("Пересекаются\n");

        // Находим точку пересечения
        float x,y;
        intersect(A1,A2,B1,B2,C1,C2,&x,&y);
        printf("Точка пересечения:x = %d,y = %d",x,y);
    }
    else printf("Не пересекаются!\n");

    return 0;
}