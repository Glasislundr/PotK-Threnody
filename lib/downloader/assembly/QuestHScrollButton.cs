// Decompiled with JetBrains decompiler
// Type: QuestHScrollButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class QuestHScrollButton : MonoBehaviour
{
  [SerializeField]
  protected UILabel apCost;
  [SerializeField]
  protected UISprite newSprite;
  [SerializeField]
  protected UISprite clearSprite;
  [SerializeField]
  protected UI2DSprite StageNumberSprite;
  [SerializeField]
  protected LongPressFloatButton StageButton;
  [SerializeField]
  protected UI2DSprite IdleSprite;
  [SerializeField]
  protected UI2DSprite PressSprite;
  protected float defaultScale;
  protected float changedScale;
  protected float SpaceValue;
  [SerializeField]
  protected List<UIWidget> ObjWidgets;
  public GameObject Mask;
  protected int StageNumber;
  protected int ID;
  protected int AP;
  protected bool canPlay;
  protected GameObject MissionDescription;
  [SerializeField]
  protected UISprite StageBounas;
  private bool anim;
  private int? folderid;
  public List<bool> missionList;

  public void onSetValue()
  {
    this.defaultScale = 1f;
    this.changedScale = 0.9f;
    this.SpaceValue = ((Component) ((Component) this).transform.parent).GetComponent<UIGrid>().cellWidth;
  }

  private void WidgetGetInit()
  {
    this.ObjWidgets = new List<UIWidget>();
    this.ObjWidgets.Add(((Component) ((Component) this.apCost).transform).GetComponent<UIWidget>());
    this.ObjWidgets.Add(((Component) ((Component) this.newSprite).transform).GetComponent<UIWidget>());
    this.ObjWidgets.Add(((Component) ((Component) this.clearSprite).transform).GetComponent<UIWidget>());
    this.ObjWidgets.Add(((Component) ((Component) this.StageButton).transform).GetComponent<UIWidget>());
    this.ObjWidgets.Add(((Component) this.StageNumberSprite).GetComponent<UIWidget>());
    this.ObjWidgets.Add(((Component) this.IdleSprite).GetComponent<UIWidget>());
    this.ObjWidgets.Add(((Component) this.PressSprite).GetComponent<UIWidget>());
  }

  private IEnumerator ChoiceNumberSprites(string path)
  {
    Future<Sprite> spriteF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.StageNumberSprite.sprite2D = spriteF.Result;
    UI2DSprite stageNumberSprite1 = this.StageNumberSprite;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) stageNumberSprite1).width = num1;
    UI2DSprite stageNumberSprite2 = this.StageNumberSprite;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) stageNumberSprite2).height = num2;
  }

  protected virtual IEnumerator Init(
    int num,
    float gridWidth,
    int center,
    string NumberSpritePath,
    string colorName,
    bool eventQuest = false,
    bool storyOnly = false)
  {
    IEnumerator e = this.InitButton(colorName, eventQuest, storyOnly);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) this.ChoiceNumberSprites(NumberSpritePath);
    this.WidgetGetInit();
    this.StageNumber = num;
  }

  protected void InitValue(
    int ap,
    bool is_new,
    bool is_clear,
    int? has_reward,
    float gridWidth,
    int centernum,
    int[] clear_rewards)
  {
    ((Component) this).GetComponent<TweenPosition>().from.x = (float) ((this.StageNumber - centernum - 1) * 139);
    ((Component) this).GetComponent<TweenPosition>().to.x = (float) ((this.StageNumber - centernum - 1) * 139);
    this.apCost.SetTextLocalize(ap);
    ((Component) this.newSprite).gameObject.SetActive(is_new);
    ((Component) this.clearSprite).gameObject.SetActive(is_clear);
    this.SetBonusRewardImage(clear_rewards);
  }

  private void SetBonusRewardImage(int[] clear_rewards)
  {
    if (clear_rewards == null || clear_rewards.Length == 0)
    {
      ((Component) this.StageBounas).gameObject.SetActive(false);
    }
    else
    {
      List<BattleStageClear> list = ((IEnumerable<int>) clear_rewards).Where<int>((Func<int, bool>) (x => MasterData.BattleStageClear.ContainsKey(x))).Select<int, BattleStageClear>((Func<int, BattleStageClear>) (x => MasterData.BattleStageClear[x])).ToList<BattleStageClear>();
      if (list.Count <= 0)
      {
        ((Component) this.StageBounas).gameObject.SetActive(false);
      }
      else
      {
        ((Component) this.StageBounas).gameObject.SetActive(true);
        UISpriteData uiSpriteData = (UISpriteData) null;
        UISprite component = ((Component) this.StageBounas).GetComponent<UISprite>();
        if (list.Count == 1)
        {
          string str = string.Format("slc_BonusIcon_{0}.png__GUI__002-2_sozai__002-2_sozai_prefab", (object) list[0].entity_type_CommonRewardType);
          if (Singleton<NGGameDataManager>.GetInstance().IsSea)
            str = str.Replace("002-2_sozai", "002-2_sozai_sea");
          uiSpriteData = component.atlas.GetSprite(str);
          if (uiSpriteData != null)
            component.spriteName = str;
        }
        if (uiSpriteData != null || list.Count <= 0)
          return;
        string str1 = "slc_BonusIcon_Other.png__GUI__002-2_sozai__002-2_sozai_prefab";
        if (Singleton<NGGameDataManager>.GetInstance().IsSea)
          str1 = str1.Replace("002-2_sozai", "002-2_sozai_sea");
        component.spriteName = str1;
      }
    }
  }

  public void ChangeToneConditionJudge(float variationValue)
  {
    float num = Mathf.Abs(variationValue) / this.SpaceValue;
    this.ChangeToneCondition((double) num >= 1.0 ? 1f : ((double) num <= 0.0 ? 0.0f : num));
  }

  private void ChangeToneCondition(float changeValue)
  {
    float num = (this.defaultScale - this.changedScale) * changeValue;
    Color color = Color.Lerp(Color.white, Color.gray, changeValue);
    ((Component) this).transform.localScale = new Vector3(this.defaultScale - num, this.defaultScale - num, this.defaultScale);
    foreach (UIWidget objWidget in this.ObjWidgets)
      objWidget.color = !this.canPlay ? Color.Lerp(Color.white, Color.gray, 1f) : color;
  }

  public void NotTouch(bool judge)
  {
    bool flag = judge || !this.canPlay;
    this.Mask.SetActive(flag);
    ((Behaviour) this.StageButton).enabled = !flag;
  }

  public void centerAnimation(bool flag)
  {
    if (this.anim != flag && flag && this.canPlay)
    {
      Color color = Color.Lerp(Color.white, Color.gray, 0.0f);
      foreach (UIWidget objWidget in this.ObjWidgets)
        objWidget.color = color;
      ((UITweener) ((Component) this).GetComponent<TweenScale>()).PlayForward();
    }
    else if (!this.canPlay)
    {
      Color color = Color.Lerp(Color.white, Color.gray, 1f);
      foreach (UIWidget objWidget in this.ObjWidgets)
        objWidget.color = color;
      ((Behaviour) ((Component) this).GetComponent<TweenScale>()).enabled = false;
    }
    this.anim = flag;
  }

  private IEnumerator InitButton(string color, bool eventQuest, bool storyOnly = false)
  {
    QuestHScrollButton questHscrollButton = this;
    if (color == "")
      color = "brown";
    IEnumerator e = questHscrollButton.SetSpritePaths(color, eventQuest, storyOnly);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(questHscrollButton.StageButton.onOver, new EventDelegate.Callback(questHscrollButton.\u003CInitButton\u003Eb__31_0));
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(questHscrollButton.StageButton.onOut, new EventDelegate.Callback(questHscrollButton.\u003CInitButton\u003Eb__31_1));
    questHscrollButton.OverOrOut(false);
  }

  private void OverOrOut(bool over)
  {
    ((Component) this.IdleSprite).gameObject.SetActive(!over);
    ((Component) this.PressSprite).gameObject.SetActive(over);
  }

  private IEnumerator SetSpritePaths(string colorName, bool eventQuest, bool storyOnly)
  {
    string str = eventQuest ? (!storyOnly ? "Prefabs/Quest/Extra/btn_Color/" + colorName : "Prefabs/Quest/Extra/btn_Color/story") : (!storyOnly ? "Prefabs/Quest/Story/btn_Color/" + colorName : "Prefabs/Quest/Story/btn_Color/story");
    string path = str + "/idle";
    string presspath = str + "/press";
    IEnumerator e = this.CreateSprite(path, this.IdleSprite);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.CreateSprite(presspath, this.PressSprite);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CreateSprite(string path, UI2DSprite spriteobj)
  {
    Future<Texture2D> futureIdle = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
    IEnumerator e = futureIdle.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = futureIdle.Result;
    Sprite spr = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
    ((Object) spr).name = ((Object) result).name;
    this.AtacheSprite(spr, spriteobj);
  }

  private void AtacheSprite(Sprite spr, UI2DSprite sprobj)
  {
    UI2DSprite ui2Dsprite1 = sprobj;
    Rect rect1 = spr.rect;
    int num1 = Mathf.FloorToInt(((Rect) ref rect1).width);
    ((UIWidget) ui2Dsprite1).width = num1;
    UI2DSprite ui2Dsprite2 = sprobj;
    Rect rect2 = spr.rect;
    int num2 = Mathf.FloorToInt(((Rect) ref rect2).height);
    ((UIWidget) ui2Dsprite2).height = num2;
    sprobj.sprite2D = spr;
  }
}
