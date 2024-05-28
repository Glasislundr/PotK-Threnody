// Decompiled with JetBrains decompiler
// Type: Versus02612ScrollRewardItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Versus02612ScrollRewardItem : MonoBehaviour
{
  [SerializeField]
  private UILabel txtDetail;
  [SerializeField]
  private UIButton ibtnDetail;
  [SerializeField]
  private GameObject objLine;
  [SerializeField]
  private GameObject linkTarget;
  private GameObject detailPopup;
  private MasterDataTable.CommonRewardType rewardType;
  private int rewardID;
  private bool isDetail;
  private bool isReisou;
  private int[] canDetailItems = new int[9]
  {
    1,
    24,
    3,
    26,
    35,
    2,
    19,
    21,
    29
  };

  public IEnumerator CreateItem(
    MasterDataTable.CommonRewardType rewardType,
    int rewardID,
    string txt,
    bool isLineObj,
    bool isUnitRarityCenter = false)
  {
    this.txtDetail.SetTextLocalize(txt);
    this.objLine.SetActive(isLineObj);
    this.isDetail = false;
    this.rewardType = rewardType;
    this.rewardID = rewardID;
    CreateIconObject target = this.linkTarget.GetOrAddComponent<CreateIconObject>();
    IEnumerator e = target.CreateThumbnail(rewardType, rewardID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (isUnitRarityCenter && (rewardType == MasterDataTable.CommonRewardType.unit || rewardType == MasterDataTable.CommonRewardType.material_unit))
      target.GetIcon().GetComponent<UnitIcon>().RarityCenter();
    this.isReisou = false;
    if (rewardType == MasterDataTable.CommonRewardType.gear)
    {
      this.isReisou = MasterData.GearGear[rewardID].isReisou();
      ((Component) this.ibtnDetail).gameObject.SetActive(true);
    }
    if (((IEnumerable<int>) this.canDetailItems).Any<int>((Func<int, bool>) (x => (MasterDataTable.CommonRewardType) x == rewardType)) && !this.isReisou)
    {
      ((Component) this.ibtnDetail).gameObject.SetActive(true);
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public void IbtnDetail()
  {
    if (this.isDetail)
      return;
    this.isDetail = true;
    this.StartCoroutine(this.onDetail());
  }

  private IEnumerator onDetail()
  {
    if (this.isReisou)
    {
      GearGear gear = (GearGear) null;
      MasterData.GearGear.TryGetValue(this.rewardID, out gear);
      PlayerItem playerItemDummy = new PlayerItem(gear, MasterDataTable.CommonRewardType.gear);
      GameCore.ItemInfo itemInfo = new GameCore.ItemInfo(playerItemDummy);
      yield return (object) this.linkTarget.GetOrAddComponent<CreateIconObject>().GetIcon().GetComponent<ItemIcon>().OpenReisouDetailPopupAsync(itemInfo, playerItemDummy);
      this.isDetail = false;
    }
    else
    {
      GameObject detail = this.detailPopup.Clone();
      detail.SetActive(false);
      IEnumerator e = detail.GetComponent<ItemDetailPopupBase>().SetInfo(this.rewardType, this.rewardID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(detail, isCloned: true);
      detail.SetActive(true);
      this.isDetail = false;
    }
  }
}
