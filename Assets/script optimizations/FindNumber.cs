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
            Array.Sort(array);
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
        int start = 0;
        int end = RandomArray.Length - 1;
        int middle = 0;

        if (RandomArray[start] == targetnum) { return start; }
        else if (RandomArray[end] == targetnum) { return end; }

        while (start - end != -1)
        {
            middle = (start + end) / 2;

            if (RandomArray[middle] == targetnum) { return middle; }
            else if (RandomArray[middle] > targetnum) { end = middle; }
            else if (targetnum > RandomArray[middle]) { start = middle; }
        }
        if (targetnum == RandomArray[start]) { return start; }
        return -1;
    }
}
