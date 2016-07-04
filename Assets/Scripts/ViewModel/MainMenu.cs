using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {


	void Start ()
    {
	
	}
	

	void Update ()
    {
	
	}

    public void PlayButton()
    {
        SceneManager.LoadScene("Level1");
    }
}
