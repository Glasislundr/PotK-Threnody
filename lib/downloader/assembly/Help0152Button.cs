// Decompiled with JetBrains decompiler
// Type: Help0152Button
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Help0152Button : MonoBehaviour
{
  [SerializeField]
  private UILabel txtHelp01;
  private HelpHelp help;
  private GameObject popup;
  private GameObject textPrefab;
  private GameObject spritePrefab;
  private Sprite helpSprite;
  private BackButtonMenuBase _baseMenu;

  public void init(BackButtonMenuBase baseMenu) => this._baseMenu = baseMenu;

  public IEnumerator setTextHelp01(string str, HelpHelp help)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.popup, (Object) null))
    {
      Future<GameObject> popupF = Res.Prefabs.help015_3.popup_015_3__anim_popup01.Load<GameObject>();
      e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.popup = popupF.Result;
      Future<GameObject> textPrefabF = Res.Prefabs.help015_3.textBox.Load<GameObject>();
      e = textPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.textPrefab = textPrefabF.Result;
      Future<GameObject> spritePrefabF = Res.Prefabs.help015_3.spriteBox.Load<GameObject>();
      e = spritePrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.spritePrefab = spritePrefabF.Result;
      popupF = (Future<GameObject>) null;
      textPrefabF = (Future<GameObject>) null;
      spritePrefabF = (Future<GameObject>) null;
    }
    if (help.image_name != "")
    {
      Future<Sprite> textureF = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Help/" + help.image_name);
      e = textureF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.helpSprite = textureF.Result;
      textureF = (Future<Sprite>) null;
    }
    this.txtHelp01.SetTextLocalize(str);
    this.help = help;
  }

  public void IbtnHelp()
  {
    if (this._baseMenu.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().openAlert(this.popup).GetComponent<Help0153Menu>().InitHelpPopup(this.help, this.helpSprite, this.textPrefab, this.spritePrefab);
  }
}
