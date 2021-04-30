using KarmaSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKarma_Attack_State : CEnemyState
{
    [SerializeField]
    private GameObject _CokeObj;

    public override void begineState()
    {
        base.begineState();

        //CGameStateManager.instance.ingameUI.bossGaugeBar.initGaugeBar(CGameStateManager.instance.inGameScene.sceneInfo, true);
        CGameStateManager.instance.ingameUI.bossGaugeBar.initGaugeBar(eSceneInfo.Karma, true);

        if (CGameManager.instance.dataLoader.userData.IsCleardStage(CGameStateManager.instance.inGameScene.sceneInfo))
        {
            _CokeObj.SetActive(true);
        }

        // 공격 콜라이더를 전부 켬
        getStateManager.getBossUnit.setCollidersEanble(true);

        // GetCurPhase 현재 페이즈에 맞는 클래스를 리턴함
        // InitPhase 처음 시작할 때
        // ChangePhase 처음 시작이 아닐 때

        if (getCurPhase == null)
            initPhase(getStateManager.getPhaseManager.getCurPhase);
        else
            changePhase(getStateManager.getPhaseManager.getCurPhase);

    }

    public override void excuteState()
    {
        base.excuteState();

        if (getCurPhase != null)
            getCurPhase.excutePhase();

        Phase2Start();
    }

    private void Phase2Start()
    {
        var boss = getStateManager.getBossUnit;
        // Init 애니메이션이 끝나고 idle상태가 되었을 때
        // State 변경함수는 Apeear 클립의 마지막 부분에 있음.
        if (boss.getState == (int)eKarmaState.Appear && boss.anim as CSpineAnimation)
        {
            _CokeObj.SetActive(false);

            // State Init -> Attack 으로 변경
            if (boss.anim as CSpineAnimation != null)
            {
                CSpineAnimation spineAnim = null;

                spineAnim = (CSpineAnimation)boss.anim;
                bool condition = spineAnim.getAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
                bool condition2 = spineAnim.getAnimator.GetCurrentAnimatorStateInfo(0).IsName("appear");

                if (condition && condition2)
                {
                    var karmaPhaseManager = (CKarma_PhaseManager)getStateManager.getPhaseManager;
                    karmaPhaseManager.GetColLinkObj().SetActive(true);

                    if (karmaPhaseManager._bChangePhase)
                    {
                        var gameStateManager = CGameStateManager.instance;
                        var scenePrefabProvider = gameStateManager.scenePrefabProvider;
                        var karmaScene = gameStateManager.inGameScene as CKarma_Scene;
                        scenePrefabProvider.spawnItemToAction<GameObject>(CScenePrefabProvider.appearEffect, karmaPhaseManager.GetPivotTransform[0].position);
                        scenePrefabProvider.spawnItemToAction<GameObject>(CScenePrefabProvider.appearEffect, karmaPhaseManager.GetPivotTransform[1].position);

                        // ketchup
                        var ketchupBottle = karmaScene.getKarmaKetchupBottle();
                        ketchupBottle.transform.position = karmaPhaseManager.GetPivotTransform[0].position;
                        ketchupBottle.gameObjectSetActiveOn();

                        var potatoBox = karmaScene.getPotatoBoxObject();
                        potatoBox.transform.position = karmaPhaseManager.GetPivotTransform[1].position;
                        potatoBox.gameObjectSetActiveOn();
                        karmaPhaseManager._bChangePhase = false;
                    }
                }

            }
        }
    }
}
