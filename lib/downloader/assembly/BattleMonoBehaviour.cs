// Decompiled with JetBrains decompiler
// Type: BattleMonoBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class BattleMonoBehaviour : MonoBehaviour
{
  public BattleMonoBehaviourStatus bmStatus = new BattleMonoBehaviourStatus();
  protected NGBattleManager battleManager;
  protected BE env_;
  private bool isDoStart;
  private bool asyncStart;
  private bool asyncInitialized;

  protected virtual IEnumerator Start_Original()
  {
    yield break;
  }

  protected virtual void Update_Original()
  {
  }

  protected virtual void LateUpdate_Original()
  {
  }

  protected virtual IEnumerator Start_Battle()
  {
    yield break;
  }

  protected virtual void Update_Battle()
  {
  }

  protected virtual void LateUpdate_Battle()
  {
  }

  protected BE env
  {
    get
    {
      if (this.env_ == null)
      {
        this.battleManager = Singleton<NGBattleManager>.GetInstance();
        if (Object.op_Inequality((Object) this.battleManager, (Object) null))
          this.env_ = this.battleManager.environment;
      }
      return this.env_;
    }
  }

  public void resetEnvironment() => this.env_ = (BE) null;

  public IEnumerator onBattleInitSceneAsync()
  {
    if (!this.asyncStart && !this.asyncInitialized)
    {
      this.asyncStart = true;
      this.battleManager = Singleton<NGBattleManager>.GetInstance();
      IEnumerator e = this.onInitAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.asyncInitialized = true;
      this.asyncStart = false;
    }
  }

  public virtual IEnumerator onInitAsync()
  {
    yield break;
  }

  private IEnumerator Start()
  {
    this.isDoStart = true;
    IEnumerator e = this.Start_Original();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    while (!this.bmStatus.IsBattleManagerInitialized() || this.asyncStart)
      yield return (object) null;
    if (!this.asyncInitialized)
    {
      e = this.onBattleInitSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = this.Start_Battle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.bmStatus.StartCompleted = true;
  }

  private void Update()
  {
    this.Update_Original();
    if (!this.bmStatus.IsBattleManagerCompleted())
      return;
    this.Update_Battle();
  }

  private void LateUpdate()
  {
    this.LateUpdate_Original();
    if (!this.bmStatus.IsBattleManagerCompleted())
      return;
    this.LateUpdate_Battle();
  }

  public bool isStartInitialized()
  {
    return !((Component) this).gameObject.activeSelf || !this.isDoStart || this.bmStatus.IsBattleManagerCompleted();
  }

  public bool isAsyncInitialized => this.asyncInitialized;
}
