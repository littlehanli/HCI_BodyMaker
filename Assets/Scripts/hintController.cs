using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintController : MonoBehaviour
{
    public GameObject manager;
    BodyJoints bj;

    // Start is called before the first frame update
    void Start()
    {
        bj = manager.GetComponent<BodyJoints>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter()
    {
        /*Image Active*/
        bj.Hint();
        print("hint");
    }
}
