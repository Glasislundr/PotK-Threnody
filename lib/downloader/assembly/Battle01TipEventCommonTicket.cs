// Decompiled with JetBrains decompiler
// Type: Battle01TipEventCommonTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01TipEventCommonTicket : Battle01TipEventBase
{
  private UniqueIcons icon;

  public override IEnumerator onInitAsync()
  {
    Battle01TipEventCommonTicket eventCommonTicket = this;
    Future<GameObject> f = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    eventCommonTicket.icon = eventCommonTicket.cloneIcon<UniqueIcons>(f.Result);
    ((Component) eventCommonTicket.icon).gameObject.SetActive(false);
    eventCommonTicket.disableAllIcon();
  }

  public override void setData(BL.DropData e, BL.Unit unit)
  {
    if (e.reward.Type != MasterDataTable.CommonRewardType.common_ticket)
      return;
    this.setText(Consts.GetInstance().TipEvent_text_common_ticket.F((object) MasterData.CommonTicket[e.reward.Id].name, (object) e.reward.Quantity));
    this.StartCoroutine(this.setIcon(e.reward.Id));
  }

  private IEnumerator setIcon(int id)
  {
    Battle01TipEventCommonTicket eventCommonTicket = this;
    IEnumerator e = eventCommonTicket.icon.SetCommonTicket(id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) eventCommonTicket.icon).gameObject.SetActive(true);
    eventCommonTicket.icon.BackGroundActivated = false;
    eventCommonTicket.icon.LabelActivated = false;
    eventCommonTicket.selectIcon(0);
  }
}
