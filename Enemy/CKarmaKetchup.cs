using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CKarmaKetchup : CEnemyUnit
{
    public GameObject _FirePivot;
    public GameObject _PotatoBox;

    [Header("케첩발사 딜레이는 MAX값 수정")]
    public float _fShotMaxDelay;


    public float _fShotDelay;

    private int   _iObjCnt = 0;
    private Animator _anima;

    public List<CKarmaKetchupObject> _KetchupObj;

    private void Awake()
    {
        _anima = GetComponentInChildren<Animator>();
        _fShotDelay = _fShotMaxDelay;
    }
    protected override void unitLoop()
    {
        base.unitLoop();       
        
        if (_iObjCnt < 1)
        {
            _fShotDelay -= Time.deltaTime * CGameStateManager.instance.tryGetScenetime();

            if (_fShotDelay < 0)
            {     

                _anima.SetInteger("State", 1);

                _fShotDelay = _fShotMaxDelay;
            }
            else if (_anima.GetCurrentAnimatorStateInfo(0).IsName("SHOT") &&
                    _anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.15f)
            {
                Vector2 FireObjectPos = new Vector2(_FirePivot.transform.position.x, _FirePivot.transform.position.y);
                CGameStateManager.instance.scenePrefabProvider.spawnItemToActionEx<GameObject>(CKarma_Scene.ketchupFire, FireObjectPos, this.transform.eulerAngles);

                float fX = CGameStateManager.instance.getPlayerUnit().transform.position.x;

                Vector2 TempPos = new Vector2(fX - 1.5f, -1.43f);
                CGameStateManager.instance.scenePrefabProvider.spawnItemToAction<CKarmaKetchupCreate>(CKarma_Scene.ketchupCreate, TempPos);

                _anima.SetInteger("State", 0);
            }
        }
    }
}
