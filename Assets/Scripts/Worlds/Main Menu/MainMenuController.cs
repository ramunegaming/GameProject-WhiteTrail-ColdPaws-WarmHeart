using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Button Startbutton;
    public Text uiText; // Assigned in Inspector
    private bool isSelected;

    public void StartGame() => SceneManager.LoadScene("LevelSelect");

    public void Options() => Debug.Log("Options button clicked");

    public void Start()
    {
        EventSystem.current.SetSelectedGameObject(Startbutton.gameObject);
    }
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }

    void Update()
    {
        if (isSelected)
            uiText.color = new Color(1, 1, 1, Mathf.PingPong(Time.time, 1) * 0.5f + 0.5f);
    }

    public void OnMouseEnter() => isSelected = true;

    public void OnMouseExit() => isSelected = false;
}