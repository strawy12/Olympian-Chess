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

    bool once = true;
    void Update()
    {
        //InstantiateYellow();
        Knight_();
    }

    private void InstantiateYellow()
    {
        if (TutorialManager.Instance.isClicked && once)
        {
            Debug.Log("Move1");
            once = false;
            MV_M = Instantiate(movePlate_me, transform.position, Quaternion.identity);
        }
    }
    private void Knight_()
    {
        if (TutorialManager.Instance.is6Story)
        {
            MV_M = Instantiate(movePlate_me, transform.position, Quaternion.identity);
            StartCoroutine(KnightMove());
            TutorialManager.Instance.is6Story = false;

        }
    }
    private IEnumerator KnightMove()
    {
        yield return new WaitForSeconds(3.5f);

        Vector3 startPos = knight.transform.position;
        Vector3 endPos = new Vector3(1.02f, 1.02f, 0f);

        TutorialManager.Instance.turnEnd = false;
        Destroy(MV_M);

        TutorialManager.Instance.MoveChessSound();
        knight.transform.position = endPos;

        
        yield return null;
    }
}
