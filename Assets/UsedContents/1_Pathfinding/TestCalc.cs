using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCalc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10000; i++)
        {
            int z = i % 100;
            int x = i / 100;

            Debug.Log($"z: {z},x: {x}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
