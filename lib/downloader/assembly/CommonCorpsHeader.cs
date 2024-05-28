// Decompiled with JetBrains decompiler
// Type: CommonCorpsHeader
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
[AddComponentMenu("NGCommon/Header/Corps")]
public class CommonCorpsHeader : CommonHeaderBase
{
  [SerializeField]
  private UILabel lblPlayerName;
  [SerializeField]
  private UILabel lblStoneNum;
  [SerializeField]
  private GameObject kisekiBikkuriIcon;
  [SerializeField]
  private UILabel lblCorpsMedalNum;
  [SerializeField]
  private UI2DSprite corpsMedalIcon;
  [SerializeField]
  private GameObject objAreaName;
  [SerializeField]
  private UILabel lblAreaName;
  private int commonTicketId;
  private Modified<PlayerCommonTicket[]> ticketObserver;

  private void Start() => this.Init();

  protected override void Update()
  {
    if (this.player.Value == null)
      return;
    base.Update();
    if (this.isChangedOncePlayer)
      this.SetStone(this.player.Value.coin);
    if (this.ticketObserver == null || !this.ticketObserver.IsChangedOnce())
      return;
    this.UpdateCorpsMedal();
  }

  public IEnumerator SetInfo(int ticketId)
  {
    this.commonTicketId = ticketId;
    this.lblPlayerName.SetTextLocalize(SMManager.Get<Player>().name);
    this.SetAreaName(string.Empty);
    IEnumerator e = this.LoadCoinIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.UpdateInfo();
    this.ticketObserver = SMManager.Observe<PlayerCommonTicket[]>();
    this.ticketObserver.Commit();
  }

  public void UpdateInfo()
  {
    this.UpdateCorpsMedal();
    this.UpdateBikkuriIcon();
  }

  private void UpdateBikkuriIcon()
  {
    this.kisekiBikkuriIcon.SetActive(Singleton<NGGameDataManager>.GetInstance().receivableGift);
  }

  private void UpdateCorpsMedal()
  {
    PlayerCommonTicket playerCommonTicket = ((IEnumerable<PlayerCommonTicket>) SMManager.Get<PlayerCommonTicket[]>()).FirstOrDefault<PlayerCommonTicket>((Func<PlayerCommonTicket, bool>) (x => x.ticket_id == this.commonTicketId));
    if (playerCommonTicket != null)
      this.lblCorpsMedalNum.SetTextLocalize(Mathf.Min(playerCommonTicket.quantity, Consts.GetInstance().SUBCOIN_DISP_MAX));
    else
      this.lblCorpsMedalNum.SetTextLocalize(0);
  }

  private void SetStone(int val) => this.lblStoneNum.SetTextLocalize(val);

  public void SetAreaName(string area)
  {
    if (!string.IsNullOrEmpty(area))
    {
      this.objAreaName.SetActive(true);
      this.lblAreaName.SetTextLocalize(area);
    }
    else
      this.objAreaName.SetActive(false);
  }

  private IEnumerator LoadCoinIcon()
  {
    CommonTicket commonTicket;
    if (MasterData.CommonTicket.TryGetValue(this.commonTicketId, out commonTicket))
    {
      string path = string.Format("Coin/{0}/Coin_S", (object) (commonTicket.icon_id != 0 ? commonTicket.icon_id : commonTicket.ID));
      Future<Sprite> future = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.corpsMedalIcon.sprite2D = future.Result;
      ((Component) this.corpsMedalIcon).gameObject.SetActive(true);
      future = (Future<Sprite>) null;
    }
    else
      ((Component) this.corpsMedalIcon).gameObject.SetActive(false);
  }
}
