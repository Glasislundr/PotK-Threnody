// Decompiled with JetBrains decompiler
// Type: Colosseum0236Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Colosseum0236Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtNum01;
  [SerializeField]
  protected UILabel TxtNum02;
  [SerializeField]
  protected UILabel TxtNum04;
  [SerializeField]
  protected UILabel TxtNum05;
  [SerializeField]
  protected UILabel TxtNum06;
  [SerializeField]
  protected UILabel TxtNum07;
  [SerializeField]
  protected UILabel TxtNum08;
  [SerializeField]
  protected UILabel TxtRankName;
  [SerializeField]
  protected UILabel TxtResultsNum;
  private ColosseumUtility.Info info;

  public override void onBackButton() => this.IbtnBack();

  public IEnumerator Initialize(ColosseumUtility.Info info)
  {
    Colosseum0236Menu colosseum0236Menu = this;
    colosseum0236Menu.info = info;
    ColosseumRecord colosseumRecord = info.colosseum_record;
    colosseum0236Menu.TxtRankName.SetText(ColosseumUtility.GetRankName(colosseumRecord.rank_pt));
    string text = string.Format(Consts.GetInstance().COLOSSEUM_RECODE_WINLOSE, (object) colosseumRecord.attack_win, (object) colosseumRecord.attack_lose);
    colosseum0236Menu.TxtResultsNum.SetTextLocalize(text);
    colosseum0236Menu.TxtNum01.SetTextLocalize(colosseumRecord.entry_count.ToLocalizeNumberText());
    colosseum0236Menu.TxtNum02.SetTextLocalize(colosseumRecord.total_win.ToLocalizeNumberText());
    colosseum0236Menu.TxtNum04.SetTextLocalize(colosseumRecord.attack_win.ToLocalizeNumberText());
    colosseum0236Menu.TxtNum05.SetTextLocalize(colosseumRecord.attack_max_consecutive_wins.ToLocalizeNumberText());
    colosseum0236Menu.TxtNum06.SetTextLocalize(colosseumRecord.attack_lose.ToLocalizeNumberText());
    colosseum0236Menu.TxtNum07.SetTextLocalize(colosseumRecord.defence_win.ToLocalizeNumberText());
    colosseum0236Menu.TxtNum08.SetTextLocalize(colosseumRecord.defence_max_consecutive_wins.ToLocalizeNumberText());
    IEnumerator e = Singleton<CommonRoot>.GetInstance().GetColosseumHeaderComponent().SetInfo(CommonColosseumHeader.BtnMode.Back, new Action(colosseum0236Menu.IbtnBack));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Colosseum0234Scene.ChangeScene(this.info);
  }
}
