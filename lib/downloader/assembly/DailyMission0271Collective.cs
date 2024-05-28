// Decompiled with JetBrains decompiler
// Type: DailyMission0271Collective
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class DailyMission0271Collective : BackButtonMonoBehaiviour
{
  [SerializeField]
  private Animator effectAnima;
  private DailyMissionWindow missionWindow;
  private bool popupView;

  public void IbtnNo()
  {
    if (this.popupView)
      return;
    this.popupView = true;
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.missionWindow.CollectivePopupEnd();
  }

  public void Initialize(DailyMissionWindow window)
  {
    this.missionWindow = window;
    this.popupView = false;
  }

  public override void onBackButton() => this.IbtnNo();

  protected override void Update()
  {
    base.Update();
    AnimatorStateInfo animatorStateInfo = this.effectAnima.GetCurrentAnimatorStateInfo(0);
    if ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime <= 4.0)
      return;
    this.IbtnNo();
  }

  private IEnumerator OpenResultPopup()
  {
    Future<GameObject> prefab = new ResourceObject("Prefabs/popup/popup_027_5_collective__anim_popup01").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab.Result);
  }
}
