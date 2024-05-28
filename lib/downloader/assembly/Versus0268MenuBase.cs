// Decompiled with JetBrains decompiler
// Type: Versus0268MenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Versus0268MenuBase : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtAttention1;
  [SerializeField]
  protected UILabel TxtAttentionNum;
  [SerializeField]
  protected UILabel TxtCharaEXP26;
  [SerializeField]
  protected UILabel TxtDraw;
  [SerializeField]
  protected UILabel TxtLeague;
  [SerializeField]
  protected UILabel TxtLose;
  [SerializeField]
  protected UILabel TxtRank;
  [SerializeField]
  protected UILabel TxtRemain;
  [SerializeField]
  protected UILabel TxtRemainNum;
  [SerializeField]
  protected UILabel TxtResultTitleVersus30;
  [SerializeField]
  protected UILabel TxtSeason;
  [SerializeField]
  protected UILabel TxtWin;
  [SerializeField]
  protected UILabel TxtWinPoint;

  public virtual void IbtnReplay() => Debug.Log((object) "click default event IbtnReplay");

  public virtual void SlcTouchToNext() => Debug.Log((object) "click default event SlcTouchToNext");
}
