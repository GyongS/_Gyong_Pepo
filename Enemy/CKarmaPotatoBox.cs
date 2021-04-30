using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKarmaPotatoBox : CEnemyUnit
{
    [Header("감자튀김 발사 쿨타임")]
    [SerializeField] private float _fMaxDelay = 3f;

    [SerializeField] private float _fDelay = 0f;


    protected override void unitLoop()
    {
        base.unitLoop();       

        _fDelay += Time.deltaTime * CGameStateManager.instance.tryGetScenetime();

        if(_fDelay >= _fMaxDelay)
        {
            var ingameScene = CGameStateManager.instance.inGameScene as CKarma_Scene;
            CKarmaKetchup ketChupBottle = ingameScene.getKarmaKetchupBottle();                   

            for(int i = 0; i < ketChupBottle._KetchupObj.Count; ++i)
            {
                if (!ketChupBottle._KetchupObj[i]._bColWithPotato)
                {
                    var PotatoBox = ingameScene.getPotatoBoxObject();
                    int iRand = Random.Range(0, ketChupBottle._KetchupObj.Count);

                    if (ketChupBottle._KetchupObj[iRand]._bColWithPotato)
                        continue;

                    var potatoObj = CGameStateManager.instance.scenePrefabProvider.spawnItemToFunc<CKarmaPotatoObject>(
                      CKarma_Scene.potatoObject,
                      PotatoBox.gameObject.transform.position);
                    potatoObj.setTarget(ketChupBottle._KetchupObj[iRand]);
                    potatoObj.setUseGravity(false);
                    potatoObj.gameObjectSetActiveOn();
                    _fDelay = 0f;
                    break;
                }
            }
        }
    }

    public override void gameObjectSetActiveOn()
    {
        base.gameObjectSetActiveOn();


    }

    public override void gameObjectSetActiveOff()
    {
        base.gameObjectSetActiveOff();

    }
}
