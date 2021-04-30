using UnityEngine;

public sealed class CKarmaKetchupCreate : CTimeAnimatorAnimation
{
    [Header("케첩 오브젝트의 Y 축")]
    [SerializeField] float fY = -4.7f;
    public override void gameObjectSetActiveOff()
    {
        base.gameObjectSetActiveOff();
        Vector2 TempPos = new Vector2(transform.position.x + 2f, fY);
        CGameStateManager.instance.scenePrefabProvider.spawnItemToAction<CKarmaKetchupObject>(CKarma_Scene.ketchupObject, TempPos);
    }


}
