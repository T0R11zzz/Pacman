using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMapController : MonoBehaviour
{
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(LoadStartscene);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadStartscene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
