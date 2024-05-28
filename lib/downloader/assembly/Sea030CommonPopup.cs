// Decompiled with JetBrains decompiler
// Type: Sea030CommonPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030CommonPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private GameObject notObject;
  [SerializeField]
  private UILabel notTitle;
  [SerializeField]
  private UILabel notText1;
  [SerializeField]
  private UILabel notText2;
  [SerializeField]
  private GameObject yesNoObject;
  [SerializeField]
  private UILabel yesNoText1;
  [SerializeField]
  private UILabel yesNoText2;
  [SerializeField]
  private UILabel yesNoText3;
  private bool popupEnd;
  private PlayerUnit unitData;
  private Action<PlayerTalkMessage> message;

  public IEnumerator initialize(
    PlayerUnit unit,
    PlayerCallLetter[] callLetter,
    Action<PlayerTalkMessage> act)
  {
    this.unitData = unit;
    if (act != null)
      this.message = act;
    int num1 = 0;
    if (callLetter != null)
    {
      for (int index = 0; index < callLetter.Length; ++index)
      {
        if (callLetter[index].call_status == 1)
          ++num1;
      }
    }
    if (num1 >= 10)
    {
      this.notText1.SetText(string.Format("{0}と\nTalk IDを交換できませんでした", (object) unit.unit.name));
      this.notText2.SetText(string.Format("※同時にTalkできるのは、10名までです\n全ての誓約ミッションを完了させて\nTalkに空き枠を作ってから\n再度デートに誘ってください"));
      this.notObject.SetActive(true);
      this.yesNoObject.SetActive(false);
    }
    else if (callLetter == null || num1 < 10)
    {
      int num2 = 10 - num1;
      this.yesNoText1.SetText(string.Format("{0}と\nTalk IDを交換しますか？", (object) unit.unit.name));
      this.yesNoText2.SetText(string.Format("※同時にTalkできるのは、10名までです\n全ての誓約ミッションを完了させると\nその姫はこの10名から除外されます"));
      this.yesNoText3.SetText(string.Format("{0}", (object) num2));
      this.notObject.SetActive(false);
      this.yesNoObject.SetActive(true);
      yield break;
    }
  }

  public IEnumerator initialize(string title, string msg)
  {
    this.notTitle.SetTextLocalize(title);
    this.notText1.SetTextLocalize(msg);
    yield break;
  }

  private IEnumerator CallMakeApi()
  {
    Sea030CommonPopup sea030CommonPopup = this;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.SeaCallMakeletter> futureAPI = WebAPI.SeaCallMakeletter(sea030CommonPopup.unitData.id, new Action<WebAPI.Response.UserError>(sea030CommonPopup.\u003CCallMakeApi\u003Eb__13_0));
    IEnumerator e = futureAPI.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    if (futureAPI.Result != null)
    {
      if (futureAPI.Result.player_call_letters != null)
        Singleton<NGGameDataManager>.GetInstance().callLetter = futureAPI.Result.player_call_letters;
      if (sea030CommonPopup.message != null)
        sea030CommonPopup.message(futureAPI.Result.latest_talk_message);
      sea030CommonPopup.TalkIdExchangeText();
    }
  }

  private void TalkIdExchangeText()
  {
    this.notObject.SetActive(true);
    this.yesNoObject.SetActive(false);
    ((Component) this.notText1).transform.localPosition = new Vector3(((Component) this.notText1).transform.localPosition.x, 81f, ((Component) this.notText1).transform.localPosition.z);
    this.notText1.SetText(string.Format("{0}と\nTalk IDを交換しました", (object) this.unitData.unit.name));
    this.notText2.SetText("");
  }

  public IEnumerator PopupEnd()
  {
    while (!this.popupEnd)
      yield return (object) null;
  }

  public override void onBackButton()
  {
  }

  public void onOkButton()
  {
    this.popupEnd = true;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onYesButton() => this.StartCoroutine(this.CallMakeApi());

  public void onNoButton()
  {
    this.popupEnd = true;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
