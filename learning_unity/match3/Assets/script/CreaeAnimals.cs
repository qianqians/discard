using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaeAnimals : MonoBehaviour
{
    public struct animal
    {
        public GameObject box;
        public int x;
        public int y;
    }

    public animal[,] animals = new animal[6,6];
    public int len = 6;
    public int weight = 6;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < len; i++)
        {
            for(int j = 0; j < weight; j++)
            {
                animals[i, j].box = Resources.Load("Prefabs/boxSprite") as GameObject;
                var check = Instantiate(animals[i, j].box) as GameObject;

                check.transform.localPosition = new Vector3((float)(i*1.6-5.5), (float)(j*(0-1.6)+4), 0);

                animals[i, j].x = i;
                animals[i, j].y = j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
