using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCRKR : MonoBehaviour
{
    [SerializeField] GameObject ccrkr = null;
    [SerializeField] GameObject backGround = null;
    [SerializeField] GameObject startbackGround = null;
    [SerializeField] GameObject startBtn = null;
    [SerializeField] GameObject startBtnText = null;

    private void Start()
    {
        StartCoroutine(StartingGame());
    }
    public void Stop()
    {
        StopCoroutine(StartingGame());
    }
    private IEnumerator StartingGame()
    {
        yield return new WaitForSeconds(5f);
        ccrkr.SetActive(false);
        backGround.SetActive(false);
        startbackGround.SetActive(true);
        startBtn.SetActive(true);
        startBtnText.SetActive(true);

    }
}
