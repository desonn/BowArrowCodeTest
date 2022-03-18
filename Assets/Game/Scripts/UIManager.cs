using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text scoreTxt;
    int score;
    private void Awake()
    {
      
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
      

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
   
    void Start()
    {
        scoreTxt.text = "Score: " + score;
    }

  public void UpdateScore()
    {
        score++;
        //scoreTxt.text = "Score: " + score;
    }
}
