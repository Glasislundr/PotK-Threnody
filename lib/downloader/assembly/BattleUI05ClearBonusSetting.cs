// Decompiled with JetBrains decompiler
// Type: BattleUI05ClearBonusSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleUI05ClearBonusSetting : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> linkObjects;
  [SerializeField]
  private UISprite slcTxtBox;
  [SerializeField]
  private UILabel txtRwardMessage;
  [SerializeField]
  private GameObject[] objTragerBox;
  [SerializeField]
  private GameObject objHimeishi;
  [SerializeField]
  private GameObject dirEffect;
  [SerializeField]
  private GameObject slc_ClearBonus_message;
  private const int BOX_SIZE_BASE = 38;

  public IEnumerator CreateClearBonusIcon(
    List<BattleResultBonusInfo> Rewards,
    bool isButton = true,
    bool isUnitRarityCenter = false)
  {
    ((UIRect) this.slcTxtBox).alpha = 0.0f;
    if (Rewards.Count > 1 || Rewards[0].rewardType != MasterDataTable.CommonRewardType.coin)
    {
      this.linkObjects.ForEachIndex<GameObject>((Action<GameObject, int>) ((x, i) => x.SetActive(Rewards.Count - 1 == i)));
      GameObject linkObject = this.linkObjects[Rewards.Count - 1];
      int count = 0;
      foreach (Component component in linkObject.transform)
      {
        CreateIconObject target = component.gameObject.GetOrAddComponent<CreateIconObject>();
        IEnumerator e = target.CreateThumbnail(Rewards[count].rewardType, Rewards[count].rewardID, isButton: isButton);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (isUnitRarityCenter && (Rewards[count].rewardType == MasterDataTable.CommonRewardType.unit || Rewards[count].rewardType == MasterDataTable.CommonRewardType.material_unit))
          target.GetIcon().GetComponent<UnitIcon>().RarityCenter();
        ++count;
        target = (CreateIconObject) null;
      }
    }
  }

  public void SetClearBonusInfo(List<BattleResultBonusInfo> Rewards, bool isBoxSizeSet = true)
  {
    string str = "";
    if (Rewards.Count > 1 || Rewards[0].rewardType != MasterDataTable.CommonRewardType.coin)
    {
      this.objHimeishi.SetActive(false);
      ((IEnumerable<GameObject>) this.objTragerBox).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(true)));
      this.dirEffect.SetActive(true);
      int num = Rewards.Count<BattleResultBonusInfo>();
      for (int index = 0; index < num; ++index)
        str = str + Rewards[index].rewardMessage + (index == num - 1 ? "" : "\n");
    }
    else
    {
      this.objHimeishi.SetActive(true);
      ((IEnumerable<GameObject>) this.objTragerBox).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
      this.dirEffect.SetActive(false);
      str = Rewards[0].rewardMessage;
    }
    this.txtRwardMessage.text = str;
    int num1 = ((UIWidget) this.txtRwardMessage).height / 38;
    if (isBoxSizeSet)
    {
      UISprite slcTxtBox = this.slcTxtBox;
      ((UIWidget) slcTxtBox).height = ((UIWidget) slcTxtBox).height + 38 * (num1 - 1);
    }
    this.slc_ClearBonus_message.SetActive(Rewards.Any<BattleResultBonusInfo>((Func<BattleResultBonusInfo, bool>) (x => x.isMessage)));
    ((UIRect) this.slcTxtBox).alpha = 1f;
  }
}
