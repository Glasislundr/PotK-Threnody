// Decompiled with JetBrains decompiler
// Type: Tower029TowerClearAnimPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029TowerClearAnimPopup : MonoBehaviour
{
  private const string sprite_text = "slc_ef_text_%(num)s.png__GUI__tower_animation_sozai__tower_animation_sozai_prefab";
  private const string sprite_text_add = "slc_ef_text_%(num)s.png__GUI__tower_animation_sozai_Add__tower_animation_sozai_Add_prefab";
  public const int BaseLayer = 0;
  [SerializeField]
  private Animator anim;
  [SerializeField]
  private UISprite slc_ef_text_100;
  [SerializeField]
  private UISprite slc_ef_text_100_add;
  [SerializeField]
  private UISprite slc_ef_text_map;
  [SerializeField]
  private UISprite slc_ef_text_map_add;
  private Action actionSkip;

  private void Update()
  {
  }

  public void Initialize(int floor, Action action)
  {
    string empty = string.Empty;
    this.slc_ef_text_100.ChangeSprite(Consts.Format("slc_ef_text_%(num)s.png__GUI__tower_animation_sozai__tower_animation_sozai_prefab", (IDictionary) new Hashtable()
    {
      {
        (object) "num",
        (object) floor
      }
    }));
    this.slc_ef_text_100_add.ChangeSprite(Consts.Format("slc_ef_text_%(num)s.png__GUI__tower_animation_sozai_Add__tower_animation_sozai_Add_prefab", (IDictionary) new Hashtable()
    {
      {
        (object) "num",
        (object) floor
      }
    }));
    int width1 = ((UIWidget) this.slc_ef_text_100).width;
    int width2 = ((UIWidget) this.slc_ef_text_map).width;
    ((Component) this.slc_ef_text_100).transform.localPosition = new Vector3((float) ((double) -(width1 + width2) * 0.5 + (double) width1 * 0.5), ((Component) this.slc_ef_text_100).transform.localPosition.y, ((Component) this.slc_ef_text_100).transform.localPosition.z);
    ((Component) this.slc_ef_text_100_add).transform.localPosition = ((Component) this.slc_ef_text_100).transform.localPosition;
    ((Component) this.slc_ef_text_map).transform.localPosition = new Vector3((float) ((double) ((Component) this.slc_ef_text_100).transform.localPosition.x + (double) (width1 + width2) * 0.5 - 5.0), ((Component) this.slc_ef_text_map).transform.localPosition.y, ((Component) this.slc_ef_text_map).transform.localPosition.z);
    ((Component) this.slc_ef_text_map_add).transform.localPosition = ((Component) this.slc_ef_text_map).transform.localPosition;
    this.actionSkip = action;
  }

  public void Skip()
  {
    if (this.actionSkip == null)
      return;
    this.actionSkip();
    this.actionSkip = (Action) null;
  }

  public bool animEnd
  {
    get
    {
      AnimatorStateInfo animatorStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
      return (double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime >= 1.0;
    }
  }
}
