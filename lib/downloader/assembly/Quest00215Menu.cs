// Decompiled with JetBrains decompiler
// Type: Quest00215Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Quest00215Menu : NGMenuBase
{
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private UI2DSprite DynCharacter;
  [SerializeField]
  protected UILabel TxtAp;
  [SerializeField]
  protected UILabel TxtEpisodetitle;
  [SerializeField]
  protected UILabel TxtTitle;

  public virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  public virtual void IbtnBack()
  {
    Debug.Log((object) "click default event IbtnBack");
    this.backScene();
  }

  public virtual void IbtnEpisode() => Debug.Log((object) "click default event IbtnEpisode");

  public virtual void IbtnEpisodeBlock()
  {
    Debug.Log((object) "click default event IbtnEpisodeBlock");
  }

  public virtual void IbtnFriendly()
  {
    Debug.Log((object) "click default event IbtnFriendly");
    UnitUnit unitUnit = MasterData.UnitUnit[new List<int>((IEnumerable<int>) MasterData.UnitUnit.Keys)[0]];
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_2_2", true, (object) unitUnit);
  }

  public virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");

  public void ScrollContainerResolvePosition() => this.ScrollContainer.ResolvePosition();

  private IEnumerator SetCharacterLargeImage(int id)
  {
    UnitUnit unit = MasterData.UnitUnit[id];
    Future<GameObject> goFuture = unit.LoadMypage();
    IEnumerator e = goFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit.SetLargeSpriteSnap(goFuture.Result.Clone(((Component) this.DynCharacter).transform), 5);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator CreateEpisodes(int id)
  {
    Future<GameObject> prefabF = Res.Prefabs.quest002_15.vscroll_520_0.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    for (int id1 = 0; id1 < 20; ++id1)
    {
      GameObject gameObject = result.Clone();
      this.ScrollContainer.Add(gameObject);
      gameObject.GetComponent<Quest00215DirEpisode>().setData(id1, true);
    }
    for (int id2 = 0; id2 < 20; ++id2)
    {
      GameObject gameObject = result.Clone();
      this.ScrollContainer.Add(gameObject);
      gameObject.GetComponent<Quest00215DirEpisode>().setData(id2, false);
    }
  }

  public IEnumerator Init(int id)
  {
    this.TxtTitle.SetTextLocalize(MasterData.UnitUnit[id].name);
    IEnumerator e = this.SetCharacterLargeImage(id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollContainer.Clear();
    e = this.CreateEpisodes(id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollContainer.ResolvePosition();
  }
}
