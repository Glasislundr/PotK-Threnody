// Decompiled with JetBrains decompiler
// Type: GearAddMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GearAddMaterial : MonoBehaviour
{
  [SerializeField]
  private GameObject materialAddImg;
  [SerializeField]
  private GameObject rateBaseImg;
  [SerializeField]
  private UILabel lblAddStatus;
  [SerializeField]
  private UILabel lblAddPercent;
  private ItemIcon itemIcon;

  public void Init()
  {
    this.materialAddImg.SetActive(true);
    this.rateBaseImg.SetActive(false);
    if (Object.op_Equality((Object) this.itemIcon, (Object) null))
      return;
    Object.Destroy((Object) ((Component) this.itemIcon).gameObject);
    this.itemIcon = (ItemIcon) null;
  }

  public IEnumerator Set(GameCore.ItemInfo baseGear, GameCore.ItemInfo materialGear)
  {
    GearBuildupLogic gearBuildupLogic = (GearBuildupLogic) null;
    int? nullable1 = new int?();
    this.materialAddImg.SetActive(false);
    this.rateBaseImg.SetActive(true);
    switch (materialGear.gear.kind.Enum)
    {
      case GearKindEnum.sword:
        int? nullable2 = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == 0));
        if (nullable2.HasValue)
          gearBuildupLogic = MasterData.GearBuildupLogicList[nullable2.Value];
        this.lblAddStatus.SetTextLocalize(Consts.GetInstance().UNIT_00443_AGILITY);
        break;
      case GearKindEnum.axe:
        int? nullable3 = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == 0));
        if (nullable3.HasValue)
          gearBuildupLogic = MasterData.GearBuildupLogicList[nullable3.Value];
        this.lblAddStatus.SetTextLocalize(Consts.GetInstance().UNIT_00443_STRENGTH);
        break;
      case GearKindEnum.spear:
        int? nullable4 = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == 0));
        if (nullable4.HasValue)
          gearBuildupLogic = MasterData.GearBuildupLogicList[nullable4.Value];
        this.lblAddStatus.SetTextLocalize(Consts.GetInstance().UNIT_00443_VITALITY);
        break;
      case GearKindEnum.bow:
        int? nullable5 = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == 0));
        if (nullable5.HasValue)
          gearBuildupLogic = MasterData.GearBuildupLogicList[nullable5.Value];
        this.lblAddStatus.SetTextLocalize(Consts.GetInstance().UNIT_00443_DEXTERITY);
        break;
      case GearKindEnum.gun:
        int? nullable6 = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == 0));
        if (nullable6.HasValue)
          gearBuildupLogic = MasterData.GearBuildupLogicList[nullable6.Value];
        this.lblAddStatus.SetTextLocalize(Consts.GetInstance().UNIT_00443_INTELLIGENCE);
        break;
      case GearKindEnum.staff:
        int? nullable7 = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == 0));
        if (nullable7.HasValue)
          gearBuildupLogic = MasterData.GearBuildupLogicList[nullable7.Value];
        this.lblAddStatus.SetTextLocalize(Consts.GetInstance().UNIT_00443_MIND);
        break;
      case GearKindEnum.shield:
        int? nullable8 = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == 0));
        if (nullable8.HasValue)
          gearBuildupLogic = MasterData.GearBuildupLogicList[nullable8.Value];
        this.lblAddStatus.SetTextLocalize(Consts.GetInstance().UNIT_00443_HP);
        break;
      case GearKindEnum.accessories:
        int? nullable9 = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == 0));
        if (nullable9.HasValue)
          gearBuildupLogic = MasterData.GearBuildupLogicList[nullable9.Value];
        this.lblAddStatus.SetTextLocalize(Consts.GetInstance().UNIT_00443_LUCK);
        break;
    }
    int num = 0;
    if (gearBuildupLogic != null)
      num = gearBuildupLogic.MaterialRank(materialGear.gearLevel);
    this.lblAddPercent.SetTextLocalize(num.ToString() + "%");
    IEnumerator e = this.CreateIcon(materialGear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CreateIcon(GameCore.ItemInfo gear)
  {
    GearAddMaterial gearAddMaterial = this;
    Future<GameObject> fp = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = fp.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gearAddMaterial.itemIcon = fp.Result.Clone(((Component) gearAddMaterial).transform).GetComponent<ItemIcon>();
    e = gearAddMaterial.itemIcon.InitByItemInfo(gear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
