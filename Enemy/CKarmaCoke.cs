using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKarmaCoke : CEnemyUnit
{
    private float _fX = 0f;

    [SerializeField]
    private Transform _TargetTransform;

    private bool _bTurn = false;


    public override void active()
    {
        base.active();

        _fX = stat.speed * Time.deltaTime * CGameStateManager.instance.inGameScene.sceneTime;

        if (this.transform.position.x <= -11f)
        {
            _bTurn = true;
        }
        if (this.transform.position.x >= _TargetTransform.position.x - 2f && _bTurn)
        {
            _bTurn = false;
        }
        if (Vector3.Distance(this.transform.position, _TargetTransform.position) >= 1f && _bTurn)
        {
            this.transform.position = new Vector3(this.transform.position.x + _fX, this.transform.position.y, 0f);
        }
        else if (!_bTurn)
        {
            this.transform.position = new Vector3(this.transform.position.x - _fX, this.transform.position.y, 0f);
        }
    }

    protected override void unitLoop()
    {
        base.unitLoop();
        active();
    }
}
