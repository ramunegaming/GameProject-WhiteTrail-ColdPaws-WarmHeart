using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text livesText;

    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
        else
        {
            Debug.Log("Lives remaining: " + lives);
        }
    }
}