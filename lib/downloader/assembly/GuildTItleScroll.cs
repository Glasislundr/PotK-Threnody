// Decompiled with JetBrains decompiler
// Type: GuildTItleScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildTItleScroll : MonoBehaviour
{
  [SerializeField]
  private GameObject objNew;
  [SerializeField]
  private UI2DSprite scrImg;
  [SerializeField]
  private GameObject objUnKnown;
  private int id;
  private int rarity;
  private string description;
  private bool hasEmblem;
  private bool isCur;
  private DateTime? time;
  private Action act;
  private Guild0284Menu _baseMenu;

  public int ID => this.id;

  public int Rarity => this.rarity;

  public DateTime? Time => this.time;

  public IEnumerator Init(
    bool hasEmblem,
    int id,
    int rarity,
    string description,
    bool isCur,
    bool isNew,
    DateTime? time,
    Action act,
    Guild0284Menu baseMenu)
  {
    this.id = id;
    this.rarity = rarity;
    this.description = description;
    this.isCur = isCur;
    this.time = time;
    this.hasEmblem = hasEmblem;
    this.act = act;
    this._baseMenu = baseMenu;
    this.objNew.SetActive(isNew);
    ((Component) this.scrImg).gameObject.SetActive(hasEmblem);
    this.objUnKnown.SetActive(!hasEmblem);
    if (hasEmblem)
    {
      IEnumerator e = this.CreateSprite();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void TapTitle()
  {
    if (this._baseMenu.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenTitleSetPopup());
  }

  private IEnumerator OpenTitleSetPopup()
  {
    GameObject prefab = this._baseMenu.Guild02841Popup.Clone();
    IEnumerator e = prefab.GetComponent<Guild02841Popup>().Initialize(this.id, this.description, this.hasEmblem, this.isCur, this.act);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private IEnumerator CreateSprite()
  {
    Future<Sprite> sprF = EmblemUtility.LoadGuildEmblemSprite(this.id);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.scrImg.sprite2D = sprF.Result;
  }
}
