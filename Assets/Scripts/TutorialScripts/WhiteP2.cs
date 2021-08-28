using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteP2 : MonoBehaviour
{
    [SerializeField]
    private GameObject movePlate_me;
    [SerializeField]
    private GameObject movePlate_other;
    [SerializeField]
    private GameObject BP;

    private GameObject MV_M;
    private GameObject MV_O;

    void Start()
    {
        
    }

    void Update()
    {
        if (TutorialManager.Instance.is7Story)
        {
            InstantiateMV_M();
            InstantiateMV_O();
            TutorialManager.Instance.is7Story = false;
        }
    }
    private void InstantiateMV_M()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0f);
        MV_M = Instantiate(movePlate_me,pos,Quaternion.identity);
    }

    private void InstantiateMV_O()
    {
        Vector3 pos = new Vector3(0.342f, 1.026f, 0f);
        MV_O = Instantiate(movePlate_other,pos,Quaternion.identity);
    }

    public void DestroyMV_O()
    {
        Destroy(MV_O);
    }

    public void DestroyMV_M()
    {
        Destroy(MV_M);
    }
    public void DestoryBP()
    {
        Destroy(BP);
    }

    public IEnumerator PositionMove()
    {
        Vector3 startPos = new Vector3(transform.position.x,transform.position.y,0f);
        Vector3 endPos = new Vector3(0.336f,1.02f,0f);

        TutorialManager.Instance.MoveChessSound();
        transform.position = endPos;
        yield return null;
    }
}
