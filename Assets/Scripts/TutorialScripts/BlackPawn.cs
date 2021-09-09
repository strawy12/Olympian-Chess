using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPawn : MonoBehaviour
{
    [SerializeField]
    private GameObject movePlate_me;

    [SerializeField]
    private GameObject knight;

    [SerializeField]
    private Transform p;

    private GameObject MV_M;

    bool once = true;
    void Update()
    {
        Knight_();
    }

    private void Knight_()
    {
        if (TutorialManager.Instance.is6Story)
        {
            MV_M = Instantiate(movePlate_me, new Vector3(0.297f,0.894f,0f), Quaternion.identity);
            StartCoroutine(KnightMove());
            TutorialManager.Instance.is6Story = false;

        }
    }
    private IEnumerator KnightMove()
    {
        yield return new WaitForSeconds(3.5f);

        Vector3 startPos = knight.transform.position;
        Vector3 endPos = new Vector3(1.041f, 1.006f, 0f);

        TutorialManager.Instance.turnEnd = true;
        Destroy(MV_M);

        SoundManager.Instance.MoveChessSound();
        //knight.transform.position = endPos;
        knight.transform.position = p.transform.position;


        yield return null;
    }
}
