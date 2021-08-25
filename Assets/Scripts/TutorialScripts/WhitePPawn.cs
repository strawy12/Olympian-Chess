using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePPawn : MonoBehaviour
{
    [SerializeField]
    private GameObject movePlate_me;
    [SerializeField]
    private GameObject movePlate_other;
    [SerializeField]
    private GameObject BP;

    private GameObject MV_M;
    private GameObject MV_O;

    void Update()
    {
        if(TutorialManager.Instance.is2StoryEnd)
        {
            InstantiateMV_M();
            InstantiateMV_O();
        }
    }

    private void InstantiateMV_M()
    {
        MV_M=Instantiate(movePlate_me);
    }

    private void InstantiateMV_O()
    {
        MV_O=Instantiate(movePlate_other);
        TutorialManager.Instance.is2StoryEnd = false;
    }

    public void DestroyMV_M()
    {
        Destroy(MV_M);
    }

    public void DestroyMV_O()
    {
        Destroy(MV_O);
    }

    public void DestoryBP()
    {
        Destroy(BP);
    }

    public IEnumerator PositionMove()
    {
        Vector3 statPos = transform.position;
        Vector3 endPos = new Vector3(-0.348f, 0.343f, 0f);

        //float t = 0f;

        //while(t<1f)
        //{
        //    Debug.Log("qweqwe");
        //    transform.position = Vector3.Lerp(statPos, endPos, t);
        //    yield return null;
        //}
        transform.position = endPos;
        yield return null;

    }
}
