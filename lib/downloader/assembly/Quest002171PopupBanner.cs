// Decompiled with JetBrains decompiler
// Type: Quest002171PopupBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest002171PopupBanner : MonoBehaviour
{
  [SerializeField]
  private FloatButton BtnFormation;
  [SerializeField]
  private UI2DSprite IdleSprite;
  [SerializeField]
  private UIButton button;
  private Quest002171Scroll scrollcomp;

  public void SetBtnFormationEnable(bool active)
  {
    ((Collider) ((Component) this.BtnFormation).GetComponent<BoxCollider>()).enabled = active;
  }

  public IEnumerator InitScroll(
    bool isScroll,
    bool isAtlas,
    PlayerQuestGate gate,
    Quest002171Scroll scrollcomp)
  {
    this.scrollcomp = scrollcomp;
    IEnumerator e = this.SetSprite(gate.quest_gate_id, this.IdleSprite);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetScrollButtonCondition(gate);
  }

  private IEnumerator SetSprite(int id, UI2DSprite idle)
  {
    IEnumerator e = this.CreateSprite(string.Format("Prefabs/Banners/KeyQuest/popup_banner/{0}_idle", (object) id), idle);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetSprite(int id, UISprite sprite, PlayerQuestGate gate)
  {
    PlayerQuestGate[] tmp = new PlayerQuestGate[1]{ gate };
    EventDelegate.Set(this.button.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      this.scrollcomp.StartQuestReleasePopup(tmp);
    }));
    yield break;
  }

  private IEnumerator CreateSprite(string path, UI2DSprite obj)
  {
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
      path = string.Format("Prefabs/Banners/ExtraQuest/M/1/Specialquest_idle");
    Future<Texture2D> future = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = future.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      ((UIWidget) obj).width = ((Texture) result).width;
      ((UIWidget) obj).height = ((Texture) result).height;
      obj.sprite2D = sprite;
    }
  }

  private void SetScrollButtonCondition(PlayerQuestGate gate)
  {
    PlayerQuestGate[] tmp = new PlayerQuestGate[1]{ gate };
    EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() =>
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      this.scrollcomp.StartQuestReleasePopup(tmp);
    }));
  }
}
