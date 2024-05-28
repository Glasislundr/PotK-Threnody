// Decompiled with JetBrains decompiler
// Type: Quest00215AMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00215AMenu : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtAp;
  [SerializeField]
  protected UILabel TxtEpisode;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtVictory;
  public GameObject StageName;
  public UISprite slc_New;
  public UISprite slc_Clear;
  public UI2DSprite dyn_Character;

  public virtual void IbtnBack() => this.backScene();

  public virtual void IbtnEpisodeback() => this.backScene();

  public virtual void IbtnDecide()
  {
  }

  private void SetEpisode(int episode)
  {
    this.StageName.transform.GetChildren().ForEach<Transform>((Action<Transform>) (t =>
    {
      string name = ((Object) ((Component) t).gameObject).name;
      int num = int.Parse(name.Substring(name.Length - 2));
      ((Component) t).gameObject.SetActive(num == episode);
    }));
  }

  private void SetState(Quest00215AMenu.State state)
  {
    ((Component) this.slc_New).gameObject.SetActive(state == Quest00215AMenu.State.NEW);
    ((Component) this.slc_Clear).gameObject.SetActive(state == Quest00215AMenu.State.CLEAR);
  }

  public IEnumerator SetCharacter(int episode, UnitUnit unit)
  {
    this.SetState(Quest00215AMenu.State.NEW);
    this.SetEpisode(3);
    this.TxtAp.SetTextLocalize(15);
    this.TxtEpisode.SetTextLocalize(unit.name + Consts.GetInstance().QUEST_00215_AMENU_SET_CHARACTER_1);
    this.TxtTitle.SetTextLocalize(unit.name);
    this.TxtVictory.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_00215_AMENU_SET_CHARACTER_2, (IDictionary) new Hashtable()
    {
      {
        (object) "turn",
        (object) "nn"
      }
    }));
    Future<Sprite> loader = unit.LoadSpriteLarge();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dyn_Character.sprite2D = loader.Result;
  }

  public enum State
  {
    NORMAL,
    CLEAR,
    NEW,
  }
}
