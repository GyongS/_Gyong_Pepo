using Boo.Lang;
using UnityEngine;

public class CKarmaKetchupObject : CEnemyUnit
{
    // TODO 에셋번들로 빼면 좋을듯.
    [SerializeField] private Sprite[] _ketChupSprite;

    public bool _bColWithPotato = false;

    // object -> potato 위치 전해야됨.
    // object -> potato off 새로 뽑아야함.

    public override void gameObjectSetActiveOn()
    {
        base.gameObjectSetActiveOn();
        int iRand = Random.Range(0, 4);
        if (animIsVaild && anim as CSpriteAnimation != null)
            (anim as CSpriteAnimation).getSpriteRenderer.sprite = _ketChupSprite[iRand];

        var ingameScene = CGameStateManager.instance.inGameScene as CKarma_Scene;
        CKarmaKetchup ketChupBottle = ingameScene.getKarmaKetchupBottle();
        ketChupBottle._KetchupObj.Add(this);
    }

    public override void gameObjectSetActiveOff()
    {
        base.gameObjectSetActiveOff();

        var ingameScene = CGameStateManager.instance.inGameScene as CKarma_Scene;
        CKarmaKetchup ketChupBottle = ingameScene.getKarmaKetchupBottle();

        ketChupBottle._KetchupObj.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D rhs)
    {
        if (rhs.gameObject.layer == LayerMask.NameToLayer("EYellow"))
        {
            _bColWithPotato = true;
        }
    }

    private void OnTriggerExit2D(Collider2D rhs)
    {
        if (rhs.gameObject.layer == LayerMask.NameToLayer("EYellow"))
        {
            _bColWithPotato = false;
        }
    }
}
