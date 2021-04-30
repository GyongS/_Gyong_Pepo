using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CICEObject : CEnemyUnit
{
    enum SpawnID
    {
        Left = 0,
        Right
    }

    SpawnID eSpawnID;
    byte byDirNum;

    private float _fX;

    [Header("x축 최소 이동 속도")]
    [SerializeField] private float _fMinspeed;

    [Header("x축 최대 이동 속도")]
    [SerializeField] private float _fMaxspeed;

    private float _fspeed;

    [Header("x축 사이 간격")]
    [SerializeField] private float _fdirX;

    [Header("점프력")]
    [SerializeField] private float _fjumpForce;

    [SerializeField] private Sprite[] _iceSprite;

    

    private bool _binitCheck = false;

    public override void gameObjectSetActiveOn()
    {
        base.gameObjectSetActiveOn();

        int iRand = Random.Range(0, 4);

        SpriteRenderer _spriteRenderer = null;

        if (anim as CSpriteAnimation != null)
        {
            _spriteRenderer = (anim as CSpriteAnimation).getSpriteRenderer;

            _spriteRenderer.sprite = _iceSprite[iRand] ?? null;
        }

        _fspeed = Random.Range(_fMinspeed, _fMaxspeed);

        if (transform.position.x <= getTarget().gameObject.transform.position.x)
        {
            byDirNum = 0;
        }
        else if (transform.position.x >= getTarget().gameObject.transform.position.x)
        {
            byDirNum = 1;
        }

        switch (byDirNum)
        {
            case 0:
                eSpawnID = SpawnID.Left;
                break;
            case 1:
                eSpawnID = SpawnID.Right;
                break;
        }
    }
    protected override void unitLoop()
    {
        base.unitLoop();

        if (this.transform.position.y <= -10f)
        {
            gameObjectSetActiveOff();
        }

        _fX = _fspeed * Time.deltaTime * CGameStateManager.instance.tryGetScenetime();

        if (eSpawnID == SpawnID.Left)
        {
            transform.position = new Vector3(transform.position.x + _fX, transform.position.y, 0f);
            transform.GetChild(0).transform.eulerAngles -= new Vector3(0, 0, 150 * Time.deltaTime * CGameStateManager.instance.tryGetScenetime());

           
        }
        else if (eSpawnID == SpawnID.Right)
        {
            transform.position = new Vector3(transform.position.x - _fX, transform.position.y, 0f);
            transform.GetChild(0).eulerAngles += new Vector3(0, 0, 150 * Time.deltaTime * CGameStateManager.instance.tryGetScenetime());

        }        
    }

    protected override void groundLand()
    {
        base.groundLand();

        if (eSpawnID == SpawnID.Left)
        {
            Vector2 TempPos = new Vector2(transform.position.x + _fdirX, transform.position.y);
            setParabolaTargetValue(TempPos, _fjumpForce);
        }
        else if (eSpawnID == SpawnID.Right)
        {
            Vector2 TempPos = new Vector2(transform.position.x - _fdirX, transform.position.y);
            setParabolaTargetValue(TempPos, _fjumpForce);
        }

    }

}