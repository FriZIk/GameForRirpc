using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTest : MonoBehaviour
{
    // Описания массивов вершин 
    public List <Vector3> Test1 = new List<Vector3>(); 
    public int CountOfVertex1 = 4;
    public List <Vector3> Test2 = new List<Vector3>();
    int CountOfVertex2 = 10; 
    public List <Vector3> Test3 = new List<Vector3>();
    int CountOfVertex3 = 6;
    public List <Vector3> Test4 = new List<Vector3>();
    int CountOfVertex4 = 8;
    void Start()
    {
        // Задаём значения для верщин каждого из тестовых массивов
        
        /* Тест 1 */
        Test1.Add(new Vector3(4,7,0.5f));
        Test1.Add(new Vector3(4,7,0.5f));
        Test1.Add(new Vector3(4,7,0.5f));
        Test1.Add(new Vector3(4,7,0.5f));

        /* Тест 2 */
        Test2.Add(new Vector3(1,2,0.5f));
        Test2.Add(new Vector3(1,4,0.5f));
        Test2.Add(new Vector3(2,6,0.5f));
        Test2.Add(new Vector3(4,7,0.5f));
        Test2.Add(new Vector3(6,7,0.5f));
        Test2.Add(new Vector3(8,6,0.5f));
        Test2.Add(new Vector3(9,4,0.5f));
        Test2.Add(new Vector3(9,2,0.5f));
        Test2.Add(new Vector3(7,1,0.5f));
        Test2.Add(new Vector3(3,1,0.5f));

        /* Тест 3 */
        Test3.Add(new Vector3(4,7,0.5f));
        Test3.Add(new Vector3(7,10,0.5f));
        Test3.Add(new Vector3(10,10,0.5f));
        Test3.Add(new Vector3(15,4,0.5f));
        Test3.Add(new Vector3(11,1,0.5f));
        Test3.Add(new Vector3(6,1,0.5f));

        /* Тест 4 */
        Test4.Add(new Vector3(2,5,0.5f));
        Test4.Add(new Vector3(2,10,0.5f));
        Test4.Add(new Vector3(5,10,0.5f));
        Test4.Add(new Vector3(5,6,0.5f));
        Test4.Add(new Vector3(9,7,0.5f));
        Test4.Add(new Vector3(12,6,0.5f));
        Test4.Add(new Vector3(9,3,0.5f));
        Test4.Add(new Vector3(4,3,0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        Generator Test = new Generator();
        Test.NumberOfVegetables = CountOfVertex2;
        for(int i = 0;i < Test.NumberOfVegetables;i++)
        {  
            Test.VertexPoint = Test2;
        }
    }
}
