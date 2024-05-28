// Decompiled with JetBrains decompiler
// Type: Popup00591Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup00591Menu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel m_Warning;
  [SerializeField]
  private UILabel m_Description;
  [SerializeField]
  private UILabel m_DescriptionReisou;
  [SerializeField]
  private UILabel m_DescriptionReisou2;
  [SerializeField]
  private UIGrid m_Grid;
  private Action m_YesCallback;
  private static readonly string COLOR_TAG_GREEN = "[00ff00]{0}[-]";
  private static readonly string COLOR_TAG_RED = "[ff0000]{0}[-]";

  public IEnumerator Init(
    GameCore.ItemInfo before,
    GameCore.ItemInfo after,
    PlayerItem beforeReisou,
    PlayerItem afterReisou,
    List<InventoryItem> materials,
    Action yesCallback,
    GameObject iconPrefab = null)
  {
    Consts consts = Consts.GetInstance();
    IEnumerator e;
    if (Object.op_Equality((Object) iconPrefab, (Object) null))
    {
      Future<GameObject> ItemIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = ItemIconF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      iconPrefab = ItemIconF.Result;
      ItemIconF = (Future<GameObject>) null;
    }
    this.m_YesCallback = yesCallback;
    foreach (InventoryItem item in materials)
    {
      ItemIcon icon = iconPrefab.CloneAndGetComponent<ItemIcon>(((Component) this.m_Grid).transform);
      e = icon.InitByItemInfo(item.Item);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      icon.SetRenseiIcon(false, false);
      icon.SetRenseiMaterialNum(item.Item.isTempSelectedCount ? item.Item.tempSelectedCount : 1);
      icon = (ItemIcon) null;
    }
    this.m_Grid.repositionNow = true;
    if (materials.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.gear.rarity.index >= 3)))
      this.m_Warning.SetTextLocalize(consts.Popup00591DescriptionRareText);
    else
      this.m_Warning.SetTextLocalize(consts.Popup00591DescriptionNormalText);
    if (before != null)
    {
      string str1 = after.gearLevel.ToString();
      if (before.gearLevel < after.gearLevel)
        str1 = Popup00591Menu.COLOR_TAG_GREEN.F((object) str1);
      else if (before.gearLevel > after.gearLevel)
        str1 = Popup00591Menu.COLOR_TAG_RED.F((object) str1);
      string str2 = after.gearLevelLimit.ToString();
      if (before.gearLevelLimit < after.gearLevelLimit)
        str2 = Popup00591Menu.COLOR_TAG_GREEN.F((object) str2);
      else if (before.gearLevelLimit > after.gearLevelLimit)
        str2 = Popup00591Menu.COLOR_TAG_RED.F((object) str2);
      this.m_Description.SetTextLocalize(consts.Popup00591DescriptionText.F((object) before.gearLevel, (object) before.gearLevelLimit, (object) str1, (object) str2));
    }
    if (afterReisou != (PlayerItem) null)
    {
      if (afterReisou.gear.isMythologyReisou())
      {
        PlayerMythologyGearStatus mythologyGearStatus1 = beforeReisou.GetPlayerMythologyGearStatus();
        PlayerMythologyGearStatus mythologyGearStatus2 = afterReisou.GetPlayerMythologyGearStatus();
        string str3 = this.setLevelString(mythologyGearStatus1.holy_gear_level, mythologyGearStatus2.holy_gear_level);
        string str4 = this.setLevelString(mythologyGearStatus1.holy_gear_level_limit, mythologyGearStatus2.holy_gear_level_limit);
        this.m_DescriptionReisou.SetTextLocalize(consts.Popup00591HolyReisouDescriptionText.F((object) mythologyGearStatus1.holy_gear_level, (object) mythologyGearStatus1.holy_gear_level_limit, (object) str3, (object) str4));
        string str5 = this.setLevelString(mythologyGearStatus1.chaos_gear_level, mythologyGearStatus2.chaos_gear_level);
        string str6 = this.setLevelString(mythologyGearStatus1.chaos_gear_level_limit, mythologyGearStatus2.chaos_gear_level_limit);
        this.m_DescriptionReisou2.SetTextLocalize(consts.Popup00591ChaosReisouDescriptionText.F((object) mythologyGearStatus1.chaos_gear_level, (object) mythologyGearStatus1.chaos_gear_level_limit, (object) str5, (object) str6));
      }
      else
      {
        string str7 = this.setLevelString(beforeReisou.gear_level, afterReisou.gear_level);
        string str8 = this.setLevelString(beforeReisou.gear_level_limit, afterReisou.gear_level_limit);
        if (afterReisou.gear.isHolyReisou())
        {
          this.m_DescriptionReisou.SetTextLocalize(consts.Popup00591HolyReisouDescriptionText.F((object) beforeReisou.gear_level, (object) beforeReisou.gear_level_limit, (object) str7, (object) str8));
          ((Component) this.m_DescriptionReisou2).gameObject.SetActive(false);
        }
        else
        {
          this.m_DescriptionReisou.SetTextLocalize(consts.Popup00591ChaosReisouDescriptionText.F((object) beforeReisou.gear_level, (object) beforeReisou.gear_level_limit, (object) str7, (object) str8));
          ((Component) this.m_DescriptionReisou2).gameObject.SetActive(false);
        }
      }
    }
    else
    {
      ((Component) this.m_DescriptionReisou).gameObject.SetActive(false);
      ((Component) this.m_DescriptionReisou2).gameObject.SetActive(false);
    }
  }

  private string setLevelString(int before_lv, int after_lv)
  {
    if (before_lv < after_lv)
      return Popup00591Menu.COLOR_TAG_GREEN.F((object) after_lv);
    if (before_lv <= after_lv)
      return after_lv.ToString();
    return Popup00591Menu.COLOR_TAG_RED.F((object) after_lv);
  }

  public void IbtnYes()
  {
    if (this.m_YesCallback == null)
      return;
    this.m_YesCallback();
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
