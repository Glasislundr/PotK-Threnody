// Decompiled with JetBrains decompiler
// Type: Sea030StoryButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030StoryButton : MonoBehaviour
{
  private const string path_btn_idle = "ibtn_quest_{0}_idle.png__GUI__sea_home__sea_home_prefab";
  private const string path_btn_pressed = "ibtn_quest_{0}_pressed.png__GUI__sea_home__sea_home_prefab";
  private const string path_btn_area_name = "slc_area_name_{0}_base.png__GUI__sea_home__sea_home_prefab";
  private const string path_map_star = "slc_map_star{0}.png__GUI__sea_home__sea_home_prefab";
  [SerializeField]
  private Sea030StoryButton.StoryButtonImageinfo[] storyBtnImgList;
  [SerializeField]
  private UIButton btnUnlockedCircle;
  [SerializeField]
  private UIButton btnClearedCircle;
  [SerializeField]
  private UISprite Bonus;
  [SerializeField]
  private GameObject slc_area_name_orange_base;
  [SerializeField]
  private GameObject slc_area_name_gray_base;
  [SerializeField]
  private UILabel lblStoryName;
  [SerializeField]
  private GameObject[] _slc_pointer_area;
  [SerializeField]
  private UISprite clearSprite;
  [SerializeField]
  private GameObject missionAchevement;
  [SerializeField]
  private UILabel missionAchevementCount;
  [SerializeField]
  private UILabel missionAchevementComplete;
  [SerializeField]
  private UISprite slc_map_star;
  private float StartDelay;
  private bool clickMySelf;
  private int ButtonResourcePathNumber;
  private int ButtonMnumber;
  private int ButtonLnumber;
  private int _TweenIndex;

  public UIButton BtnClearedCircle => this.btnClearedCircle;

  public GameObject slc_pointer_area
  {
    get
    {
      return this.Lindex < 0 || this.Lindex >= this._slc_pointer_area.Length ? (GameObject) null : this._slc_pointer_area[this.Lindex];
    }
  }

  public EventDelegate onClick
  {
    set => EventDelegate.Set(this.btnUnlockedCircle.onClick, value);
  }

  public int PathNumber
  {
    set => this.ButtonResourcePathNumber = value;
  }

  public int Mnumber
  {
    get => this.ButtonMnumber;
    set => this.ButtonMnumber = value;
  }

  public int Lnumber
  {
    get => this.ButtonLnumber;
    set => this.ButtonLnumber = value;
  }

  public int Lindex { get; set; }

  public int TweenIndex
  {
    get => this._TweenIndex;
    set => this._TweenIndex = value;
  }

  public string StoryName
  {
    set => this.lblStoryName.SetTextLocalize(value);
  }

  public bool isActive() => ((Component) this.btnUnlockedCircle).gameObject.activeSelf;

  public void Lock()
  {
    ((Component) this.clearSprite).gameObject.SetActive(false);
    this.missionAchevement.SetActive(false);
    ((Component) this.Bonus).gameObject.SetActive(false);
    ((Component) this.btnClearedCircle).gameObject.SetActive(false);
    ((Component) this.btnUnlockedCircle).gameObject.SetActive(true);
    ((UIButtonColor) this.btnUnlockedCircle).isEnabled = false;
    this.slc_area_name_gray_base.SetActive(true);
    this.slc_area_name_orange_base.SetActive(false);
  }

  public void UnLock(bool clearflag, bool newflag)
  {
    ((Component) this.clearSprite).gameObject.SetActive(clearflag);
    this.missionAchevement.SetActive(true);
    ((Component) this.missionAchevementComplete).gameObject.SetActive(false);
    ((Component) this.missionAchevementCount).gameObject.SetActive(false);
    ((Component) this.slc_map_star).gameObject.SetActive(false);
    ((Component) this.btnClearedCircle).gameObject.SetActive(false);
    ((Component) this.btnUnlockedCircle).gameObject.SetActive(true);
    ((UIButtonColor) this.btnUnlockedCircle).isEnabled = true;
    this.slc_area_name_gray_base.SetActive(false);
    this.slc_area_name_orange_base.SetActive(true);
  }

  public void MissionAchevement(int nowCount, int allCount)
  {
    if (allCount == 0)
    {
      ((Component) ((Component) this.missionAchevementComplete).transform.parent).gameObject.SetActive(false);
    }
    else
    {
      bool flag = nowCount == allCount;
      ((Component) this.missionAchevementComplete).gameObject.SetActive(flag);
      ((Component) this.missionAchevementCount).gameObject.SetActive(!flag);
      ((Component) this.slc_map_star).gameObject.SetActive(!flag);
      if (flag)
        return;
      this.missionAchevementCount.SetTextLocalize(nowCount.ToString() + string.Format("/{0}", (object) allCount));
    }
  }

  public void changeClickMySelf()
  {
    TweenPosition component = ((Component) this).GetComponent<TweenPosition>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      component.to.y = -component.to.y;
      ((UITweener) component).delay = 0.0f;
    }
    this.clickMySelf = !this.clickMySelf;
  }

  public void initTween()
  {
    if (!this.clickMySelf)
      return;
    this.changeClickMySelf();
  }

  public void SetBonus(int bonusCategory)
  {
    if (bonusCategory == 0)
    {
      Debug.LogWarning((object) "＋＋＋＋＋＋＋　ボーナスなし　＋＋＋＋＋＋＋");
      ((Component) this.Bonus).gameObject.SetActive(false);
    }
    else
    {
      string str = string.Format("slc_Bonus_{0}.png__GUI__quest_bonus_sozai_sea__quest_bonus_sozai_sea_prefab", (object) bonusCategory);
      Debug.Log((object) string.Format("＋＋＋＋＋＋＋　spriteName : {0}  ＋＋＋＋＋＋＋", (object) str));
      UISpriteData sprite = this.Bonus.atlas.GetSprite(str);
      if (sprite != null)
      {
        ((Component) this.Bonus).gameObject.SetActive(true);
        this.Bonus.spriteName = str;
        UIWidget component = ((Component) this.Bonus).GetComponent<UIWidget>();
        Vector3 localPosition = ((Component) component).transform.localPosition;
        component.SetRect(0.0f, 0.0f, (float) sprite.width, (float) sprite.height);
        ((Component) component).transform.localPosition = localPosition;
      }
      else
      {
        Debug.LogWarning((object) string.Format("＋＋＋＋＋＋＋　{0}がありません　＋＋＋＋＋＋＋", (object) str));
        ((Component) this.Bonus).gameObject.SetActive(false);
      }
    }
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

  public void SetSprite(Sea030_questMenu.storyGroup group)
  {
    Sea030StoryButton.StoryButtonImageinfo storyButtonImageinfo = (Sea030StoryButton.StoryButtonImageinfo) null;
    for (int index = 0; index < this.storyBtnImgList.Length; ++index)
    {
      if (this.storyBtnImgList[index].group == group)
      {
        storyButtonImageinfo = this.storyBtnImgList[index];
        break;
      }
    }
    if (storyButtonImageinfo == null)
      return;
    ((UIWidget) this.missionAchevementComplete).color = storyButtonImageinfo.achievementCountColor;
    ((UIWidget) this.missionAchevementCount).color = storyButtonImageinfo.achievementCountColor;
    UISprite component1 = ((Component) this.btnUnlockedCircle).GetComponent<UISprite>();
    if (Object.op_Inequality((Object) component1, (Object) null))
      component1.spriteName = string.Format("ibtn_quest_{0}_idle.png__GUI__sea_home__sea_home_prefab", (object) storyButtonImageinfo.color_path);
    UISprite component2 = this.slc_area_name_orange_base.GetComponent<UISprite>();
    if (Object.op_Inequality((Object) component2, (Object) null))
      component2.spriteName = string.Format("slc_area_name_{0}_base.png__GUI__sea_home__sea_home_prefab", (object) storyButtonImageinfo.color_path);
    UISprite component3 = ((Component) this.slc_map_star).GetComponent<UISprite>();
    if (Object.op_Inequality((Object) component3, (Object) null))
      component3.spriteName = string.Format("slc_map_star{0}.png__GUI__sea_home__sea_home_prefab", (object) storyButtonImageinfo.num_path);
    UIButton component4 = ((Component) this.btnUnlockedCircle).GetComponent<UIButton>();
    if (!Object.op_Inequality((Object) component4, (Object) null))
      return;
    component4.normalSprite = string.Format("ibtn_quest_{0}_idle.png__GUI__sea_home__sea_home_prefab", (object) storyButtonImageinfo.color_path);
    component4.pressedSprite = string.Format("ibtn_quest_{0}_pressed.png__GUI__sea_home__sea_home_prefab", (object) storyButtonImageinfo.color_path);
  }

  [Serializable]
  public class StoryButtonImageinfo
  {
    public Sea030_questMenu.storyGroup group;
    public string color_path = string.Empty;
    public string num_path = string.Empty;
    public Color achievementCountColor = new Color(1f, 0.545098066f, 0.0f, 1f);
  }
}
