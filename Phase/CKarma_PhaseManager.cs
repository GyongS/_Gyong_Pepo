using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public sealed class CKarma_PhaseManager : CEnemyPhaseManager
{
    #region PHASE1_MEMBER
    [SerializeField] private Transform _movementPivot;
    [SerializeField] private Transform _movementObject;

    [SerializeField] private Transform[] _TopBotBurn;
    [SerializeField] private Transform _FoodSpawnPivot;

    [SerializeField] private GameObject _MainSpriteObj;
    [SerializeField] private GameObject _MainSpineObj;
    [SerializeField] private GameObject _MainColLinkObj;
    public GameObject GetColLinkObj() => _MainColLinkObj;
    public GameObject Get_MainSprite() { return _MainSpriteObj; }

    public List<CObject> _ingredientList;
    private float _fX = 0f;

    [Header("재료들이 출발하기 전 카운트")]
    [SerializeField] private float _fCount = 0f;
    public float GetStartCount() { return _fCount; }
        

    private float _fTempCount = 0f;

    [Header("재료들이 앞으로가는 속도")]
    [SerializeField] private float _fSpeed = 5f;

    [Header("시작 할 때 윗빵이 위로가는 속도")]
    [SerializeField] private float _fUpSpeed = 10f;

    [Header("시작하고 윗빵이 정해진 위치에 도달했을 때")]
    public bool _bStart = false;

    // 재료들이 버거 사이에 도착 했는지?
    private bool _bArriveLocation;
    private bool _bArriveLocationClone;

    [Header("페이즈 전환 체크")]
    public bool _bChangePhase = false;
    public int _iSpawnCnt = 0;

    [Header("페이즈 전환 클론 오브젝트")]
    public GameObject[] _CloneObject;
    public GameObject[] _CloneTopbotObject;
    #endregion

    #region PHASE2_MEMBER
    [SerializeField] Transform[] _PivotTransform;
    public Transform[] GetPivotTransform => _PivotTransform;

    [SerializeField] Transform[] _icePivotTransform;
    public Transform[] GetIcePivotTransform => _icePivotTransform;


    public float TempPosY;

    #endregion



    public void StartMovePattern()
    {
        if (_movementObject.transform.position.y <= _movementPivot.position.y)
        {
            _movementObject.Translate(Vector2.up * _fUpSpeed * (Time.fixedDeltaTime * CGameStateManager.instance.tryGetScenetime()));            
        }
        else if(_iSpawnCnt <= 0)
        {
            if (getMyPhaseNum == 1)
            {
                _bChangePhase = true;
            }

            if (!_bChangePhase && getMyPhaseNum == 0)
            {
                SpawnFood();
                _bStart = true;
            }
        }
    }

    public void Fireingredient()
    {
        // 재료들이 버거 사이에 도착 했을 때 카운트 시작
        if (_ingredientList[0].gameObject.transform.position.x <= _TopBotBurn[0].position.x + 0.1f && _fCount > -1)
        {
            _fCount -= Time.deltaTime * CGameStateManager.instance.inGameScene.sceneTime;
            _bArriveLocation = true;
        }


        // 카운트가 끝나면 출발
        if (_fCount < 0)
        {
            _fCount = -1;
            _fX = _fSpeed * Time.deltaTime * CGameStateManager.instance.inGameScene.sceneTime;

            for (int i = 0; i < _ingredientList.Count; ++i)
            {
                _ingredientList[i].gameObject.transform.position = new Vector3(_ingredientList[i].gameObject.transform.position.x - _fX, _ingredientList[i].gameObject.transform.position.y, 0f);

                if (_ingredientList[i].gameObject.transform.position.x < -17f)
                {
                    for (int j = 0; j < _ingredientList.Count; ++j)
                    {
                        _ingredientList[j].gameObjectSetActiveOff();
                        _iSpawnCnt--;
                    }

                    _ingredientList.Clear();
                    _fCount = _fTempCount;
                    _bStart = false;
                    _iSpawnCnt = 0;
                    _bArriveLocation = false;
                }

            }
        }

    }

    private void SpawnFood()
    {
        float BurnHeight = Mathf.Abs((_TopBotBurn[0].position.y - 1.7f) - (_TopBotBurn[1].position.y)) / 5;

        Vector3 FoodPos = new Vector3(_FoodSpawnPivot.position.x, (_TopBotBurn[1].position.y + BurnHeight), 0f);

        int _Length = GameUtils.getCMYKColorLen();
        int irand = Random.Range(0, 4);

        List<eCMYKColor> colors = new List<eCMYKColor>(5);

        for (int i = 0; i < _Length; ++i)
        {
            colors.Add((eCMYKColor)i);
        }

        colors.Add((eCMYKColor)irand);

        GameUtils.shuffleList(colors);

        for (int i = 0; i < colors.Count; ++i)
        {
            
            var item = CGameStateManager.instance.scenePrefabProvider.spawnItemToFunc<CEnemyUnit>(CKarma_Scene.buggerIngredients, colors[i], FoodPos);          

            if (item != null)
            {
                if (item.animIsVaild)
                {
                    if(item.anim as CSpriteAnimation != null)
                    {
                        ((CSpriteAnimation)item.anim).getSpriteRenderer.sortingOrder = i;
                    }
                }

                item.setDeathEvent(() =>
                {
                    _ingredientList.Remove(item);
                });

                item.gameObjectSetActiveOn();
                item.addFlagObjState(ObjStat.eObjState.HoldInvisible);
                StartCoroutine(item.transform.StartMoveToPointCoruotine(new Vector2(_TopBotBurn[0].position.x, item.transform.position.y), 3, false, () => {
                    item.subFlagObjState(ObjStat.eObjState.HoldInvisible);
                }));
                _ingredientList.Add(item);
            }

            FoodPos.y += BurnHeight;
            _iSpawnCnt++;
        }

    }

    public void checkBuggerHeight()
    {
        if (!_bArriveLocation || _bChangePhase)
            return;

        float BurnHeight = Mathf.Abs((_TopBotBurn[0].position.y - 1.6f) - (_TopBotBurn[1].position.y)) / 5;
        Vector3 FoodPos = new Vector3(_FoodSpawnPivot.position.x, (_TopBotBurn[1].position.y + BurnHeight), 0f);

        for (int i = 0; i < _ingredientList.Count; ++i)
        {
            Vector3 calcVec = new Vector3(_ingredientList[i].transform.position.x, _TopBotBurn[1].transform.position.y + i * BurnHeight + 1.2f, _ingredientList[i].transform.position.z);
            _ingredientList[i].transform.position = Vector3.Lerp(_ingredientList[i].transform.position, calcVec, Time.deltaTime * CGameStateManager.instance.tryGetScenetime());
        }
        
        Vector3 TopBurnVec = new Vector3(_TopBotBurn[0].position.x, _TopBotBurn[1].transform.position.y + _ingredientList.Count + 1 * BurnHeight + 1.2f, _TopBotBurn[0].position.z);
        _TopBotBurn[0].position = Vector3.Lerp(_TopBotBurn[0].position, TopBurnVec, Time.deltaTime * CGameStateManager.instance.tryGetScenetime());
    }

     

    public void ChangePhase()
    {
       for(int i = 0; i < _CloneTopbotObject.Length; ++i)
        {
            _CloneTopbotObject[i].SetActive(true);

            if (!_bArriveLocationClone)
            {
                _CloneTopbotObject[0].transform.position = _TopBotBurn[0].position;
                _CloneTopbotObject[1].transform.position = _TopBotBurn[1].position;
            }
            _MainSpriteObj.SetActive(false);
        }

       foreach(var obj in _CloneObject)
        {
            obj.SetActive(true);
            if (obj.transform.position.x <= _CloneTopbotObject[0].transform.position.x)
                _bArriveLocationClone = true;
            else
                obj.transform.position = new Vector3(obj.transform.position.x - _fSpeed * Time.deltaTime * CGameStateManager.instance.tryGetScenetime(), obj.transform.position.y, obj.transform.position.z);
        }

       if(_bArriveLocationClone)
            combineBurger();
    }

    private void combineBurger()
    {
        // 0이면 윗빵 1이면 아랫빵
        int iTopbotNum = 1;

        for (int i = 0; i < _CloneObject.Length; ++i)
        {
            Vector3 calcVec = new Vector3(_CloneObject[i].transform.position.x, _CloneTopbotObject[iTopbotNum].transform.position.y + i, _CloneObject[i].transform.position.z);

            if (i == 0)
                calcVec = new Vector3(_CloneObject[i].transform.position.x, _CloneTopbotObject[iTopbotNum].transform.position.y + i + 1.7f, _CloneObject[i].transform.position.z);
            else if (i == 1)
                calcVec = new Vector3(_CloneObject[i].transform.position.x, _CloneTopbotObject[iTopbotNum].transform.position.y + i + .5f, _CloneObject[i].transform.position.z);
            else if (i == 2)
                calcVec = new Vector3(_CloneObject[i].transform.position.x, _CloneTopbotObject[iTopbotNum].transform.position.y + i - 1f, _CloneObject[i].transform.position.z);
            else if (i == 3)
                calcVec = new Vector3(_CloneObject[i].transform.position.x, _CloneTopbotObject[iTopbotNum].transform.position.y + i - 2.3f, _CloneObject[i].transform.position.z);
            else if (i == 4)
                calcVec = new Vector3(_CloneObject[i].transform.position.x, _CloneTopbotObject[iTopbotNum].transform.position.y + i - 4f, _CloneObject[i].transform.position.z);

            _CloneObject[i].transform.position = Vector3.Lerp(_CloneObject[i].transform.position, calcVec, 5f * Time.deltaTime * CGameStateManager.instance.tryGetScenetime());
        }

        Vector3 TopBurnVec = new Vector3(_CloneTopbotObject[0].transform.position.x, _CloneTopbotObject[iTopbotNum].transform.position.y + 3f, _CloneTopbotObject[0].transform.position.z);

        _CloneTopbotObject[0].transform.position = Vector3.Lerp(_CloneTopbotObject[0].transform.position, TopBurnVec, 5f * Time.deltaTime * CGameStateManager.instance.tryGetScenetime());

        if (_CloneTopbotObject[0].transform.position.y <= -1f)
        {
            _MainSpineObj.SetActive(true);
            foreach(var obj in _CloneObject)
            {
                obj.SetActive(false);
            }

            foreach(var obj in _CloneTopbotObject)
            {
                obj.SetActive(false);
            }
            getStateManager.getCurState.changePhase(getCurPhase);
        }

    }

    public void JumpingBurger()
    {
        getStateManager.getBossUnit.setParabolaTargetValue(new Vector2(CGameStateManager.instance.getPlayerUnit().transform.position.x, TempPosY), 1.3f);
    }


    public override void initPhase()
    {
        base.initPhase();
        _fTempCount = _fCount;
    }

}
