using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rucksack_Hinweis_aktivieren : MonoBehaviour
{
    public GameObject Rucksack;
    public GameObject Deaktivator;
    // Start is called before the first frame update
    void Start()
    {
        Rucksack.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (Deaktivator.activeSelf)
        {
            Rucksack.SetActive(false);
        }
    }
}
