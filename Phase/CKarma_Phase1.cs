using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKarma_Phase1 : CEnemyPhase
{    
    private void Pattern1()
    {
        if(isDeActiveState)
        {            
            setPatternState(ePatternState.ACTIVE);
        }
        else if(isNotCompletePattern)
        {
            ((CKarma_PhaseManager)getPhaseManager).StartMovePattern();

            if(((CKarma_PhaseManager)getPhaseManager)._bStart)
                setPatternState(ePatternState.COMPLETE);
        }
    }

    private void Pattern2()
    {
        if (isDeActiveState)
        {
            setPatternState(ePatternState.ACTIVE);
        }
        else if (isNotCompletePattern)
        {
            if (((CKarma_PhaseManager)getPhaseManager)._ingredientList.Count <
                ((CKarma_PhaseManager)getPhaseManager)._iSpawnCnt &&
                ((CKarma_PhaseManager)getPhaseManager).GetStartCount() > 0)
            {
                ((CKarma_PhaseManager)getPhaseManager).checkBuggerHeight();
            }

            if (((CKarma_PhaseManager)getPhaseManager)._ingredientList.Count > 0)
            {
                ((CKarma_PhaseManager)getPhaseManager).Fireingredient();
            }
            else
            {
                setPatternState(ePatternState.COMPLETE);
            }
        }
    }

    public override void beginePhase()
    {
        base.beginePhase();

        setCurPattern(0);
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

        if (((CKarma_PhaseManager)getPhaseManager)._bChangePhase)
        {
            ((CKarma_PhaseManager)getPhaseManager).ChangePhase();            
        }

    }
}
