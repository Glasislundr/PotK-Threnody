// Decompiled with JetBrains decompiler
// Type: BattleUI05HardModeOpenMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI05HardModeOpenMenu : ResultMenuBase
{
  private GameObject popupObject;
  private string title;
  private string message;

  public bool ToNext { set; get; }

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    this.title = result.unlock_messages[0].title;
    this.message = result.unlock_messages[0].message;
    Future<GameObject> popupPrefab = Res.Prefabs.popup.popup_020_31__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popupObject = popupPrefab.Result;
    this.ToNext = false;
  }

  public override IEnumerator Run()
  {
    BattleUI05HardModeOpenMenu menu = this;
    BattleUI05HardModeOpenPopup popup = Singleton<PopupManager>.GetInstance().open(menu.popupObject).GetComponent<BattleUI05HardModeOpenPopup>();
    popup.Init(menu, menu.title, menu.message);
    while (!menu.ToNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        popup.IbtnPopupOk();
      }
      yield return (object) null;
    }
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
