using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BodyJoints : MonoBehaviour
{
    public GameObject[] bodyJoints;
    private GameObject[] temp;
    public GameObject stopSign,stick,destroySign;
    public GameObject EmptyPrefab;
    GameObject newObject;
    private float[] jointX, jointY, jointZ;
    private int lamp, plant, teapot, shelf, penholder,takepicture;
    private string nowGesture;
    private int THRESHOLD = 150;
    private bool key = false;
    Vector3 moved;
    public float speed = 1.0f;
    bool setposition = false;
    GameObject hint;
    public GameObject hintSign;
    public GameObject screenshoter;
    screenShot picture;
    int pictureTime = 0;
    public GameObject Image;
    public GameObject all;
    public GameObject people;

    // Start is called before the first frame update
    void Start()
    {
        picture = screenshoter.GetComponent<screenShot>();
        bodyJoints = new GameObject[10];
        temp = new GameObject[10];
        jointX = new float[10];
        jointY = new float[10];
        jointZ = new float[10];
        bodyJoints[0] = GameObject.Find("Hand_L");
        bodyJoints[1] = GameObject.Find("Elbow_L");
        bodyJoints[2] = GameObject.Find("Shoulder_L");
        bodyJoints[3] = GameObject.Find("Shoulder_R");
        bodyJoints[4] = GameObject.Find("Elbow_R");
        bodyJoints[5] = GameObject.Find("Hand_R");
        bodyJoints[6] = GameObject.Find("Eyebrows");
        /*for (int i = 0; i < 7; i++)
        {
            temp[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            temp[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            temp[i].transform.parent = this.transform;
        }*/
        all.SetActive(true);
        people.SetActive(true);
        stopSign.SetActive(false);
        destroySign.SetActive(false);
        stick.SetActive(false);
        hint = GameObject.Find("Hint");
        hint.SetActive(false);
        Image.SetActive(false);
        moved = new Vector3(stick.transform.position.x, stick.transform.position.y, stick.transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        print("shelf+ " + shelf + " penholder+ " + penholder + " plant+ " + plant + " lamp " + lamp + " teapot " + teapot + " takept+ " + takepicture);
        
        for (int i = 0; i < 7; i++)
        {
            //temp[i].transform.position = bodyJoints[i].transform.position;
            jointX[i] = bodyJoints[i].transform.position.x;
            jointY[i] = bodyJoints[i].transform.position.y;
            jointZ[i] = bodyJoints[i].transform.position.z;
        }
        //print("Y: " + jointY[0] + " " + jointY[1] + " " + jointY[2] + " " + jointY[3] + " " + jointY[4] + " " + jointY[5] + " " + jointY[6]);
        //print("X: " + jointX[0] + " " + jointX[1] + " " + jointX[2] + " " + jointX[3] + " " + jointX[4] + " " + jointX[5] + " " + jointX[6]);
        if(key == false)
        {
            DetectGesture();
        }
        nowGesture = SuccessGesture();
        if (nowGesture != null && key == true && nowGesture!="takepicture")
        {
            print(nowGesture);
            placeObject(nowGesture);
            nowGesture = null;
        }
        else if (nowGesture == "takepicture")
        {
            newObject = null;
            people.SetActive(false);
            all.SetActive(false);
            picture.TakeScreenshot(pictureTime);
            people.SetActive(true);
            ShowImage();
            pictureTime++;
            // wait for 10 seconds
            StartCoroutine(ExampleCoroutine());
            nowGesture = null;
        }
        else
        {
            UpdateNewObject(newObject);
        }
    }
    void ShowImage()
    {
        string path = "Assets/" + "Resources/screenshot" + pictureTime.ToString() + ".png";
        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.AssetDatabase.ImportAsset(path);
        UnityEditor.TextureImporter importer = UnityEditor.AssetImporter.GetAtPath(path) as TextureImporter;
        importer.textureType = UnityEditor.TextureImporterType.Sprite;
        AssetDatabase.WriteImportSettingsIfDirty(path);
        UnityEditor.AssetDatabase.Refresh();
        Sprite NewSprite = Resources.Load<Sprite>("screenshot" + pictureTime.ToString());
        SpriteRenderer sr = Image.GetComponent<SpriteRenderer>();
        sr.sprite = NewSprite;
        Image.SetActive(true);
    }
    void Restart()
    {
        all.SetActive(true);
        initiate();
        stopSign.SetActive(false);
        destroySign.SetActive(false);
        stick.SetActive(false);
        hint.SetActive(true);
        Image.SetActive(false);
        newObject = null;
        key = false;
        setposition = false;
        moved = new Vector3(stick.transform.position.x, stick.transform.position.y, stick.transform.position.z);
        Destroy(GameObject.Find("NewGameObject"));
        GameObject n=Instantiate(EmptyPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        n.name = "NewGameObject";
    }
    private void DetectGesture()
    {
        print("DetectGesture");
        if (jointY[0] > jointY[1] && jointY[5] > jointY[4] &&
            jointX[0] > jointX[1] && jointX[4] > jointX[5] &&
           Mathf.Abs(jointY[0] - jointY[6]) < 0.8f && Mathf.Abs(jointY[5] - jointY[6]) < 0.8f &&
           Mathf.Abs(jointX[0] - jointX[1]) < 0.8f && Mathf.Abs(jointX[5] - jointX[4]) < 0.8f &&
           Mathf.Abs(jointY[1] - jointY[2]) < 0.8f && Mathf.Abs(jointY[3] - jointY[4]) < 0.8f)
        {
            shelf++;
            print("shelf");
        }
        if (jointY[0] > jointY[1] && jointY[1] > jointY[2] && 
            jointY[5] > jointY[4] && jointY[4] > jointY[3] &&
            jointY[0] > jointY[6] && jointY[5] > jointY[6] &&
            Mathf.Abs(jointX[0] - jointX[1]) < 0.8f && Mathf.Abs(jointX[5] - jointX[4]) < 0.8f &&
            Mathf.Abs(jointX[1] - jointX[2]) < 0.8f && Mathf.Abs(jointX[4] - jointX[3]) < 0.8f)
        {
            penholder++;
            print("penholder");
        }
        if (jointX[5] > jointX[4] && jointX[4] > jointX[3] &&
            jointX[2] > jointX[1] && jointX[1] > jointX[0] &&
            jointY[5] > jointY[4] && jointY[4] > jointY[3] &&
            jointY[2] > jointY[1] && jointY[1] > jointY[0])
        {
            lamp++;
            print("lamp");
        }
        if (jointY[2] > jointY[1] && jointY[1] > jointY[0] &&
            jointY[5] > jointY[4] && jointY[4] > jointY[3] &&
            jointX[1] < jointX[2] && jointX[1] < jointX[0] &&
            jointX[3] < jointX[4] && jointX[4] < jointX[5])
        {
            teapot++;
            print("teapot");
        }
        if (jointY[2] > jointY[1] && jointY[1] > jointY[0] &&
            jointX[2] < jointX[1] && jointX[1] < jointX[0] &&
            jointX[3] < jointX[4] && jointX[4] < jointX[5] &&
            jointY[3] > jointY[4] && jointY[4] > jointY[5] )
            {
            plant++;
            print("plant");
        }
        if (jointY[2] > jointY[1] && jointY[2] > jointY[0] &&
            jointY[3] < jointY[4] && jointY[3] < jointY[5] &&
            jointY[5] > jointY[6] && jointY[6] > jointY[0] && 
            Mathf.Abs(jointY[0] - jointY[1]) < 0.8f && Mathf.Abs(jointY[5] - jointY[4]) < 0.8f&&
            Mathf.Abs(jointX[1] - jointX[2]) < 0.8f && Mathf.Abs(jointX[4] - jointX[3]) < 0.8f)
        {
            takepicture++;
            print("takepicture");
        }
    }
    private string SuccessGesture() 
    {
        if(shelf > THRESHOLD)
        {
            key = true;
            setposition = false;
            initiate();
            return "shelf";
        }
        if(lamp > THRESHOLD)
        {
            key = true;
            setposition = false;
            initiate();
            return "lamp";
        }
        if(teapot > THRESHOLD)
        {
            key = true;
            setposition = false;
            initiate();
            return "teapot";
        }
        if(plant > THRESHOLD)
        {
            key = true;
            setposition = false;
            initiate();
            return "plant";
        }
        if(penholder > THRESHOLD)
        {
            key = true;
            setposition = false;
            initiate();
            return "penholder";
        }
        if (takepicture > THRESHOLD)
        {
            key = true;
            setposition = false;
            initiate();
            return "takepicture";
        }
        return null;
    }
    private void initiate()
    {
        shelf = 0;
        penholder = 0;
        plant = 0;
        teapot = 0;
        lamp = 0;
        takepicture = 0;
    }
    private void placeObject(string gameObject)
    {
        if(key == true)
        {
            stopSign.SetActive(true);
            destroySign.SetActive(true);
            stick.SetActive(true);
            hintSign.SetActive(false);
            GameObject prefabnewObject = Resources.Load<GameObject>(gameObject+"_object");
            newObject = Instantiate(prefabnewObject, new Vector3(56.16f,1.9f,-10.97f), new Quaternion(0, 0, 0, 0));
            newObject.transform.parent = GameObject.Find("NewGameObject").transform;
        }

        moved = new Vector3(stick.transform.position.x, stick.transform.position.y, stick.transform.position.z);
    }
    private void UpdateNewObject(GameObject movingObject)
    {
        if (movingObject != null&&!setposition)
        {
            Vector3 delta = new Vector3(stick.transform.position.x - moved.x, stick.transform.position.y - moved.y, stick.transform.position.z - moved.z);
            movingObject.transform.Translate(new Vector3(delta.x, delta.y, delta.z)*speed);
            moved = new Vector3(stick.transform.position.x, stick.transform.position.y, stick.transform.position.z);
        }
    }
    public void PutDown()
    {
        setposition = true;
        key = false;
        stopSign.SetActive(false);
        stick.SetActive(false);
        hintSign.SetActive(true);
        hint.SetActive(false);
        Rigidbody r = newObject.transform.GetChild(0).GetComponent<Rigidbody>();
        r.useGravity = true;
        r.mass = 10000;
        initiate(); /* reset here*/
        print("PUTDOWN");
    }
    public void DestroyObject()
    {
        //print("DestroyObject");
        key = false;
        stopSign.SetActive(false);
        destroySign.SetActive(false);
        stick.SetActive(false);
    }
    public void Hint()
    {
        hint.SetActive(true);
        hintSign.SetActive(false);
    }

    IEnumerator ExampleCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(10);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        print("restart");
        Restart();
    }
}
