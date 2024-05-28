// Decompiled with JetBrains decompiler
// Type: Versus02622Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02622Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel txt_TitleString;
  [SerializeField]
  protected UILabel txt_RecordString01;
  [SerializeField]
  protected UILabel txt_RecordString02;
  [SerializeField]
  protected UILabel txt_RecordString03;
  [SerializeField]
  protected UILabel txt_RecordString04;
  [SerializeField]
  protected UILabel txt_RecordString05;
  [SerializeField]
  protected UILabel txt_RecordString06;
  [SerializeField]
  protected UILabel txt_RecordString07;
  [SerializeField]
  protected UILabel txt_RecordString08;
  [SerializeField]
  protected UILabel txt_RecordString09;
  [SerializeField]
  protected UILabel txt_RecordString10;
  [SerializeField]
  protected UILabel txt_RecordString11;
  [SerializeField]
  protected UILabel txt_RandomRecordNum01;
  [SerializeField]
  protected UILabel txt_RandomRecordNum02;
  [SerializeField]
  protected UILabel txt_RandomRecordNum03;
  [SerializeField]
  protected UILabel txt_RandomRecordNum04;
  [SerializeField]
  protected UILabel txt_RandomRecordNum05;
  [SerializeField]
  protected UILabel txt_RandomRecordNum06;
  [SerializeField]
  protected UILabel txt_RandomRecordNum07;
  [SerializeField]
  protected UILabel txt_RandomRecordNum08;
  [SerializeField]
  protected UILabel txt_RandomRecordNum09;
  [SerializeField]
  protected UILabel txt_RandomRecordNum10;
  [SerializeField]
  protected UILabel txt_RandomRecordNum11;
  [SerializeField]
  protected UILabel txt_FriendRecordNum01;
  [SerializeField]
  protected UILabel txt_FriendRecordNum02;
  [SerializeField]
  protected UILabel txt_FriendRecordNum03;
  [SerializeField]
  protected UILabel txt_FriendRecordNum04;
  [SerializeField]
  protected UILabel txt_FriendRecordNum05;
  [SerializeField]
  protected UILabel txt_FriendRecordNum06;
  [SerializeField]
  protected UILabel txt_FriendRecordNum07;
  [SerializeField]
  protected UILabel txt_FriendRecordNum08;
  [SerializeField]
  protected UILabel txt_FriendRecordNum09;
  [SerializeField]
  protected UILabel txt_FriendRecordNum10;
  [SerializeField]
  protected UILabel txt_FriendRecordNum11;

  public IEnumerator Initialize(PvPRecord randomInfo, PvPRecord friendInfo)
  {
    this.txt_TitleString.text = Consts.GetInstance().PVP_RECORD_TITLE;
    this.txt_RecordString01.text = Consts.GetInstance().PVP_RECORD_ENTRY;
    this.txt_RecordString02.text = Consts.GetInstance().PVP_RECORD_WIN;
    this.txt_RecordString03.text = Consts.GetInstance().PVP_RECORD_MAX_CONSECUTIVE_WIN;
    this.txt_RecordString04.text = Consts.GetInstance().PVP_RECORD_CURRENT_CONSECUTIVE_WIN;
    this.txt_RecordString05.text = Consts.GetInstance().PVP_RECORD_LOSS;
    this.txt_RecordString06.text = Consts.GetInstance().PVP_RECORD_CURRENT_CONSECUTIVE_LOSS;
    this.txt_RecordString07.text = Consts.GetInstance().PVP_RECORD_DRAW;
    this.txt_RecordString08.text = Consts.GetInstance().PVP_RECORD_DISCONNECTED;
    this.txt_RecordString09.text = Consts.GetInstance().PVP_RECORD_EXCELLENT_WIN;
    this.txt_RecordString10.text = Consts.GetInstance().PVP_RECORD_GREAT_WIN;
    this.txt_RecordString11.text = Consts.GetInstance().PVP_RECORD_POINT_WIN;
    this.txt_RandomRecordNum01.SetTextLocalize(randomInfo.entry);
    this.txt_RandomRecordNum02.SetTextLocalize(randomInfo.win);
    this.txt_RandomRecordNum03.SetTextLocalize(randomInfo.max_consecutive_win);
    this.txt_RandomRecordNum04.SetTextLocalize(randomInfo.current_consecutive_win);
    this.txt_RandomRecordNum05.SetTextLocalize(randomInfo.loss);
    this.txt_RandomRecordNum06.SetTextLocalize(randomInfo.current_consecutive_loss);
    this.txt_RandomRecordNum07.SetTextLocalize(randomInfo.draw);
    this.txt_RandomRecordNum08.SetTextLocalize(randomInfo.disconnected);
    this.txt_RandomRecordNum09.SetTextLocalize(randomInfo.excellent_win);
    this.txt_RandomRecordNum10.SetTextLocalize(randomInfo.great_win);
    this.txt_RandomRecordNum11.SetTextLocalize(randomInfo.point_win);
    this.txt_FriendRecordNum01.SetTextLocalize(friendInfo.entry);
    this.txt_FriendRecordNum02.SetTextLocalize(friendInfo.win);
    this.txt_FriendRecordNum03.SetTextLocalize(friendInfo.max_consecutive_win);
    this.txt_FriendRecordNum04.SetTextLocalize(friendInfo.current_consecutive_win);
    this.txt_FriendRecordNum05.SetTextLocalize(friendInfo.loss);
    this.txt_FriendRecordNum06.SetTextLocalize(friendInfo.current_consecutive_loss);
    this.txt_FriendRecordNum07.SetTextLocalize(friendInfo.draw);
    this.txt_FriendRecordNum08.SetTextLocalize(friendInfo.disconnected);
    this.txt_FriendRecordNum09.SetTextLocalize(friendInfo.excellent_win);
    this.txt_FriendRecordNum10.SetTextLocalize(friendInfo.great_win);
    this.txt_FriendRecordNum11.SetTextLocalize(friendInfo.point_win);
    yield break;
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
