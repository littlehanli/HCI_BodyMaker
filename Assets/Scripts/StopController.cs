using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopController : MonoBehaviour
{
    public GameObject manager;
    BodyJoints bj;
    int times = 0;

    // Start is called before the first frame update
    void Start()
    {
        bj =manager.GetComponent<BodyJoints>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter()
    {
        times++;
        if (times > 3)
        {
            print("you are superman");
            bj.PutDown();
        }
        if (times > 4)
        {
            times = 0;
        }
    }
}
