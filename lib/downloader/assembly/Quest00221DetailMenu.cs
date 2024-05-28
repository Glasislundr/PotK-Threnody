// Decompiled with JetBrains decompiler
// Type: Quest00221DetailMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00221DetailMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtRecommendStrength_;
  [SerializeField]
  private UI2DSprite[] iconKinds_;
  [SerializeField]
  private UI2DSprite[] iconElements_;
  [SerializeField]
  private UI2DSprite[] iconAilments_;
  [SerializeField]
  private GameObject topDropView_;
  [SerializeField]
  private GameObject infoNoDrop_;
  [SerializeField]
  private UIScrollView scroll_;
  [SerializeField]
  private UIGrid grid_;
  [SerializeField]
  private Vector2 scaleDropIcon_ = new Vector2(0.8f, 0.8f);
  [SerializeField]
  private GameObject topSkillDetail_;
  private GameObject prefabElement_;
  private GameObject prefabSkillDetail_;
  private GameObject objSkillDetail_;
  private BattleskillSkill currentSkillDetail_;

  public IEnumerator coInitialize(QuestDetailData data)
  {
    this.initialize();
    this.txtName_.SetTextLocalize(data.name);
    this.txtRecommendStrength_.SetTextRecommendCombat(data.recommend_strength);
    for (int index = 0; index < data.kinds.Length && index < this.iconKinds_.Length; ++index)
    {
      this.iconKinds_[index].sprite2D = this.getSpriteGearKind(data.kinds[index]);
      ((Component) this.iconKinds_[index]).gameObject.SetActive(true);
    }
    Future<GameObject> ld1 = Res.Icons.CommonElementIcon.Load<GameObject>();
    IEnumerator e = ld1.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabElement_ = ld1.Result;
    ld1 = (Future<GameObject>) null;
    ld1 = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
    e = ld1.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabSkillDetail_ = ld1.Result;
    ld1 = (Future<GameObject>) null;
    CommonElementIcon component = this.prefabElement_.GetComponent<CommonElementIcon>();
    for (int index = 0; index < data.elements.Length && index < this.iconElements_.Length; ++index)
    {
      this.iconElements_[index].sprite2D = component.getIcon(data.elements[index]);
      ((Component) this.iconElements_[index]).gameObject.SetActive(true);
    }
    int i;
    if (data.ailments.Length != 0)
    {
      for (i = 0; i < data.ailments.Length && i < this.iconAilments_.Length; ++i)
      {
        Future<Sprite> ld2 = data.ailments[i].LoadBattleSkillIcon();
        e = ld2.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.iconAilments_[i].sprite2D = ld2.Result;
        ((Component) this.iconAilments_[i]).gameObject.SetActive(true);
        this.setEventClickedSkillIcon(((Component) this.iconAilments_[i]).gameObject, data.ailments[i]);
        ld2 = (Future<Sprite>) null;
      }
    }
    if (data.isDisplayDrops && data.drops.Length != 0)
    {
      this.topDropView_.SetActive(true);
      this.infoNoDrop_.SetActive(false);
      QuestDetailData.Drop[] dropArray = data.drops;
      for (i = 0; i < dropArray.Length; ++i)
      {
        e = this.coCreateDropIcon(dropArray[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      dropArray = (QuestDetailData.Drop[]) null;
      this.scroll_.ResetPosition();
      this.grid_.Reposition();
    }
    else if (data.isDisplayDrops)
    {
      this.topDropView_.SetActive(false);
      this.infoNoDrop_.SetActive(true);
    }
    else
    {
      this.topDropView_.SetActive(true);
      this.infoNoDrop_.SetActive(false);
    }
  }

  private void setEventClickedSkillIcon(GameObject go, BattleskillSkill skill)
  {
    EventDelegate.Set(go.GetComponent<UIButton>().onClick, (EventDelegate.Callback) (() =>
    {
      if (this.currentSkillDetail_ == skill && Object.op_Inequality((Object) this.objSkillDetail_, (Object) null) && this.objSkillDetail_.GetComponentInChildren<Battle0171111Event>().DialogConteiner.activeSelf)
        return;
      this.currentSkillDetail_ = skill;
      if (Object.op_Equality((Object) this.objSkillDetail_, (Object) null))
        this.objSkillDetail_ = this.prefabSkillDetail_.Clone(this.topSkillDetail_.transform);
      Battle0171111Event componentInChildren = this.objSkillDetail_.GetComponentInChildren<Battle0171111Event>();
      componentInChildren.setData(skill);
      componentInChildren.enableSkillLv(false);
      componentInChildren.Show();
    }));
  }

  private IEnumerator coCreateDropIcon(QuestDetailData.Drop data)
  {
    CreateIconObject cio = ((Component) this.grid_).gameObject.GetOrAddComponent<CreateIconObject>();
    IEnumerator e = cio.CreateThumbnail(data.rewardType, data.rewardId, data.quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    cio.GetIcon().transform.localScale = Vector2.op_Implicit(this.scaleDropIcon_);
  }

  private Sprite getSpriteGearKind(GearKindEnum eKind)
  {
    string path;
    switch (eKind)
    {
      case GearKindEnum.unique_wepon:
      case GearKindEnum.smith:
      case GearKindEnum.accessories:
      case GearKindEnum.dummy:
      case GearKindEnum.none:
        path = "Icons/Materials/GuideWeaponBtn/slc_unique_wepon_idle";
        break;
      default:
        path = string.Format("Icons/Materials/GuideWeaponBtn/slc_{0}_idle", (object) eKind.ToString());
        break;
    }
    return this.getSprite(path);
  }

  private Sprite getSprite(string path) => Resources.Load<Sprite>(path);

  private void initialize()
  {
    foreach (Component iconKind in this.iconKinds_)
      iconKind.gameObject.SetActive(false);
    foreach (Component iconElement in this.iconElements_)
      iconElement.gameObject.SetActive(false);
    foreach (Component iconAilment in this.iconAilments_)
      iconAilment.gameObject.SetActive(false);
  }

  public override void onBackButton() => this.onClickBack();

  public void onClickBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
