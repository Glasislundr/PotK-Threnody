// Decompiled with JetBrains decompiler
// Type: PopupLeaderFriendSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupLeaderFriendSkill : BackButtonPopupBase
{
  [Header("リーダー枠")]
  [SerializeField]
  private PopupLeaderFriendSkill.Layout layoutLeader;
  [SerializeField]
  private GameObject dirNoneLeaderSkill;
  [Header("フレンド枠")]
  [SerializeField]
  private PopupLeaderFriendSkill.Layout layoutFriend;
  [SerializeField]
  private GameObject dirNoneFriend;
  [SerializeField]
  private GameObject dirNoneGuestSkill;
  private GameObject prefabUnitIcon;
  private bool isSea;

  public static IEnumerator show(
    GameObject prefabUnitIcon,
    bool isSea,
    PlayerUnit leaderUnit,
    PlayerUnit friendUnit,
    BattleskillSkill friendLeaderSkill)
  {
    Future<GameObject> ldPrefab = new ResourceObject(isSea ? "Prefabs/UnitGUIs/PopupLeaderFriendSkill_Sea" : "Prefabs/UnitGUIs/PopupLeaderFriendSkill").Load<GameObject>();
    IEnumerator e = ldPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = ldPrefab.Result;
    ldPrefab = (Future<GameObject>) null;
    if (!Object.op_Equality((Object) result, (Object) null))
      yield return (object) Singleton<PopupManager>.GetInstance().open(result).GetComponent<PopupLeaderFriendSkill>().initialize(prefabUnitIcon, isSea, leaderUnit, friendUnit, friendLeaderSkill);
  }

  public IEnumerator initialize(
    GameObject prefabUnitIcon,
    bool isSea,
    PlayerUnit leaderUnit,
    PlayerUnit friendUnit,
    BattleskillSkill friendLeaderSkill)
  {
    this.prefabUnitIcon = prefabUnitIcon;
    this.isSea = isSea;
    BattleskillSkill leaderSkill = leaderUnit.leader_skill?.skill;
    yield return (object) this.setLayout(leaderUnit, leaderSkill, this.layoutLeader);
    this.dirNoneLeaderSkill.SetActive(leaderSkill == null);
    yield return (object) this.setLayout(friendUnit, friendLeaderSkill, this.layoutFriend);
    this.dirNoneFriend.SetActive(friendUnit == (PlayerUnit) null);
    this.dirNoneGuestSkill.SetActive(friendUnit != (PlayerUnit) null && friendLeaderSkill == null);
  }

  private IEnumerator setLayout(
    PlayerUnit unit,
    BattleskillSkill skill,
    PopupLeaderFriendSkill.Layout layout)
  {
    UnitIcon unitScript = this.prefabUnitIcon.Clone(layout.dynUnitIcon.transform).GetComponent<UnitIcon>();
    if (unit != (PlayerUnit) null)
    {
      IEnumerator e = unitScript.setSimpleUnit(unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitScript.setLevelText(unit);
      unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    }
    else
      unitScript.SetEmpty();
    unitScript.SetIconBoxCollider(false);
    unitScript = (UnitIcon) null;
    layout.TxtSkillName.SetTextLocalize(skill != null ? skill.name : "");
    if (skill != null)
    {
      UI2DSprite spriteSkillProperty1 = layout.spriteSkillProperty1;
      BattleskillGenre? nullable = skill.genre1;
      Sprite sprite1;
      if (!nullable.HasValue)
      {
        sprite1 = (Sprite) null;
      }
      else
      {
        nullable = skill.genre1;
        sprite1 = SkillGenreIcon.loadSprite(nullable.Value, this.isSea);
      }
      spriteSkillProperty1.sprite2D = sprite1;
      UI2DSprite spriteSkillProperty2 = layout.spriteSkillProperty2;
      nullable = skill.genre2;
      Sprite sprite2;
      if (!nullable.HasValue)
      {
        sprite2 = (Sprite) null;
      }
      else
      {
        nullable = skill.genre2;
        sprite2 = SkillGenreIcon.loadSprite(nullable.Value, this.isSea);
      }
      spriteSkillProperty2.sprite2D = sprite2;
    }
    else
    {
      ((Component) layout.spriteSkillProperty1).gameObject.SetActive(false);
      ((Component) layout.spriteSkillProperty2).gameObject.SetActive(false);
    }
    layout.TxtSkillDescription.SetTextLocalize(skill != null ? skill.description : "");
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  [Serializable]
  public class Layout
  {
    public GameObject dynUnitIcon;
    public UILabel TxtSkillName;
    public UILabel TxtSkillDescription;
    public UI2DSprite spriteSkillProperty1;
    public UI2DSprite spriteSkillProperty2;
  }
}
