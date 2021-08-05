# Olympian-Chess

## 원의 방정식 이해를 위한 리드미

private List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCnt, float height, Vector3 scale) // practical card sorting (Align the cards using the equation of a circle)
    {
        float[] objLerps = new float[objCnt];
        List<PRS> results = new List<PRS>(objCnt);

        switch (objCnt)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCnt - 1);
                for (int i = 0; i < objCnt; i++)
                    objLerps[i] = interval * i;  // 위에서 0에서 1까지의 위치값을 생성 및 정렬
                break;
        } // 카드 수가 3개까지는 고정값을 이용해서 정렬
        // 4개 이상부턴 원의 방정식을 이용해 카드를 정렬함


        for (int i = 0; i < objCnt; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]); // 위에서 생성한 위치값을 토대로한 위치를 타겟 포지션으로 설정
                                                                                          // (미리 지정한 카드정렬의 가장 왼쪽부터 오른쪽까지의 위치 사이에서 포지션이 구해짐)
            var targetRot = Utils.QI; // Quaternion identity의 줄임말 스크립트로 편의성을 준거임
            if (objCnt >= 4) // 4개 이상일 때만 원의 방정식을 이용
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2)); // 원의 방정식으로 식을 유도후 y값을 구한것(이해하고싶으면 물어보시면 알려드리겠음)
                curve = height >= 0 ? curve : -curve; // 높이가 여기서 음수라는것은 포지션으로 따졌을땐 양수이기에 커브에 음수를 줘 기존 y값에서 - 시키는것임
                                                      // 양수는 반대로
                targetPos.y += curve; // x값은 위에 Lerp로 조정 되었기에 고정값이 되있는 y값만 변경
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]); // 회전값도 변경
            }
            results.Add(new PRS(targetPos, targetRot, scale)); // 변경이 다 되었다면 그 position, rotation, scale 을 모두 저장후 results에 넣음
        }
        return results; // 다 끝났다면 그 값을 반환

    }
    // Code you don't necessarily need to understand

## 참고 
https://youtu.be/Xo7EEegTUfE?t=1549

## TryPutCard 함수 이해를 위한 리드미

private bool TryPutCard(bool isMine, bool isUsed) // Executed when using a card
    {
        if (isStop) return false; // false리턴이면 비정상적 종료를 의미함
        if (isUse) return false;
        if (isMine && myPutCount >= 1)
            return false;

        if (!isMine && otherCards.Count <= 0)
            return false;

        Card card = isMine ? selectCard : otherCards[Random.Range(0, otherCards.Count)];
        var targetCards = isMine ? myCards : otherCards;
        if (isUsed)
        {

            Skill sk = SkillManager.Inst.SpawnSkillPrefab(card, chessPiece);// 사용하면 사용된 카드의 값과 선택된 체스말을 토대로 스킬 프리팹이 생성됨
            if (isBreak) //만약 스킬 매니저에서 isbreak를 선언한다면 비정상적 종료
            {
                Destroy(sk);
                return false;
            }
            if (CheckSkillList("파도", GameManager.Inst.GetCurrentPlayer())) return true; // 파도나 수면이나 에로스의 사랑같은 경우에는 카드 사용 이후 무브플레이트가 나와서 한번더 체스말을 선택해야하기에 정상적 종료를 시킴
            if (CheckSkillList("수면", GameManager.Inst.GetCurrentPlayer())) return true;
            if (CheckSkillList("에로스의 사랑", GameManager.Inst.GetCurrentPlayer())) return true;

            DestroyCard(card, targetCards); // 사용한 카드는 삭제
            isUse = true; // 사용중을 표시함

            if (isMine) // 내 카드를 사용한거라면
            {
                if (selectCard.carditem.name == "전쟁광" || selectCard.carditem.name == "달빛") // 전쟁광이나 달빛일 경우에는 셀렉트카드를 초기화 시키지 않음
                    targetPicker.SetActive(false);
                else
                {
                    selectCard = null;
                    targetPicker.SetActive(false);
                }
            }

            CardAlignment(isMine); // 카드가 하나 사라졌기에 카드를 다시 정렬한다
            if (CheckSkillList("제물", GameManager.Inst.GetCurrentPlayer())) return true; // 이 또한 카드가 제물일 경우에는 자동 턴 마침을 하면 안되기에 만든 코드
            if (selectCard != null)
                if (selectCard.carditem.name == "전쟁광" || selectCard.carditem.name == "달빛") // 원래는 위에 있는 코드와 마찬가지로 이 코드들은 턴이 자동 종료되기에 짠 코드라 추후에 삭제 예정
                    return true;

            //TurnManager.Inst.EndTurn();
            return true;
        }
        else // 존재 이유가 없는 코드 추후 삭제 예정
        {
            targetCards.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false));
            CardAlignment(isMine);
            return false;
        }

    }

## 필살기 이해를 위한 리드미

public void SetselectPiece(Chessman cp) //Set of ChessPiece to be used for PilSalGi
    {
        GameManager.Inst.DestroyMovePlates(); // 남아있는 무브플레이트 삭제
        selectPiece = cp;
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(133, 101, 0, 0)); // 색 변경 후
        selectPiece.InitiateMovePlates(); //그 체스말의 이동범위 무브플레이트 생성
        UsingPilSalGi();
    }
    private void UsingPilSalGi() // 필살기 사용
    {
        MovePlate[] mps = FindObjectsOfType<MovePlate>();
        for (int i = 0; i < mps.Length; i++)
        {
            if(mps[i].Getreference() == selectPiece && mps[i].GetChessPiece() != selectPiece)
            {
                cps.Add(mps[i].GetChessPiece());
            }
            
        } // 무브플레이트 위에 있는 체스말들 체크

        for (int i = 0; i < cps.Count; i++)
        {
            if (cps[i] == null)
                continue;
            SkillManager.Inst.SetDontClickPiece(cps[i]);
        } // 그리고 그 체크된 체스말들을 모두 못움직이게 설정(필살기 적용)

        StartCoroutine(SkillEffect()); // 필살기 실행
        isUsePilSalGi = false; // 필살기 사용 종료
        selectPiece.DestroyMovePlates();
        TurnManager.Inst.ButtonColor(); // 턴 종료 버튼 활성화
        attackCnt = 0;
    }
    private void ResetPilSalGi() // 필살기 지속시간 종료후 리셋 시키는 함수
    {
        ChangeSprite();
        this.selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        isUsePilSalGi = false;
        this.selectPiece = null;
        cps = null;
    }
    private IEnumerator SkillEffect() // 스킬 이펙트 코루틴 함수
    {
        Chessman cp = null;
        int k = GetTurnTime() + 2;
        
        while (GetTurnTime() < k) // 두턴동안 지속
        {
            for (int i = 0; i < cps.Count; i++)
            {
                if (cps[i] == null)
                    continue;
                cp = cps[i];

                cp.spriteRenderer.material.SetColor("_Color", new Color32(255, 228, 0, 0));
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < cps.Count; i++)
            {
                if (cps[i] == null)
                    continue;
                cp = cps[i];

                cp.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
            }
            yield return new WaitForSeconds(0.2f);

            // 설정된 체스말들에게 이펙트 적용시키는 코드
        } 
        for (int i = 0; i < cps.Count; i++) 
        {
            if (cps[i] == null)
                continue;
            cp = cps[i];

            SkillManager.Inst.RemoveDontClickPiece(cp); // 다 끝나면 모두 초기화 시킴
        }
        
        ResetPilSalGi(); // 리셋
    }
}