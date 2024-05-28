// Decompiled with JetBrains decompiler
// Type: GuildRaidUnitWanted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class GuildRaidUnitWanted : MonoBehaviour
{
  public const int hpRateMax = 10000;
  [SerializeField]
  private GameObject slcUnitWantedBaseClear;
  [SerializeField]
  private GameObject slcUnitWantedBase;
  [SerializeField]
  private GameObject dynRaidBaseBoss;
  [SerializeField]
  private GameObject slcUnlockCircle;
  [SerializeField]
  private GameObject slcLockCircle;
  [SerializeField]
  private GameObject dirHpGauge;
  [SerializeField]
  private NGTweenGaugeScale slcHpGauge;
  private Action tapAction;
  [SerializeField]
  private UIButton ibtnDetail;
  [SerializeField]
  private GameObject dynBossDefeatAnim;
  [SerializeField]
  private TweenAlpha dirTweenAlpha;
  [SerializeField]
  private GameObject slcTextRaidClearBase;
  [SerializeField]
  private GameObject slcTextRaidClear;
  private bool isCurrent;

  public GameObject DynRaidBaseBoss => this.dynRaidBaseBoss;

  public bool isDefeatAnimEnd { get; set; }

  public IEnumerator Init(GuildRaid info, Action action)
  {
    this.tapAction = action;
    yield break;
  }

  public void DispDetail()
  {
    if (!this.isCurrent)
      return;
    this.tapAction();
  }

  public void setCurrent()
  {
    this.isCurrent = true;
    this.setUnlockCircle(true);
  }

  public void setClear()
  {
    this.slcUnitWantedBaseClear.SetActive(true);
    this.slcUnitWantedBase.SetActive(false);
    this.isCurrent = false;
  }

  public void playClearAnim()
  {
    this.setClear();
    this.playAllTween(this.slcTextRaidClearBase);
    this.playAllTween(this.slcTextRaidClear);
  }

  private void playAllTween(GameObject obj)
  {
    foreach (UITweener component in obj.GetComponents<UITweener>())
      component.PlayForward();
  }

  public void setUnlockCircle(bool flag)
  {
    this.slcUnlockCircle.SetActive(flag);
    this.slcLockCircle.SetActive(!flag);
  }

  public void setHpGaugeEnable(bool flag, int damageRatio = 10000)
  {
    this.dirHpGauge.SetActive(flag);
    damageRatio = Mathf.Min(damageRatio, 10000);
    if (!flag)
      return;
    this.slcHpGauge.setValue(10000 - damageRatio, 10000);
  }

  public void startBossDefeatAnim(int waitMilliseconds)
  {
    this.isDefeatAnimEnd = false;
    this.StartCoroutine(this.playBossDefeatAnim(waitMilliseconds));
  }

  public IEnumerator playBossDefeatAnim(int waitMilliseconds)
  {
    this.dynBossDefeatAnim.SetActive(true);
    Future<GameObject> loader = new ResourceObject("Prefabs/raid032_top/RaidBase_boss_defeat_anim").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    loader.Result.Clone(this.dynBossDefeatAnim.transform);
    Stopwatch sw = new Stopwatch();
    sw.Start();
    while (sw.ElapsedMilliseconds < (long) waitMilliseconds)
    {
      if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
      {
        sw.Stop();
        this.disableBossDefeatAnim();
        yield return (object) null;
        this.isDefeatAnimEnd = true;
        yield break;
      }
      else
        yield return (object) null;
    }
    sw.Stop();
    this.disableBossDefeatAnim();
    this.isDefeatAnimEnd = true;
  }

  public void disableBossDefeatAnim()
  {
    this.dynBossDefeatAnim.transform.Clear();
    this.dynBossDefeatAnim.SetActive(false);
  }

  public void alphaIn() => ((UITweener) this.dirTweenAlpha).PlayForward();

  public void alphaInSkip()
  {
    this.dirTweenAlpha.from = 1f;
    this.dirTweenAlpha.to = 1f;
    ((UITweener) this.dirTweenAlpha).PlayForward();
  }

  public void setButtonEnabled(bool value) => ((Behaviour) this.ibtnDetail).enabled = value;
}
