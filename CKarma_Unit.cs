using UnityEngine;
using KarmaSpace;
using UnityEditor;

public sealed class CKarma_Unit : CEnemyBossUnit
{

    public CTimeAnimatorAnimation _timeAnim;
    public bool isDead = false;
    protected override void initLogic()
    {
        base.initLogic();

        setStat(CGameStateManager.instance.TryGetStat(eSceneInfo.Karma,CGameManager.instance.dataLoader.userData.getDifficulty(), "Karma_Stat"));
    }

    protected override void groundLand()
    {
        base.groundLand();
        
        if(stateManager.getPhaseManager.getMyPhaseNum == 0)
        {
            changeState((int)eKarmaState.AppearLand);
            stateManager.initstate(eBossState.Init);
        }
        else
        {
            Vector3 vthisPos = new Vector3(transform.position.x, transform.position.y, 0);
            var karmaPhaseManager = (CKarma_PhaseManager)stateManager.getPhaseManager;
            var gameStateManager = CGameStateManager.instance;
            var scenePrefabProvider = gameStateManager.scenePrefabProvider;
            scenePrefabProvider.spawnItemToAction<GameObject>(CScenePrefabProvider.LandEffect, vthisPos);

            Vector2 vTemp;
            for (int i = 0; i < 1; ++i)
            {
                int irand = Random.Range(0, 2);
                switch (irand)
                {
                    case 0:
                        vTemp = new Vector2(((CKarma_PhaseManager)stateManager.getPhaseManager).GetIcePivotTransform[0].position.x,
                        ((CKarma_PhaseManager)stateManager.getPhaseManager).GetIcePivotTransform[0].position.y);
                        CGameStateManager.instance.scenePrefabProvider.spawnItemToAction<CICEObject>(CKarma_Scene.ice, vTemp);                                               
                        break;
                    case 1:
                        vTemp = new Vector2(((CKarma_PhaseManager)stateManager.getPhaseManager).GetIcePivotTransform[1].position.x,
                        ((CKarma_PhaseManager)stateManager.getPhaseManager).GetIcePivotTransform[1].position.y);
                        CGameStateManager.instance.scenePrefabProvider.spawnItemToAction<CICEObject>(CKarma_Scene.ice, vTemp);
                        break;
                    default:
                        break;
                }
            }
            setVeloicity(Vector2.zero);
        }

        GameUtils.log("그라운드체크", eLogTag.IngameScene);
    }

    protected override void unitLoop()
    {
        base.unitLoop();

        if (stat.getHpPercent() <= 0 &&
        stateManager.getPhaseManager.getMyPhaseNum == 1 &&
        !isDead)
        {
            var BossAnim = ((CSpineAnimation)stateManager.getBossUnit.anim);
            bool bAnimCurName = BossAnim.getAnimator.GetCurrentAnimatorStateInfo(0).IsName("IDLE");

            if (bAnimCurName)
            {
                bossClear();
                isDead = true;
            }
            
        }
    }

    protected override void postDamageCal()
    {
        base.postDamageCal();

        _timeAnim.blinkSkin();
       
        if (stat.getHpPercent() <= 50 && stateManager.getPhaseManager.getMyPhaseNum == 0)
        {            
            stateManager.getPhaseManager.AddPhase();
            changeState((int)eKarmaState.Appear);
        }       

    }

    
}
