// Decompiled with JetBrains decompiler
// Type: Quest002171Menu
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
[AddComponentMenu("Scenes/QuestExtra/LocksMenu")]
public class Quest002171Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  public UIScrollView scrollview;
  public UIGrid grid;
  private DateTime serverTime;

  public IEnumerator Init(PlayerQuestGate[] gates)
  {
    this.TxtTitle.SetText(this.GetTitle());
    foreach (Component component in ((Component) this.grid).transform)
      Object.Destroy((Object) component.gameObject);
    ((Component) this.grid).gameObject.SetActive(false);
    Future<GameObject> ScrollPrefab = Res.Prefabs.quest002_17_1.scroll.Load<GameObject>();
    IEnumerator e = ScrollPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = ScrollPrefab.Result;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.serverTime = ServerTime.NowAppTime();
    PlayerExtraQuestS[] extra = SMManager.Get<PlayerExtraQuestS[]>();
    Func<PlayerExtraQuestS[], int, int?> GetLid = (Func<PlayerExtraQuestS[], int, int?>) ((ex, id_s) => ((IEnumerable<PlayerExtraQuestS>) ex).FirstOrDefault<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (fd => fd._quest_extra_s == id_s))?.quest_extra_s.quest_l_QuestExtraL);
    IEnumerable<PlayerQuestGate> _gates = ((IEnumerable<PlayerQuestGate>) gates).Where<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => ((IEnumerable<int>) x.quest_ids).Any<int>((Func<int, bool>) (y => ((IEnumerable<PlayerExtraQuestS>) extra).Any<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (z => z._quest_extra_s == y)))))).Where<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (w =>
    {
      IEnumerable<int> idGateS = ((IEnumerable<PlayerQuestGate>) gates).SelectMany<PlayerQuestGate, int>((Func<PlayerQuestGate, IEnumerable<int>>) (s => (IEnumerable<int>) s.quest_ids));
      int? nullable = GetLid(extra, w.quest_ids[0]);
      return nullable.HasValue && ((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) extra).M(nullable.Value)).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (extra_s => !idGateS.Contains<int>(extra_s.quest_extra_s.ID))).Count<PlayerExtraQuestS>() == 0;
    }));
    Dictionary<int, bool> inProgressQuests = _gates.Aggregate<PlayerQuestGate, Dictionary<int, bool>>(new Dictionary<int, bool>(), (Func<Dictionary<int, bool>, PlayerQuestGate, Dictionary<int, bool>>) ((acc, quest) =>
    {
      if (quest.in_progress)
        acc[quest.quest_key_id] = true;
      return acc;
    }));
    foreach (int num in _gates.Distinct<PlayerQuestGate>((IEqualityComparer<PlayerQuestGate>) new LambdaEqualityComparer<PlayerQuestGate>((Func<PlayerQuestGate, PlayerQuestGate, bool>) ((a, b) => a.quest_key_id == b.quest_key_id))).OrderByDescending<PlayerQuestGate, bool>((Func<PlayerQuestGate, bool>) (x => inProgressQuests.ContainsKey(x.quest_key_id))).ThenBy<PlayerQuestGate, int>((Func<PlayerQuestGate, int>) (x => MasterData.QuestkeyQuestkey[x.quest_key_id].priority)).ThenBy<PlayerQuestGate, int>((Func<PlayerQuestGate, int>) (y => y.quest_key_id)).Select<PlayerQuestGate, int>((Func<PlayerQuestGate, int>) (z => z.quest_key_id)))
    {
      int keyKind = num;
      e = this.ScrollInit(_gates.Where<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => x.quest_key_id == keyKind)).ToArray<PlayerQuestGate>(), prefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    yield return (object) null;
    this.scrollview.ResetPosition();
    ((Component) this.grid).gameObject.SetActive(true);
    this.grid.Reposition();
    this.scrollview.ResetPosition();
  }

  public IEnumerator UpdateTime()
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.serverTime = ServerTime.NowAppTime();
    foreach (Quest00217Scroll componentsInChild in ((Component) ((Component) this.grid).transform).GetComponentsInChildren<Quest00217Scroll>())
      componentsInChild.SetTime(this.serverTime, componentsInChild.RankingEventTerm);
  }

  private IEnumerator ScrollInit(PlayerQuestGate[] gates, GameObject prefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    gameObject.transform.parent = ((Component) this.grid).transform;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localPosition = Vector3.zero;
    IEnumerator e = gameObject.GetComponent<Quest002171Scroll>().InitScroll(gates, this.serverTime);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnEvent() => Debug.Log((object) "click default event IbtnEvent");

  public virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");

  private string GetTitle() => Consts.GetInstance().QUEST_00217_KEY_TITLE;
}
