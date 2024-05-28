// Decompiled with JetBrains decompiler
// Type: PopupWithThumNoYes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupWithThumNoYes : PopupCommonNoYes
{
  [SerializeField]
  private CreateIconObject createIcon;

  public IEnumerator Initialize(
    string title,
    string message,
    MasterDataTable.CommonRewardType rewardType,
    int rewardId,
    int quantity = 0,
    Action yes = null,
    Action no = null)
  {
    PopupWithThumNoYes popupWithThumNoYes = this;
    ((Component) popupWithThumNoYes).transform.localScale = Vector3.zero;
    popupWithThumNoYes.title.SetTextLocalize(title);
    popupWithThumNoYes.message.SetTextLocalize(message);
    popupWithThumNoYes.SetDelegate(yes, no);
    IEnumerator e = popupWithThumNoYes.createIcon.CreateThumbnail(rewardType, rewardId, quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
