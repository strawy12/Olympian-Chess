using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPawn : MonoBehaviour
{
    [SerializeField]
    private GameObject movePlate_me;

    [SerializeField]
    private GameObject knight;
    
    private GameObject MV_M;

    void Update()
    {
        InstantiateYellow();
    }

    private void InstantiateYellow()
    {
        if(TutorialManager.Instance.is5StoryEnd)
        {
            TutorialManager.Instance.is5StoryEnd = false;
            Debug.Log("Move1");
            MV_M=Instantiate(movePlate_me, transform.position, Quaternion.identity);

            StartCoroutine(KnightMove());
        }
    }

    private IEnumerator KnightMove()
    {
        yield return new WaitForSeconds(5f);
        Vector3 startPos = knight.transform.position;
        Vector3 endPos = new Vector3(1.02f, 1.02f, 0f);

        Destroy(MV_M);
        knight.transform.position = endPos;

        TutorialManager.Instance.is6StoryEnd = true;
        yield return null;
    }
}
