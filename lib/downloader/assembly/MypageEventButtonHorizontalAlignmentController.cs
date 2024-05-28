// Decompiled with JetBrains decompiler
// Type: MypageEventButtonHorizontalAlignmentController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class MypageEventButtonHorizontalAlignmentController : MypageEventButtonController
{
  private const int SPACE_X = -106;
  private GameObject hotDealIconPrefab;
  private HotDealButton hotDealButton;

  private static Type FindComponent<Type>(Transform trans, string path)
  {
    Transform transform = trans.Find(path);
    return Object.op_Equality((Object) transform, (Object) null) ? default (Type) : ((Component) transform).GetComponent<Type>();
  }

  public IEnumerator InitSceneAsync()
  {
    IEnumerator e = this.InitHotDealIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitHotDealIcon()
  {
    MypageEventButtonHorizontalAlignmentController alignmentController = this;
    Transform component = MypageEventButtonHorizontalAlignmentController.FindComponent<Transform>(((Component) alignmentController).transform, "ibtn_HotDeal");
    if (Object.op_Inequality((Object) component, (Object) null))
      Object.Destroy((Object) ((Component) component).gameObject);
    IEnumerator e = alignmentController.LoadHotDealIconPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = alignmentController.hotDealIconPrefab.Clone(((Component) alignmentController).transform);
    ((Object) gameObject).name = "ibtn_HotDeal";
    alignmentController.hotDealButton = gameObject.GetComponent<HotDealButton>();
  }

  private IEnumerator LoadHotDealIconPrefab()
  {
    Future<GameObject> r = new ResourceObject("Prefabs/HotDeal/ibtn_HotDeal").Load<GameObject>();
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) r.Result, (Object) null))
      this.hotDealIconPrefab = r.Result;
  }

  public void StartIconEffect()
  {
    if (Object.op_Equality((Object) this.hotDealButton, (Object) null))
      return;
    this.hotDealButton.StartEffect();
  }

  private void Update() => this.AlignmentPosition();

  private void AlignmentPosition()
  {
    Func<MypageEventButton, float, float> func = (Func<MypageEventButton, float, float>) ((btn, spaceX) =>
    {
      if (Object.op_Equality((Object) btn, (Object) null) || !((Component) btn).gameObject.activeSelf)
        return spaceX;
      ((Component) btn).transform.localPosition = new Vector3(spaceX, ((Component) btn).transform.localPosition.y, ((Component) btn).transform.localPosition.z);
      return spaceX - 106f;
    });
    float num1 = 0.0f;
    float num2 = func(this.GetButton<TotalPaymentBonusButton>(), num1);
    float num3 = func(this.GetButton<RouletteButton>(), num2);
    float num4 = func(this.GetButton<HotDealButton>(), num3);
  }

  public override void UpdateButtonState()
  {
    base.UpdateButtonState();
    this.AlignmentPosition();
  }
}
