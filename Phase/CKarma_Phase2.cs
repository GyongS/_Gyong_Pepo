using KarmaSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKarma_Phase2 : CEnemyPhase
{
    private float _fJumpDelay = 0f;

    [Header("Karma 점프 딜레이")]
    [SerializeField] private float _fJumpMaxDelay = 0f;


    private int _iJumpCnt = 0;
    private void Pattern1()
    {
        if (isDeActiveState)
        {
            setPatternState(ePatternState.ACTIVE);
        }
        else if (isNotCompletePattern)
        {
            setPatternState(ePatternState.COMPLETE);
        }
    }

    public override void beginePhase()
    {
        base.beginePhase();

        setCurPattern(0);

        ((CKarma_PhaseManager)getPhaseManager).TempPosY = getPhaseManager.getStateManager.getBossUnit.transform.position.y;
    }

    public override void excutePhase()
    {
        if (isCompletePattern())
        {
            oatternStateTimer();
        }
        else
        {
            getPhaseSelector[getCurPattern]();
        }

        if (((CKarma_Unit)getPhaseManager.getStateManager.getBossUnit).isDead)
        {
            getPhaseManager.getStateManager.getBossUnit.changeState((int)eKarmaState.Dead);
            return;
        }

        AnimCtrl();

        if (getPhaseManager.getStateManager.getBossUnit.isGround && !((CKarma_PhaseManager)getPhaseManager)._bChangePhase)
        {
            _fJumpDelay += Time.deltaTime * CGameStateManager.instance.tryGetScenetime();
            if (_iJumpCnt > 0)
            {
                //CGameStateManager.instance.scenePrefabProvider.spawnItemToAction<GameObject>(CKarma_Scene.ice, );
                _iJumpCnt = 0;
            }
            if (_fJumpDelay >= _fJumpMaxDelay)
            {
                getPhaseManager.getStateManager.getBossUnit.changeState((int)eKarmaState.Jump);
            }
            else
                getPhaseManager.getStateManager.getBossUnit.changeState((int)eKarmaState.Land);
        }
    }

    private void AnimCtrl()
    {
        var BossAnim = ((CSpineAnimation)getPhaseManager.getStateManager.getBossUnit.anim);

        if (BossAnim.getAnimator.GetCurrentAnimatorStateInfo(0).IsName("jump") &&
            BossAnim.getAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            ((CKarma_PhaseManager)getPhaseManager).JumpingBurger();
            getPhaseManager.getStateManager.getBossUnit.changeState((int)eKarmaState.Up);
            _fJumpDelay = 0;
            _iJumpCnt = 1;
        }
        else if (BossAnim.getAnimator.GetCurrentAnimatorStateInfo(0).IsName("up") &&
            BossAnim.getAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            getPhaseManager.getStateManager.getBossUnit.changeState((int)eKarmaState.Down);
        }
    }

}
