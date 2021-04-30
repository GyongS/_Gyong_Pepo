using UnityEngine;

public sealed class CKarma_Scene : CInGameScene
{
    public const string cheese = "Yellow_Cheese";
    public const string ice = "Cyan_ICE";
    public const string ketchupBottle = "Magenta_KetchupBottle";
    public const string ketchupCreate = "Magenta_KetchupCreate";
    public const string ketchupFire = "Magenta_KetchupFire";
    public const string ketchupObject = "Magenta_KetchupObject";
    public const string lettuce = "Cyan_Lettuce";
    public const string patty = "Black_Patty";
    public const string potatoBox = "Yellow_PotatoBox";
    public const string potatoCreate = "Yellow_PotatoCreate";
    public const string potatoObject = "Yellow_PotatoObject";
    public const string tomato = "Magenta_Tomato";
    public const string buggerIngredients = "BuggerIngredients";

    // ## warnig UNIQUE ##
    private int _karmaKetchupInstanceId = -1;
    private int _karmaPotatoBoxInstanceId = -1;

    #region function

    /// <summary>
    /// 하나가 고유하다고 전제 (케첩 보틀)
    /// </summary>
    public CKarmaPotatoBox getPotatoBoxObject()
    {
        CKarmaPotatoBox potatoBox = null;

        var gameStateManager = CGameStateManager.instance;

        if (_karmaPotatoBoxInstanceId == -1)
        {
            potatoBox = gameStateManager.scenePrefabProvider.spawnItemToFunc<CKarmaPotatoBox>(CKarma_Scene.potatoBox, Vector3.zero, false); ;
            _karmaPotatoBoxInstanceId = potatoBox.gameObject.GetInstanceID();
            return potatoBox;
        }
        else
        {
            if (gameStateManager.inGameScene.isVaildCompareRigidObject(_karmaPotatoBoxInstanceId))
                potatoBox = gameStateManager.inGameScene.getRigidObject(_karmaPotatoBoxInstanceId) as CKarmaPotatoBox;
        }

        return potatoBox;
    }

    /// <summary>
    /// 하나가 고유하다고 전제 (케첩 보틀)
    /// </summary>
    public CKarmaKetchup getKarmaKetchupBottle()
    {
        CKarmaKetchup ketchup = null;

        var gameStateManager = CGameStateManager.instance;

        if (_karmaKetchupInstanceId == -1)
        {
            ketchup = gameStateManager.scenePrefabProvider.spawnItemToFunc<CKarmaKetchup>(CKarma_Scene.ketchupBottle, Vector3.zero, false); ;
            _karmaKetchupInstanceId = ketchup.gameObject.GetInstanceID();
            return ketchup;
        }
        else
        {
            if (gameStateManager.inGameScene.isVaildCompareRigidObject(_karmaKetchupInstanceId))
                ketchup = gameStateManager.inGameScene.getRigidObject(_karmaKetchupInstanceId) as CKarmaKetchup;
        }

        return ketchup;
    }

    #endregion
}


