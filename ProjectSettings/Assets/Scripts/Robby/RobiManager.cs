using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RobiManager : MonoBehaviour
{
    [SerializeField]
    private Transform mainCamera = null;
    [SerializeField]
    private Button mainButton = null;
    private Image button = null;
    [SerializeField]
    private GameObject popUpQuest = null;
    void Start()
    {
        mainCamera = Camera.main.transform;
        button = mainButton.GetComponent<Image>();
        button.color = new Color(219 / 255f, 160 / 255f, 106 / 255f);
    }

    void Update()
    {
    }
    public void Store()
    {
        button.color = new Color(1,1,1);
        mainCamera.transform.position = new Vector3(-9.5f, 0, -9f);
    }
    public void Main()
    {
        mainCamera.transform.position = new Vector3(0, 0, -9f);
    }
    public void Deck()
    {
        button.color = new Color(1,1,1);
        mainCamera.transform.position = new Vector3(10, 0, -9f);
    }
    public void OnClickgamestart()
    {
        SceneManager.LoadScene("Draft");
    }
    public void Quest()
    {
        popUpQuest.SetActive(true);
    }
    public void ExitQuest()
    {
        popUpQuest.SetActive(false);
    }
}
