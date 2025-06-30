using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class menu : MonoBehaviour
{
    public Button[] levels;
    public GameObject mainPanel;
    public GameObject levelsPanel;
    public GameObject volumenPanel;
    public void Exit()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Nivel1");
    }
    public void OpenPanel(GameObject panel)
    {

        mainPanel.SetActive(false);
        levelsPanel.SetActive(false);
        volumenPanel.SetActive(false);


        panel.SetActive(true);
    }
    public void Awake() {
        int UnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel",1);
        for (int i = 0; i<levels.Length;i++ )
        {
            levels[i].interactable = false;
        }
        for (int i = 0; i < UnlockedLevel; i++)
        {
            levels[i].interactable=true;
        }
    }
    public void OpenLevelSelect()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].gameObject == clicked)
            {
                SceneManager.LoadScene(i+1);
            }
        }
    }
}
