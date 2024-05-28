// Decompiled with JetBrains decompiler
// Type: Colosseum02371Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Colosseum02371Menu : BackButtonMenuBase
{
  public NGxScroll ScrollContainer;
  public UIButton RankingBtn;
  public UIButton FriendRankibBtn;
  private GameObject ScrollObject;
  private GameObject TextObject;
  private GameObject IconObject;
  private RankingPlayer MyRanking;
  private RankingPlayer[] TotalRankingArray;
  private RankingPlayer[] FriendRankingArray;
  private List<Colosseum02371MenuParts> TotalRankingList = new List<Colosseum02371MenuParts>();
  private List<Colosseum02371MenuParts> FriendRankingList = new List<Colosseum02371MenuParts>();
  [SerializeField]
  private GameObject BottomParts;
  [SerializeField]
  private GameObject BottomDeco;
  private ColosseumUtility.Info info;
  private Colosseum02371Menu.RANKINGTYPE rankingType;
  private const int HIDE_ANCHOR = 0;
  private const int SHOW_ANCHOR = 207;

  public virtual void Foreground()
  {
  }

  public virtual void VScrollBar()
  {
  }

  public override void onBackButton() => this.IbtnBack();

  private IEnumerator DisplayRanking(
    RankingPlayer[] RankingData,
    List<Colosseum02371MenuParts> cacheList,
    RankingPlayer MyRanking)
  {
    this.ScrollContainer.Clear();
    IEnumerator e;
    if (RankingData.Length != 0)
    {
      RankingPlayer[] rankingPlayerArray = RankingData;
      for (int index = 0; index < rankingPlayerArray.Length; ++index)
      {
        RankingPlayer data = rankingPlayerArray[index];
        GameObject gameObject = this.ScrollObject.Clone();
        this.ScrollContainer.Add(gameObject);
        Colosseum02371MenuParts component = gameObject.GetComponent<Colosseum02371MenuParts>();
        component.Init(data);
        e = this.TextObject.Clone(component.GetTextDir().transform).GetComponent<Colosseum02371MenuTextParts>().Init(data, this.IconObject);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      rankingPlayerArray = (RankingPlayer[]) null;
      RankingPlayer rankingData = ((IEnumerable<RankingPlayer>) RankingData).FirstOrDefault<RankingPlayer>((Func<RankingPlayer, bool>) (x => x.player_id == MyRanking.player_id));
      if (rankingData == null)
      {
        e = this.InitBottomParts(MyRanking);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = this.InitBottomParts(rankingData);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else
    {
      GameObject gameObject = this.ScrollObject.Clone();
      this.ScrollContainer.Add(gameObject);
      Colosseum02371MenuParts component = gameObject.GetComponent<Colosseum02371MenuParts>();
      component.Init((RankingPlayer) null);
      e = this.TextObject.Clone(component.GetTextDir().transform).GetComponent<Colosseum02371MenuTextParts>().Init((RankingPlayer) null, this.IconObject);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.ScrollContainer.ResolvePosition();
  }

  public IEnumerator Initialize(
    RankingPlayer[] TotalRanking,
    RankingPlayer[] FriendRanking,
    RankingPlayer MyRanking,
    ColosseumUtility.Info colosseumInfo)
  {
    Colosseum02371Menu colosseum02371Menu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = Singleton<CommonRoot>.GetInstance().GetColosseumHeaderComponent().SetInfo(CommonColosseumHeader.BtnMode.Back, new Action(colosseum02371Menu.\u003CInitialize\u003Eb__20_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02371Menu.info = colosseumInfo;
    colosseum02371Menu.MyRanking = MyRanking;
    colosseum02371Menu.TotalRankingArray = TotalRanking;
    colosseum02371Menu.FriendRankingArray = FriendRanking;
    colosseum02371Menu.TotalRankingList.Clear();
    colosseum02371Menu.FriendRankingList.Clear();
    Future<GameObject> prefabF = Res.Prefabs.colosseum.colosseum023_7_1.vscroll_810_12.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02371Menu.ScrollObject = prefabF.Result;
    Future<GameObject> prefabTextF = Res.Prefabs.colosseum.colosseum023_7_1.vscroll_810_12_text.Load<GameObject>();
    e = prefabTextF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02371Menu.TextObject = prefabTextF.Result;
    Future<GameObject> prefabIconF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = prefabIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02371Menu.IconObject = prefabIconF.Result;
    e = colosseum02371Menu.DisplayRanking(colosseum02371Menu.TotalRankingArray, colosseum02371Menu.TotalRankingList, colosseum02371Menu.MyRanking);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02371Menu.rankingType = Colosseum02371Menu.RANKINGTYPE.RANKING;
    ((Component) colosseum02371Menu.RankingBtn).GetComponent<SpreadColorButton>().SetColor(new Color(0.5f, 0.5f, 0.5f));
    ((Component) colosseum02371Menu.FriendRankibBtn).GetComponent<SpreadColorButton>().SetColor(new Color(0.25f, 0.25f, 0.25f));
  }

  private IEnumerator InitBottomParts(RankingPlayer rankingData)
  {
    if (rankingData == null || rankingData.ranking <= 0)
    {
      this.DispBottomParts(false);
    }
    else
    {
      this.DispBottomParts(true);
      Colosseum02371MenuParts component = this.BottomParts.GetComponent<Colosseum02371MenuParts>();
      component.Init(rankingData);
      Colosseum02371MenuTextParts colosseum02371MenuTextParts = component.GetTextDir().GetComponentInChildren<Colosseum02371MenuTextParts>();
      if (Object.op_Equality((Object) colosseum02371MenuTextParts, (Object) null))
        colosseum02371MenuTextParts = this.TextObject.Clone(component.GetTextDir().transform).GetComponent<Colosseum02371MenuTextParts>();
      IEnumerator e = colosseum02371MenuTextParts.Init(rankingData, this.IconObject);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void DispBottomParts(bool canDisp)
  {
    this.BottomParts.SetActive(canDisp);
    this.BottomDeco.SetActive(canDisp);
    UIWidget component = ((Component) this.ScrollContainer).GetComponent<UIWidget>();
    if (Object.op_Equality((Object) component, (Object) null))
      Debug.LogError((object) "DispBottomParts ScrollContainerにUIWidgetがありません");
    else if (canDisp)
      ((UIRect) component).bottomAnchor.absolute = 207;
    else
      ((UIRect) component).bottomAnchor.absolute = 0;
  }

  private IEnumerator DisplayTotalRanking()
  {
    IEnumerator e = this.DisplayRanking(this.TotalRankingArray, this.TotalRankingList, this.MyRanking);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void IbtnRanking()
  {
    if (this.rankingType == Colosseum02371Menu.RANKINGTYPE.RANKING)
      return;
    ((Component) this.RankingBtn).GetComponent<SpreadColorButton>().SetColor(new Color(0.5f, 0.5f, 0.5f));
    ((Component) this.FriendRankibBtn).GetComponent<SpreadColorButton>().SetColor(new Color(0.25f, 0.25f, 0.25f));
    this.StartCoroutine(this.DisplayTotalRanking());
    this.rankingType = Colosseum02371Menu.RANKINGTYPE.RANKING;
  }

  private IEnumerator DisplayFriendRanking()
  {
    IEnumerator e = this.DisplayRanking(this.FriendRankingArray, this.FriendRankingList, this.MyRanking);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void IbtnRankingFriend()
  {
    if (this.rankingType == Colosseum02371Menu.RANKINGTYPE.FRIENDRANKING)
      return;
    ((Component) this.RankingBtn).GetComponent<SpreadColorButton>().SetColor(new Color(0.25f, 0.25f, 0.25f));
    ((Component) this.FriendRankibBtn).GetComponent<SpreadColorButton>().SetColor(new Color(0.5f, 0.5f, 0.5f));
    this.StartCoroutine(this.DisplayFriendRanking());
    this.rankingType = Colosseum02371Menu.RANKINGTYPE.FRIENDRANKING;
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Colosseum0234Scene.ChangeScene(this.info);
  }

  private enum RANKINGTYPE
  {
    RANKING,
    FRIENDRANKING,
  }
}
