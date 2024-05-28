// Decompiled with JetBrains decompiler
// Type: APRecoveryResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class APRecoveryResult : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription02;
  [SerializeField]
  private UILabel TxtPopuptitle;
  private Action btnAct;
  private List<string> reloadShop = new List<string>()
  {
    "shop007_4",
    "shop007_4_1",
    "shop007_4_3",
    "raid032_shop",
    "guild028_shop"
  };

  public IEnumerator Init(int before_ap, int after_ap, Action questChangeScene)
  {
    this.TxtDescription02.SetTextLocalize(before_ap.ToLocalizeNumberText() + "→[ffff00]" + after_ap.ToLocalizeNumberText());
    this.SetBtnAct(questChangeScene);
    yield break;
  }

  public void SetBtnAct(Action questChangeScene) => this.btnAct = questChangeScene;

  public void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (this.btnAct == null)
      return;
    this.btnAct();
    if (!Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null))
      return;
    string sceneName = Singleton<NGSceneManager>.GetInstance().sceneBase.sceneName;
    if (!this.reloadShop.Contains(sceneName))
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, false);
  }

  public override void onBackButton() => this.IbtnOk();
}
