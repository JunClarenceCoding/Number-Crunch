using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Object.FindObjectsOfType<DontDestroy>().Length; i++)
        {
            if(Object.FindObjectsOfType<DontDestroy>()[i] != this)
            {
                if(Object.FindObjectsOfType<DontDestroy>()[i].name == gameObject.name)
                {
                    Destroy(gameObject);
                }
            }
            
        }
        DontDestroyOnLoad(gameObject);
    }

    //  private void Awake()
    // {
    //     GameObject[] objs = GameObject.FindGameObjectsWithTag("Persistent");

    //     if (objs.Length > 1)
    //     {
    //         Destroy(this.gameObject);
    //     }

    //     DontDestroyOnLoad(this.gameObject);
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
