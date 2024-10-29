using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;


    // Start is called before the first frame update
    void Start()
    {
        if (level1Button != null)
        {
            level1Button.onClick.AddListener(LoadLevel1);
        }

        if (level2Button != null)
        {
            level2Button.onClick.AddListener(LoadLevel2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLevel1()
    {
        SceneManager.LoadScene("LevelMap"); 
    }


    void LoadLevel2()
    {
        SceneManager.LoadScene("DesignIterationScene"); 
    }
}
