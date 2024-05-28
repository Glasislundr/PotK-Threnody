// Decompiled with JetBrains decompiler
// Type: CoinLimitList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class CoinLimitList : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite dynIcon;
  [SerializeField]
  private UILabel txtCoinName;
  [SerializeField]
  private UILabel txtProssessionValue;
  [SerializeField]
  private UILabel txt_Limit_Value;
  private string strRemainingdays = "あと{0}日 [ffff00]";

  public IEnumerator Init(PlayerCommonTicket ticket, TimeSpan remainingdays)
  {
    CommonTicket commonTicket = MasterData.CommonTicket[ticket.ticket_id];
    this.txtCoinName.SetTextLocalize(commonTicket.name);
    this.txt_Limit_Value.SetTextLocalize(this.strRemainingdays.F((object) remainingdays.Days));
    this.txtProssessionValue.SetTextLocalize(ticket.quantity);
    Future<Sprite> future = commonTicket.LoadIconMSpriteF();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dynIcon.sprite2D = future.Result;
    yield return (object) null;
  }
}
