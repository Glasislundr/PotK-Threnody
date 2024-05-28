// Decompiled with JetBrains decompiler
// Type: Shop007_MaterialExchangePackMenu
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
public class Shop007_MaterialExchangePackMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private NGxScroll scroll;
  [SerializeField]
  private UIButton btnOk_;
  private GameObject dir_product;
  private GameObject detailPopup;

  public IEnumerator InitMaterialIcon(SelectTicketSelectSample sample)
  {
    Shop007_MaterialExchangePackMenu exchangePackMenu = this;
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) exchangePackMenu.dir_product, (Object) null))
    {
      prefabF = Res.Prefabs.shop007_22.dir_product.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      exchangePackMenu.dir_product = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    exchangePackMenu.title.SetTextLocalize(sample.reward_title);
    exchangePackMenu.scroll.Clear();
    List<SelectTicketSelectSample> samples = new List<SelectTicketSelectSample>();
    if (sample.entity_type == MasterDataTable.CommonRewardType.deck)
    {
      for (int index = 0; index < MasterData.SelectTicketSelectSampleList.Length; ++index)
      {
        int? deckId = MasterData.SelectTicketSelectSampleList[index].deckID;
        int rewardId = sample.reward_id;
        if (deckId.GetValueOrDefault() == rewardId & deckId.HasValue)
          samples.Add(MasterData.SelectTicketSelectSampleList[index]);
      }
    }
    for (int i = 0; i < samples.Count; ++i)
    {
      GameObject obj = Object.Instantiate<GameObject>(exchangePackMenu.dir_product);
      obj.SetActive(false);
      e = obj.GetComponent<Shop00722Product>().Init(samples[i], new Action<SelectTicketSelectSample>(exchangePackMenu.IconBtnAction));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      exchangePackMenu.scroll.Add(obj);
      obj = (GameObject) null;
    }
    foreach (Component child in ((Component) exchangePackMenu.scroll.grid).transform.GetChildren())
      child.gameObject.SetActive(true);
    exchangePackMenu.scroll.ResolvePosition();
    exchangePackMenu.btnOk_.onClick.Clear();
    // ISSUE: reference to a compiler-generated method
    exchangePackMenu.btnOk_.onClick.Add(new EventDelegate(new EventDelegate.Callback(exchangePackMenu.\u003CInitMaterialIcon\u003Eb__5_0)));
    if (Object.op_Equality((Object) exchangePackMenu.detailPopup, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      exchangePackMenu.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public void IconBtnAction(SelectTicketSelectSample content)
  {
    this.StartCoroutine(this.ShowDetailPopup(content));
  }

  private IEnumerator ShowDetailPopup(SelectTicketSelectSample content)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.detailPopup);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Shop00742Menu>().Init(content.entity_type, content.reward_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public override void onBackButton() => this.onClickCancel();

  public void onClickCancel()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
