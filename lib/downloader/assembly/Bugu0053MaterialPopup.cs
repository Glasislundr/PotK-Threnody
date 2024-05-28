// Decompiled with JetBrains decompiler
// Type: Bugu0053MaterialPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu0053MaterialPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtMaterialName;
  [SerializeField]
  private UILabel TxtMaterialNum;
  [SerializeField]
  private UILabel TxtRankNeed;
  [SerializeField]
  private UILabel[] TxtQuestName;
  [SerializeField]
  private GameObject IconMaterial;
  [SerializeField]
  private UISprite[] LineQuest;
  private Bugu0053DirRecipePopup root;
  private List<string> gearDescriptions = new List<string>();

  public IEnumerator Init(
    Bugu0053DirRecipePopup recipePopup,
    GearGear gear,
    GameObject ItemIconPrefab,
    int quantity,
    int requestRank)
  {
    this.root = recipePopup;
    this.root.isBackKey = false;
    GearMaterialQuestInfo info = ((IEnumerable<GearMaterialQuestInfo>) MasterData.GearMaterialQuestInfoList).FirstOrDefault<GearMaterialQuestInfo>((Func<GearMaterialQuestInfo, bool>) (x => x.gear_id == gear.group_id));
    this.TxtMaterialName.SetTextLocalize(gear.name);
    this.TxtMaterialNum.SetTextLocalize(Consts.Format(Consts.GetInstance().Bugu0053MaterialPopup_TxtMaterialNum, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (quantity),
        (object) quantity
      }
    }));
    this.TxtRankNeed.SetTextLocalize(Consts.Format(Consts.GetInstance().Bugu0053MaterialPopup_TxtRankNeed, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (requestRank),
        (object) requestRank
      }
    }));
    this.SetLineQuest(info);
    IEnumerator e = ItemIconPrefab.Clone(this.IconMaterial.transform).GetComponent<ItemIcon>().InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator CallItemInit(GearGear gear, GameObject ItemIconPrefab, int quantity)
  {
    GearMaterialQuestInfo info = ((IEnumerable<GearMaterialQuestInfo>) MasterData.GearMaterialQuestInfoList).FirstOrDefault<GearMaterialQuestInfo>((Func<GearMaterialQuestInfo, bool>) (x => x.gear_id == gear.group_id));
    this.TxtMaterialName.SetTextLocalize(gear.name);
    this.TxtMaterialNum.SetTextLocalize(Consts.Format(Consts.GetInstance().Bugu0053MaterialPopup_TxtMaterialNum, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (quantity),
        (object) quantity
      }
    }));
    ((Component) this.TxtRankNeed).gameObject.SetActive(false);
    this.SetLineQuest(info);
    IEnumerator e = ItemIconPrefab.Clone(this.IconMaterial.transform).GetComponent<ItemIcon>().InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void SetLineQuest(GearMaterialQuestInfo info)
  {
    if (info == null)
    {
      foreach (Component component in this.TxtQuestName)
        component.gameObject.SetActive(false);
      foreach (Component component in this.LineQuest)
        component.gameObject.SetActive(false);
    }
    else
    {
      this.gearDescriptions.Add(info.detail_desc1);
      this.gearDescriptions.Add(info.detail_desc2);
      this.gearDescriptions.Add(info.detail_desc3);
      if (string.IsNullOrEmpty(info.detail_desc2))
        ((Component) this.LineQuest[0]).gameObject.SetActive(false);
      if (string.IsNullOrEmpty(info.detail_desc3))
        ((Component) this.LineQuest[1]).gameObject.SetActive(false);
      int count = this.gearDescriptions.Count;
      for (int index = 0; index < count; ++index)
        this.TxtQuestName[index].SetTextLocalize(this.gearDescriptions[index]);
    }
  }

  public void IbtnNo()
  {
    if (Object.op_Inequality((Object) this.root, (Object) null))
      Singleton<CommonRoot>.GetInstance().StartCoroutine(this.root.BackKeyEnable());
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
