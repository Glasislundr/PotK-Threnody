// Decompiled with JetBrains decompiler
// Type: Friend00810Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Friend00810Scene : NGSceneBase
{
  [SerializeField]
  private UILabel TxtId;
  [SerializeField]
  private UILabel InpFriendid;
  [SerializeField]
  private Friend00810Menu menu;
  [SerializeField]
  private GameObject IbtnBnr;
  [SerializeField]
  private GameObject SlcAttencion;

  public override IEnumerator onInitSceneAsync()
  {
    this.IbtnBnr.SetActive(false);
    this.SlcAttencion.SetActive(false);
    this.menu.setTxtId(SMManager.Get<Player>().short_id);
    this.menu.setInpFriendid("");
    this.menu.RestoreInputLabelForNonMobileDevices();
    ((Component) this.InpFriendid).GetComponent<UIInput>().caretColor = Color.black;
    return base.onInitSceneAsync();
  }

  public void update() => this.menu.onChangeInpFriendid();

  protected IEnumerator onStartSceneAsync()
  {
    IEnumerator e = new Future<NGGameDataManager.StartSceneProxyResult>(new Func<Promise<NGGameDataManager.StartSceneProxyResult>, IEnumerator>(Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxyImpl)).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
