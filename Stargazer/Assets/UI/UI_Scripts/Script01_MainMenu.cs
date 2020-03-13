using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script01_MainMenu : MonoBehaviour {

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
