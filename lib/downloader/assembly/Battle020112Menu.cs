// Decompiled with JetBrains decompiler
// Type: Battle020112Menu
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
public class Battle020112Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtCharaname22;
  [SerializeField]
  protected UILabel TxtDescription20;
  [SerializeField]
  protected UILabel TxtDescription224;
  [SerializeField]
  protected UILabel TxtPopuptitle26;
  private const float LINK_WIDTH = 136f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 1f;
  [SerializeField]
  private GameObject linkChar;
  private Action onCallback;
  private bool currentScenePush;

  public void Init(int deckno, int charno, bool push = false)
  {
    PlayerDeck playerDeck = SMManager.Get<PlayerDeck[]>()[deckno];
    this.currentScenePush = push;
    this.StartCoroutine(this.SetUnitPrefab(playerDeck.player_units[charno]));
  }

  public void Init(int unitId, bool push = false)
  {
    this.currentScenePush = push;
    this.StartCoroutine(this.SetUnitPrefab(MasterData.UnitUnit[unitId]));
  }

  public IEnumerator Init(QuestCharacterS quest, bool push = false)
  {
    this.SetText(quest.unit.name, quest.priority, quest.name);
    this.currentScenePush = push;
    IEnumerator e = this.SetUnitPrefab(quest.unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetText(string name, int episodeNo, string episodeTitle)
  {
    this.TxtCharaname22.SetTextLocalize(name);
    this.TxtDescription20.SetTextLocalize("Episode" + (object) episodeNo);
    this.TxtDescription224.SetTextLocalize(episodeTitle);
  }

  public IEnumerator SetUnitPrefab(PlayerUnit unit)
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = prefabF.Result.Clone(this.linkChar.transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    UnitIcon component = gameObject.GetComponent<UnitIcon>();
    PlayerUnit[] playerUnits = new PlayerUnit[1]{ unit };
    e = component.SetPlayerUnit(unit, playerUnits, (PlayerUnit) null, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetUnitPrefab(UnitUnit unit)
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = prefabF.Result.Clone(this.linkChar.transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    e = unitScript.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.BottomModeValue = UnitIconBase.BottomMode.Nothing;
  }

  public virtual void PopUpTap()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss(this.currentScenePush);
    if (this.onCallback == null)
      return;
    this.onCallback();
  }

  public void SetCallback(Action callback) => this.onCallback = callback;

  public override void onBackButton() => this.PopUpTap();
}
