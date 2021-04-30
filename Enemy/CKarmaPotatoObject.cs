using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CKarmaPotatoObject : CEnemyUnit
{
    private bool _bTriggerExit = false;
    private bool _bTriggerEnter = false;
    private int _byTurnCnt = 0;

    private Vector3 _vTempPos;

    [Header("PotatoSpeed")]
    [SerializeField] private float _fspeed;

    [Header("감자튀김 회전속도")]
    [SerializeField] private float _fRotspeed;

    [SerializeField] private Sprite[] _potatoSprite;
    private float _fRotZ;

    protected override void unitLoop()
    {
        base.unitLoop();

        _vTempPos = getTarget().transform.position;

        if (Vector3.Distance(_vTempPos, transform.position) >= 1f && !_bTriggerExit)
            MoveToTarget();


        if (_bTriggerExit)
        {
            if (transform.eulerAngles.z < _fRotZ)
                return;

            Set_RotationZ(_fRotspeed);

            if (!isGround)
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - 1f * Time.deltaTime * CGameStateManager.instance.tryGetScenetime(),
                    transform.position.z);
        }
    }

    protected override void groundLand()
    {
        base.groundLand();

    }

    private void Set_RotationZ(float _InRotation)
    {
        transform.eulerAngles += new Vector3(0f, 0f, transform.rotation.z + _InRotation * Time.deltaTime * CGameStateManager.instance.tryGetScenetime());
    }

    private void MoveToTarget()
    {
        Vector3 dir = _vTempPos - transform.position;

        transform.position += dir * _fspeed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    public override void gameObjectSetActiveOn()
    {
        base.gameObjectSetActiveOn();

        _bTriggerExit = false;

        int iRand = Random.Range(0, 4);

        SpriteRenderer _spriteRenderer = null;

        if (anim as CSpriteAnimation != null)
        {
            _spriteRenderer = (anim as CSpriteAnimation).getSpriteRenderer;

            _spriteRenderer.sprite = _potatoSprite[iRand] ?? null;
        }

    }

    public override void gameObjectSetActiveOff()
    {
        base.gameObjectSetActiveOff();

        _bTriggerExit = false;
        setUseGravity(false);
    }


    private void OnTriggerStay(Collider rhs)
    {
        if (rhs.gameObject.layer == LayerMask.NameToLayer("EMagenta"))
        {
            _bTriggerExit = false;
            setUseGravity(false);
        }
    }

    private void OnTriggerExit2D(Collider2D rhs)
    {
        if (rhs.gameObject.layer == LayerMask.NameToLayer("EMagenta"))
        {
            _bTriggerExit = true;
            _fRotZ = transform.eulerAngles.z - 45f;

            //setUseGravity(true);
        }
    }

}
