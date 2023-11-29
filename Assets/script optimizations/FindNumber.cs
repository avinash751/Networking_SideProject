using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FindNumber : MonoBehaviour
{
    [SerializeField] int[] RandomArray = new int[30];
    private void Start()
    {
        FillRandomArray(RandomArray);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindTargetNumberFromArray();
        }
    }

    void FillRandomArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Random.Range(0, array.Length + 1);
        }
    }
    int SelectARandomNumberFromArray(int[] array)
    {
        int targetnum = Random.Range(0, array.Length + 1);
        Debug.Log("Selected Number To Search " + targetnum);
        return targetnum;
    }


    int  FindTargetNumberFromArray()
    {
        int targetnum = SelectARandomNumberFromArray(RandomArray);
        for (int i = 0; i < RandomArray.Length; i++)
        {
            if (RandomArray[i] == targetnum)
            {
                Debug.Log("Found Number " + targetnum + " At Index " + i);
                return i;
            }
        } 
        return -1;
    }
}
