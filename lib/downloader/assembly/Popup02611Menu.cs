// Decompiled with JetBrains decompiler
// Type: Popup02611Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Popup02611Menu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDescription;

  public void Init()
  {
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetText(instance.VERSUS_NOT_LATEST_VERSION_TITLE);
    this.txtDescription.SetText(instance.VERSUS_NOT_LATEST_VERSION_DESCRIPTION_PC);
  }

  public void IbtnOK()
  {
    MypageScene.ChangeSceneOnError();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnNo()
  {
    MypageScene.ChangeSceneOnError();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
