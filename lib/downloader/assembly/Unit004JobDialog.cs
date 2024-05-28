// Decompiled with JetBrains decompiler
// Type: Unit004JobDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004JobDialog : BackButtonMenuBase
{
  [SerializeField]
  private Transform dirJobAfter;
  [SerializeField]
  private GameObject objClose;

  public IEnumerator Init(PlayerUnitJob_abilities jobAbility)
  {
    Future<GameObject> JobAfterPanelF = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.unit004_Job.Unit_job_after_sea.Load<GameObject>() : Res.Prefabs.unit004_Job.Unit_job_after.Load<GameObject>();
    IEnumerator e = JobAfterPanelF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject obj = JobAfterPanelF.Result.Clone(this.dirJobAfter);
    e = obj.GetComponent<Unit004JobAfter>().Init(2, jobAbility, bActiveSkillZoom: true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) obj.GetComponentInChildren<Collider>(true)).GetComponent<UIWidget>().depth += this.objClose.GetComponent<UIWidget>().depth;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
