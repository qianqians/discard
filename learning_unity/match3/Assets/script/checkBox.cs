using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkBox : MonoBehaviour
{
    public GameObject createAnimal()
    {
        string[] animalName = { "appleSprite", "blueberrySprite", "fryeggsSprite", "kiwifruitSprite", "peachSprite",
                                "pearSprite", "pitayaSprite", "strawberrySprite", "tomatoSprite", "watermelonSprite"};

        int index = Random.Range(0, 9);
        GameObject animal = Resources.Load("Prefabs/" + animalName[index]) as GameObject;
        return Instantiate(animal);
    }

    public void checkCreateSon()
    {
        if(transform.childCount == 0)
        {
            var son = createAnimal();
            son.transform.parent = transform;
            son.transform.position = transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        checkCreateSon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
