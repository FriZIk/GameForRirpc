using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTest : MonoBehaviour
{

    [Header("Доступные виды растений")]
    public GameObject Ver; // Вершина
    public GameObject TreeProto; // Прототип какого-то дерева
    public GameObject Wall; // Чем отрисовывать контур полигона

    // Описания массивов вершин 
    public List <Vector3> Test1 = new List<Vector3>(); 
    public int CountOfVertex1 = 4;
    public List <Vector3> Test2 = new List<Vector3>();
    int CountOfVertex2 = 10; 
    public List <Vector3> Test3 = new List<Vector3>();
    int CountOfVertex3 = 6;
    public List <Vector3> Test4 = new List<Vector3>();
    int CountOfVertex4 = 8;
    public List <Vector3> Test5 = new List<Vector3>();
    int CountOfVertex5 = 6;


    void RunTest(List <Vector3> TestNumber, int CountOfVertex)
    {
           /* Настройка параметров генерации */
        Generator Test = new Generator();

        Test.NumberOfVegetables = CountOfVertex; // Задаём количество вершин
        Test.Vegetables = TestNumber; // Собственно вершины
        Test.ExistVertex = true; // Строим вершины
        Test.Step = 1; // Шаг генерации (растояние между деревьями)
        Test.isRandom = false; // Случайно ли генерируем деревья
        Test.CountOfRandomTrees = 20; // Сколько случайных деревьев заспаунить (только если isRandom == true)
        Test.ExistCircuit = true; // Строим ли контур (стороны многоугольника)

        // Задаём нужные префабы для генерации
        Test.Vertex = Ver;
        Test.Wall = Wall;
        Test.TreePrototype = TreeProto;
       // Test.GenerateForest(); // Запускаем генерацию
        Test.GenerateForest();
    }

    void Start()
    {
        // Задаём значения для верщин каждого из тестовых массивов
        
        /* Тест 1 */
        Test1.Add(new Vector3(2,0.5f,6));
        Test1.Add(new Vector3(6,0.5f,6));
        Test1.Add(new Vector3(6,0.5f,2));
        Test1.Add(new Vector3(2,0.5f,2));
        /* Тест пройдён !!!*/

        /* Тест 2 */
        Test2.Add(new Vector3(1 + 10,0.5f,2));
        Test2.Add(new Vector3(1 + 10,0.5f,4));
        Test2.Add(new Vector3(2 + 10,0.5f,6));
        Test2.Add(new Vector3(4 + 10,0.5f,7));
        Test2.Add(new Vector3(6 + 10,0.5f,7));
        Test2.Add(new Vector3(8 + 10,0.5f,6));
        Test2.Add(new Vector3(9 + 10,0.5f,4));
        Test2.Add(new Vector3(9 + 10,0.5f,2));
        Test2.Add(new Vector3(7 + 10,0.5f,1));
        Test2.Add(new Vector3(3 + 10,0.5f,1));
        /* Очень много пустых мест, проблема в пересечении вершин, оно работает некорректно*/
        /* Мест осталость немного только одна срока не отрисовывается, будем фиксить (пробелма с пересеченим сразу двух строн в передлах Xmax)*/

        /* Тест 3 */
        Test3.Add(new Vector3(4 + 20,0.5f,7));
        Test3.Add(new Vector3(7 + 20,0.5f,10));
        Test3.Add(new Vector3(10 + 20,0.5f,10));
        Test3.Add(new Vector3(15 + 20,0.5f,4));
        Test3.Add(new Vector3(11 + 20,0.5f,1));
        Test3.Add(new Vector3(6 + 20,0.5f,1));
        /* Тест не пройден, одна строка на вершине не отображается */

        /* Тест 4 */
        Test4.Add(new Vector3(2,0.5f,5 + 10));
        Test4.Add(new Vector3(2,0.5f,10 + 10));
        Test4.Add(new Vector3(5,0.5f,10 + 10));
        Test4.Add(new Vector3(5,0.5f,6 + 10));
        Test4.Add(new Vector3(9,0.5f,7 + 10));
        Test4.Add(new Vector3(12,0.5f,6 + 10));
        Test4.Add(new Vector3(9,0.5f,3 + 10));
        Test4.Add(new Vector3(4,0.5f,3 + 10));
        /* Прблема на вершинах как в тесте 3 и 2 */

        /* Тест 5 */
        Test5.Add(new Vector3(4 + 10,0.5f,7 + 10));
        Test5.Add(new Vector3(7 + 10,0.5f,10 + 10));
        Test5.Add(new Vector3(10 + 10,0.5f,10 + 10));
        Test5.Add(new Vector3(7 + 10,0.5f,4 + 10));
        Test5.Add(new Vector3(11 + 10,0.5f,1 + 10));
        Test5.Add(new Vector3(6 + 10,0.5f,1 + 10));
        /* Тест пройдён !!!*/

        /* Запуск генераторов на тестах */
        RunTest(Test1,CountOfVertex1); // Работает с прямой
        RunTest(Test2,CountOfVertex2); // Не работает с прямой доделать
        RunTest(Test3,CountOfVertex3); // Те же симптомы что и с тестом 2
        RunTest(Test4,CountOfVertex4); // Та же дичь
        RunTest(Test5,CountOfVertex5); // Очень странное поведение! Не понятно почему не сработало
    }
}
