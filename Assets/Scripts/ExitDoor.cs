using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{

    public string sceneToLoad;

    public void exit(){
        SceneManager.LoadScene(sceneToLoad);
    }
}
