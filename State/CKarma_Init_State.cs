using KarmaSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKarma_Init_State : CEnemyState
{
    [SerializeField] GameObject _SpriteObject;

    public override void excuteState()
    {
        base.excuteState();

        var boss = getStateManager.getBossUnit;
        // Init 애니메이션이 끝나고 idle상태가 되었을 때
        // State 변경함수는 Apeear 클립의 마지막 부분에 있음.
        if (boss.getState == (int)eKarmaState.AppearLand && boss.anim as CSpineAnimation)
        {
            // State Init -> Attack 으로 변경

            if (boss.anim as CSpineAnimation != null)
            {
                CSpineAnimation spineAnim = null;

                spineAnim = (CSpineAnimation)boss.anim;
                bool condition = spineAnim.getAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
                bool condition2 = spineAnim.getAnimator.GetCurrentAnimatorStateInfo(0).IsName("AppearLand");

                if (condition && condition2)
                {                             
                    _SpriteObject.SetActive(true);
                    boss.anim.gameObjectSetActiveOff();
                    getStateManager.changeState(eBossState.Attack);
                }
                

            }
        }
    }
}
