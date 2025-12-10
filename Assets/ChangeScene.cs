using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // nama scene game yang mau dibuka (set di Inspector)
    [SerializeField] private string gameSceneName = "Game";

    // dipanggil dari tombol Play
    public void OnPlayButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}