// Decompiled with JetBrains decompiler
// Type: PopupReisouTargetWeapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupReisouTargetWeapon : BackButtonMenuBase
{
  [SerializeField]
  protected GameObject dynReisouSkillDetails01;
  [SerializeField]
  protected NGxScrollMasonry Scroll;
  private GearReisouSkill skill;
  protected int RewardListMargin = 10;
  protected int RewardListHeight = 128;
  private BackButtonMenuBase basePopup;

  public IEnumerator Init(GearReisouSkill skill, BackButtonMenuBase basePopup)
  {
    this.skill = skill;
    this.basePopup = basePopup;
    yield return (object) this.setSkillDetail();
    yield return (object) this.setWeaponList();
  }

  public IEnumerator setSkillDetail()
  {
    Future<GameObject> prefabF = new ResourceObject("Prefabs/UnitGUIs/ReisouSkillDetail_01").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) prefabF.Result.Clone(this.dynReisouSkillDetails01.transform).GetComponent<ReisouSkillDetail_01>().Init(this.skill, this.basePopup);
  }

  public IEnumerator setWeaponList()
  {
    this.Scroll.Reset();
    ((Component) this.Scroll.Scroll).gameObject.SetActive(false);
    Future<GameObject> prefabF = Res.Prefabs.shop007_20.dir_List.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    int index1 = 0;
    GearReisouSkillWeaponGroup[] skillWeaponGroupArray = MasterData.GearReisouSkillWeaponGroupList;
    for (int index2 = 0; index2 < skillWeaponGroupArray.Length; ++index2)
    {
      GearReisouSkillWeaponGroup skillWeaponGroup = skillWeaponGroupArray[index2];
      if (skillWeaponGroup.group == this.skill.awake_weapon_group)
      {
        GameObject weapon = prefab.Clone(((Component) this.Scroll.Scroll).gameObject.transform);
        e = weapon.GetComponent<Shop00720Reward>().Init(MasterDataTable.CommonRewardType.gear, skillWeaponGroup.gear_GearGear, 0, skillWeaponGroup.gear.name);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        weapon.transform.localPosition = new Vector3(0.0f, (float) ((this.RewardListHeight + this.RewardListMargin) * -index1));
        ++index1;
        weapon = (GameObject) null;
      }
    }
    skillWeaponGroupArray = (GearReisouSkillWeaponGroup[]) null;
    ((Component) this.Scroll.Scroll).gameObject.SetActive(true);
    yield return (object) null;
    this.Scroll.ResolvePosition();
    this.Scroll.Scroll.UpdatePosition();
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
