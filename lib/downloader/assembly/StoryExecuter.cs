// Decompiled with JetBrains decompiler
// Type: StoryExecuter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class StoryExecuter : BackButtonMonoBehaiviour
{
  [SerializeField]
  private bool isAutoPlay_;
  [SerializeField]
  private float waitNextAutoPlay_ = 0.5f;
  [SerializeField]
  private GameObject btnAutoON_;
  [SerializeField]
  private GameObject btnAutoOFF_;
  private float timeNextAutoPlay_;
  private List<Func<bool>> lstFunctionWait_ = new List<Func<bool>>();
  [SerializeField]
  private FadeControl fadeControl;
  [SerializeField]
  private FlushControl flushControl;
  [SerializeField]
  private Texture2D[] mSprite;
  [SerializeField]
  private GameObject resetButton;
  [SerializeField]
  private GameObject backButton;
  [SerializeField]
  private bool isReset;
  [SerializeField]
  private bool isBack;
  [SerializeField]
  private GameObject mainPanel;
  [SerializeField]
  private QuestMoviePlayer movieObj;
  [SerializeField]
  private string rgbTextNormal_ = "330000";
  [SerializeField]
  private float fadeInTime_ = 0.5f;
  public List<PlaySEData> plsySEDataList = new List<PlaySEData>();
  public GameObject[] imageObj;
  public UILabel textTopLabel;
  public UILabel nameTopLabel;
  public UILabel textBottomLabel;
  public UILabel nameBottomLabel;
  public GameObject textContainerTop;
  public GameObject topArrow;
  public GameObject textContainerBottom;
  public GameObject textTopObj;
  public GameObject textBottomObj;
  public UISprite nextTopSprite;
  public UISprite nextBottomSprite;
  public GameObject windowObj;
  public BacklogManager backlog_manager;
  public Transform[] personPoints;
  public Transform[] imagePoints;
  public Vector3[] positions;
  public NGTweenParts nextButton;
  public GameObject selectObj;
  public float skip_wait = 0.2f;
  public float select_button_wait = 1f;
  public UILabel[] SelectLabel;
  public UILabel quastion;
  public GameObject[] select;
  public GameObject[] selectBtn;
  public GameObject[] selectBtnObj;
  public GameObject[] textBoxBaseTopObj;
  public GameObject[] textBoxBaseBottomObj;
  public GameObject[] textTopFlame;
  public GameObject[] textTop3Nozzle;
  public GameObject[] textBottomFlame;
  public GameObject[] textBottom3Nozzle;
  public UISprite[] textTopBox;
  public UISprite[] textBottomBox;
  public SpriteSelectDirect nameTopBox;
  public SpriteSelectDirect nameBottomBox;
  public GameObject myselfBase;
  public GameObject charaTextBoxContainer;
  public GameObject myselfTextBoxContainer;
  public UISprite[] myselfSprite;
  public GameObject[] myselfTextBox;
  public GameObject[] myselfBoxBase;
  public GameObject cutinObj;
  public GameObject OnePiecePicture;
  public GameObject pictureContainer;
  public CutinNameManager CNM;
  public int charsPerSecond = 40;
  public float stopBGMTime = 0.5f;
  public GameObject skipBtn;
  private StoryExecuter.ScriptMode modeScript_;
  private Action<int, int> onSelected_;
  private int countSelect_;
  private bool _isFinished;
  private StoryEnvironment environment = new StoryEnvironment();
  private StoryResource storyResource = new StoryResource();
  private ShakeControl shakecontrol;
  private TweenAlpha tweenalpha;
  private GameObject position;
  private GameObject bodyObj;
  private GameObject faceObj;
  private UI2DSprite fadeColor;
  private Jump jump;
  private TextShake textTopShake;
  private TextShake textBottomShake;
  private UISprite face;
  private GameObject bgObj;
  private UI2DSprite backGround;
  private int charaPos;
  private bool fadeIn;
  private bool fillrect;
  private float fadeAlpha;
  private bool skip_enable;
  private float skip_wait_time;
  private float select_wait = 1f;
  private bool onSelect;
  private GameObject unitPrefab;
  private NGxFaceSprite unitFace;
  private ResourceObject texture;
  private string cutinName;
  private int cutInNum;
  private bool cutIn;
  private NGSoundManager sm;
  private Vector3 myPos;
  private bool textFlame;
  private List<GameObject> units = new List<GameObject>();
  private GameObject CharacterPrefab;
  private bool isInitializeDone;
  private float wait_command_time;
  private string typewriterText;
  private bool re;
  private BoxCollider box;
  private StoryBG sbg;
  private bool isWaitAndNext;
  public TextAsset storyText;
  private int coroutineCount;
  private Coroutine typewriterEffectCoroutine;
  private int scriptId = -1;
  private List<Story0093Scene.ContinuousData> continuousDataList = new List<Story0093Scene.ContinuousData>();
  public bool continuousPopupView;
  private Action endAction;
  public const string lastScriptIdKey = "LastScriptId";
  private Dictionary<int, StoryExecuter.CharacterInfo> characterList = new Dictionary<int, StoryExecuter.CharacterInfo>();
  [SerializeField]
  private FadeControl fadeSubControl_;
  [SerializeField]
  private int[] subFillrectDepths_ = new int[2]{ 1, 100 };
  [SerializeField]
  private GameObject frameTop_;
  [SerializeField]
  private GameObject frameBottom_;
  [SerializeField]
  private StoryExecuter.FramePositions[] frameOutPositions_;
  [SerializeField]
  private GameObject topButtons_;
  [SerializeField]
  private Vector2[] buttonOutPositions_;
  private bool fillrectSub_;
  private Dictionary<int, StoryExecuter.ImageInfo> imageInfo = new Dictionary<int, StoryExecuter.ImageInfo>();

  private bool checkWaitAutoPlay()
  {
    int index = 0;
    while (index < this.lstFunctionWait_.Count)
    {
      if (this.lstFunctionWait_[index]())
        ++index;
      else
        this.lstFunctionWait_.RemoveAt(index);
    }
    return this.lstFunctionWait_.Count > 0;
  }

  private void addWaitAutoPlay(Func<bool> execWait)
  {
    if (execWait == null)
      return;
    this.lstFunctionWait_.Add(execWait);
    this.timeNextAutoPlay_ = this.waitNextAutoPlay_;
  }

  private bool isNextAutoPlay()
  {
    if ((double) this.timeNextAutoPlay_ > 0.0)
      this.timeNextAutoPlay_ -= Time.deltaTime;
    return (double) this.timeNextAutoPlay_ <= 0.0;
  }

  private void clearWaitAutoPlay()
  {
    this.lstFunctionWait_.Clear();
    this.timeNextAutoPlay_ = this.waitNextAutoPlay_;
  }

  private void initAutoPlay()
  {
    this.setStatusAutoPlay(this.isAutoPlay_);
    this.clearWaitAutoPlay();
  }

  private void setStatusAutoPlay(bool isAutoPlay)
  {
    this.btnAutoON_.SetActive(!isAutoPlay);
    this.btnAutoOFF_.SetActive(isAutoPlay);
    this.isAutoPlay_ = isAutoPlay;
  }

  public void onClickedAutoPlayON()
  {
    this.setStatusAutoPlay(true);
    this.saveOptionSetting();
  }

  public void onClickedAutoPlayOFF()
  {
    this.setStatusAutoPlay(false);
    this.saveOptionSetting();
  }

  private void saveOptionSetting()
  {
    Persist<Persist.StoryOptions> storyOptions = Persist.storyOptions;
    storyOptions.Data.autoPlayEnable = this.isAutoPlay_;
    storyOptions.Flush();
  }

  private void loadOptionSetting() => this.isAutoPlay_ = Persist.storyOptions.Data.autoPlayEnable;

  private bool isFinished
  {
    set
    {
      NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
      if (Object.op_Inequality((Object) instance, (Object) null) && instance.CharacterQuestAfterBattleInfo != null && instance.CharacterQuestAfterBattleScriptId != 0 && this.endAction != null)
        this._isFinished = false;
      else
        this._isFinished = value;
    }
  }

  protected override void Update()
  {
    base.Update();
    if (!this.isInitializeDone || this._isFinished)
      return;
    bool flag = this.checkWaitAutoPlay();
    if (this.skip_enable && this.environment.SkipReady())
    {
      this.skip_wait_time -= Time.deltaTime;
      this.fadeNextExit();
      if (this.cutIn)
      {
        this.CNM.wait = 0.3f;
        this.CNM.startWait = 0.3f;
        if (this.cutInNum == 1)
          this.CNM.left = CutinNameManager.LeftState.Del;
        else if (this.cutInNum == 2)
          this.CNM.center = CutinNameManager.CenterState.Del;
        else if (this.cutInNum == 3)
          this.CNM.right = CutinNameManager.RightState.Del;
      }
      if ((double) this.skip_wait_time <= 0.0)
      {
        this.skip_wait_time = this.skip_wait;
        this.wait_command_time = 0.0f;
        this.onNextButton();
      }
    }
    if ((double) this.wait_command_time > 0.0)
      this.wait_command_time -= Time.deltaTime;
    else if (!this.backlog_manager.IsEnable() && !this.continuousPopupView && (this.isWaitAndNext && this.typewriterText == null || this.isAutoPlay_ && !flag && this.typewriterText == null && this.isNextAutoPlay()))
    {
      this.isWaitAndNext = false;
      this.wait_command_time = 0.0f;
      this.onNextButton();
    }
    if (!this.onSelect)
      return;
    this.select_button_wait -= Time.deltaTime;
    if ((double) this.select_button_wait > 0.0)
      return;
    this.select_button_wait = this.select_wait;
    this.onSelect = false;
    this.textContainerBottom.SetActive(true);
    this.onNextButton();
  }

  private void fadeNextExit()
  {
    if (this.fadeIn && (double) this.fadeAlpha == 1.0)
    {
      this.fadeControl.time = 0.0f;
      this.fadeControl.StartFade();
    }
    else if (!this.fadeIn && (double) this.fadeAlpha == 0.0)
    {
      this.fadeControl.time = 0.0f;
      this.fadeControl.StartFade();
    }
    else if (this.fillrect)
    {
      this.fillrect = false;
      this.fadeControl.time = 0.0f;
      this.fadeControl.StartFillrect();
    }
    this.fadeSubNextExit();
  }

  public void setMode(StoryExecuter.ScriptMode mode) => this.modeScript_ = mode;

  public void setEventSelected(Action<int, int> eventSelected) => this.onSelected_ = eventSelected;

  private void callEventSelected(int index)
  {
    if (this.onSelected_ != null)
      this.onSelected_(this.countSelect_, index);
    ++this.countSelect_;
  }

  public IEnumerator initialize(
    string text,
    Action endAction = null,
    int scriptId = -1,
    List<Story0093Scene.ContinuousData> continuousDataList = null)
  {
    StoryExecuter exec = this;
    exec._isFinished = false;
    exec.isInitializeDone = false;
    exec.scriptId = scriptId;
    exec.continuousDataList = continuousDataList;
    PlayerPrefs.SetInt("LastScriptId", scriptId);
    PlayerPrefs.Save();
    exec.continuousPopupView = false;
    exec.loadOptionSetting();
    exec.plsySEDataList = (List<PlaySEData>) null;
    exec.plsySEDataList = new List<PlaySEData>();
    exec.plsySEDataList.Clear();
    StoryExecuter.ColorDefault.Bottom = exec.rgbTextNormal_;
    StoryExecuter.ColorDefault.Top = exec.rgbTextNormal_;
    if (text == null)
      text = exec.storyText.text;
    exec.resetButton.SetActive(exec.isReset);
    exec.backButton.SetActive(exec.isBack);
    exec.initAutoPlay();
    IEnumerator e = exec.DeleteObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.charaPos = 0;
    exec.textContainerTop.SetActive(false);
    exec.textContainerBottom.SetActive(true);
    Future<Sprite> spriteF = Res.GUI._009_3_sozai.mask_Chara_L.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.mSprite[0] = spriteF.Result.texture;
    spriteF = Res.GUI._009_3_sozai.mask_Chara_L.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.mSprite[1] = spriteF.Result.texture;
    spriteF = Res.GUI._009_3_sozai.mask_Chara_C.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.mSprite[2] = spriteF.Result.texture;
    spriteF = Res.GUI._009_3_sozai.mask_Chara_R.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.mSprite[3] = spriteF.Result.texture;
    spriteF = Res.GUI._009_3_sozai.mask_Chara_R.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.mSprite[4] = spriteF.Result.texture;
    Future<GameObject> bg1 = Res.Prefabs.BackGround.storyBGprefab.Load<GameObject>();
    e = bg1.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.bgObj = bg1.Result.Clone(exec.windowObj.transform);
    exec.bgObj.transform.localPosition = new Vector3(0.0f, 50f, 0.0f);
    Future<GameObject> charaFuture = Res.Prefabs.ADVCharacter.Load<GameObject>();
    e = charaFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.CharacterPrefab = charaFuture.Result;
    exec.environment.initialize(text, exec);
    e = exec.storyResource.Run(exec.environment.all());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exec.isInitializeDone = true;
    exec.endAction = endAction;
  }

  public void onEndScene()
  {
    this.stopSeAll();
    this.stopVoiceAll();
  }

  public IEnumerator DeleteObject()
  {
    foreach (Object unit in this.units)
      Object.Destroy(unit);
    this.units.Clear();
    this.windowObj.SendMessage("StopShake");
    this.setColorAndTime(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    this.fadeControl.StartFade();
    if (this.re)
    {
      this.re = false;
      this.render();
      yield break;
    }
  }

  private IEnumerator typewriterEffect(string text, UILabel targetLabel)
  {
    int offset = 0;
    float nextChar = 0.0f;
    int cps = this.charsPerSecond;
    if (this.typewriterText == text)
      text = (string) null;
    this.typewriterText = text;
    while (true)
    {
      NGUIText.ParseSymbol(text, ref offset);
      if (offset < text.Length)
      {
        if ((double) nextChar <= (double) RealTime.time)
        {
          cps = Mathf.Max(1, cps);
          float num = 1f / (float) cps;
          switch (text[offset])
          {
            case '\n':
            case '!':
            case '?':
            case '、':
            case '。':
            case '・':
            case '！':
            case '？':
              num *= 4f;
              break;
          }
          nextChar = RealTime.time + num;
          this.setTextBox();
          this.setNameMsgBright();
          ((UIWidget) this.GetLabelIncludeText(text, targetLabel, ++offset)).SetDirty();
        }
        yield return (object) null;
      }
      else
        break;
    }
    this.typewriterText = (string) null;
  }

  private void renderText(string text, bool isEffect = true)
  {
    UILabel uiLabel = this.environment.current.text.pos == TextBlock.Position.BOTTOM ? this.textBottomLabel : this.textTopLabel;
    if (uiLabel.alignment != 2 & isEffect)
      this.typewriterEffectCoroutine = this.StartCoroutine(this.typewriterEffect(text, uiLabel));
    else
      this.GetLabelIncludeText(text, uiLabel, text.Length);
  }

  private void CheckEnableObject()
  {
    this.box = this.windowObj.GetComponent<BoxCollider>();
    if (this.environment.current.select != null)
    {
      this.textContainerTop.SetActive(false);
      this.textContainerBottom.SetActive(false);
      ((Collider) this.box).enabled = false;
    }
    else
    {
      for (int index = 0; index < 3; ++index)
        this.select[index].SetActive(false);
      this.selectObj.SetActive(false);
      ((Collider) this.box).enabled = true;
    }
  }

  public void render()
  {
    this.resetCoroutine();
    try
    {
      this.environment.evalCurrent();
      if (this.environment.current.select == null)
      {
        if (this.environment.current.text.pos == TextBlock.Position.BOTTOM)
          this.backlog_manager.Add(this.nameBottomLabel.text, this.environment.current.text.text);
        else
          this.backlog_manager.Add(this.nameTopLabel.text, this.environment.current.text.text);
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
    this.CheckEnableObject();
    if (this.environment.current.select != null)
      return;
    this.nextButton.isActive = this.environment.nextBlockp();
    this.renderText(this.environment.current.getText());
  }

  public void onNextButton()
  {
    if ((double) this.wait_command_time > 0.0)
      return;
    this.isWaitAndNext = false;
    this.clearWaitAutoPlay();
    if (this.typewriterText != null)
    {
      this.StopCoroutine(this.typewriterEffectCoroutine);
      this.renderText(this.typewriterText, false);
      this.typewriterText = (string) null;
      foreach (KeyValuePair<int, StoryExecuter.CharacterInfo> character in this.characterList)
      {
        Clash component = character.Value.obj.GetComponent<Clash>();
        if (component.isClash)
          component.EndClash();
      }
    }
    else
    {
      if (Object.op_Inequality((Object) this.CNM, (Object) null))
      {
        this.CNM.left = CutinNameManager.LeftState.End;
        this.CNM.right = CutinNameManager.RightState.End;
        this.CNM.center = CutinNameManager.CenterState.End;
      }
      this.charaPos = 0;
      if (this.environment.nextBlockp())
      {
        this.SetColorText(TextBlock.Position.TOP, "normal");
        this.SetColorText(TextBlock.Position.BOTTOM, "normal");
        this.SetTextAlgin(TextBlock.Position.TOP, "left");
        this.SetTextAlgin(TextBlock.Position.BOTTOM, "left");
        this.flushControl.SetEnd();
        this.fadeNextExit();
        this.environment.nextBlock();
        this.nextButton.isActive = this.environment.nextBlockp();
        this.render();
      }
      else
      {
        this.nextButton.isActive = this.environment.nextBlockp();
        if (!this.storyContinuous())
        {
          this.backScene();
          this.skip_enable = false;
          this.isFinished = true;
        }
      }
      foreach (KeyValuePair<int, StoryExecuter.CharacterInfo> character in this.characterList)
      {
        Clash component = character.Value.obj.GetComponent<Clash>();
        if (component.isClash)
          component.EndClash();
      }
    }
    if ((double) this.textTopObj.transform.localPosition.x != 0.0)
      this.stopTextShake(true);
    if ((double) this.textBottomObj.transform.localPosition.x == 0.0)
      return;
    this.stopTextShake(false);
  }

  public void onSkipEnableButton() => this.skip_enable = false;

  public void backScene()
  {
    if (this.endAction != null)
      this.endAction();
    else
      Singleton<NGSceneManager>.GetInstance().backScene();
  }

  public void onSkipButton()
  {
    if (this.modeScript_ == StoryExecuter.ScriptMode.Normal)
    {
      if (this._isFinished || this.storyContinuous())
        return;
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1040");
      else
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
      this.isFinished = true;
      this.backScene();
    }
    else
    {
      this.skip_enable = !this.skip_enable;
      if (this.skip_enable)
        this.skip_wait_time = this.skip_wait;
      if (this.environment.nextBlockp() && !Input.GetKey("return"))
        return;
      this.skip_enable = false;
      if (this.skip_enable)
        return;
      this.onNextButton();
    }
  }

  public void onLogButton()
  {
    this.skip_enable = false;
    this.box = this.windowObj.GetComponent<BoxCollider>();
    if (this.backlog_manager.IsEnable())
      return;
    this.backlog_manager.StartBacklog(new Action(this.onLogCloseButton));
    ((Collider) this.box).enabled = false;
  }

  public void onLogCloseButton()
  {
    this.box = this.windowObj.GetComponent<BoxCollider>();
    if (!this.backlog_manager.IsEnable())
      return;
    this.backlog_manager.End();
    if (this.environment.current.select != null)
      ((Collider) this.box).enabled = false;
    else
      ((Collider) this.box).enabled = true;
  }

  public void onSelectButton00()
  {
    this.environment.current.setSelectIndex(0);
    this.environment.setNextLabel(this.environment.current.select.data[0].label);
    this.selectBtnObj[1].SetActive(false);
    this.selectBtnObj[2].SetActive(false);
    UISprite component = this.selectBtn[0].GetComponent<UISprite>();
    component.spriteName = this.selectBtn[0].GetComponent<UIButton>().pressedSprite;
    ((Component) component).GetComponent<Collider>().enabled = false;
    this.select_button_wait = this.select_wait;
    if (!this.onSelect)
      this.callEventSelected(0);
    this.onSelect = true;
    this.typewriterText = (string) null;
  }

  public void onSelectButton01()
  {
    this.environment.current.setSelectIndex(1);
    this.environment.setNextLabel(this.environment.current.select.data[1].label);
    this.selectBtnObj[0].SetActive(false);
    this.selectBtnObj[2].SetActive(false);
    UISprite component = this.selectBtn[1].GetComponent<UISprite>();
    component.spriteName = this.selectBtn[1].GetComponent<UIButton>().pressedSprite;
    ((Component) component).GetComponent<Collider>().enabled = false;
    this.select_button_wait = this.select_wait;
    if (!this.onSelect)
      this.callEventSelected(1);
    this.onSelect = true;
    this.typewriterText = (string) null;
  }

  public void onSelectButton02()
  {
    this.environment.current.setSelectIndex(2);
    this.environment.setNextLabel(this.environment.current.select.data[2].label);
    this.selectBtnObj[0].SetActive(false);
    this.selectBtnObj[1].SetActive(false);
    UISprite component = this.selectBtn[2].GetComponent<UISprite>();
    component.spriteName = this.selectBtn[2].GetComponent<UIButton>().pressedSprite;
    ((Component) component).GetComponent<Collider>().enabled = false;
    this.select_button_wait = this.select_wait;
    if (!this.onSelect)
      this.callEventSelected(2);
    this.onSelect = true;
    this.typewriterText = (string) null;
  }

  public void onResetButton()
  {
    this.re = true;
    this.environment.resetBlock();
    this.textContainerTop.SetActive(false);
    this.StartCoroutine(this.DeleteObject());
  }

  public void onBackButtonInStory()
  {
    this.environment.backBlock();
    this.render();
  }

  public void onWaitEnd()
  {
  }

  public void openTopLabelObject() => this.textContainerTop.SetActive(true);

  public void setNameMsgBright()
  {
    if (this.environment.current.text.pos == TextBlock.Position.BOTTOM)
    {
      for (int index = 0; index < this.textTopBox.Length; ++index)
      {
        ((UIWidget) this.textTopBox[index]).color = new Color(0.5f, 0.5f, 0.5f);
        ((UIWidget) this.textBottomBox[index]).color = new Color(1f, 1f, 1f);
        ((UIWidget) this.myselfSprite[index]).color = new Color(1f, 1f, 1f);
      }
      ((UIWidget) this.nameTopBox.Sprite).color = new Color(0.5f, 0.5f, 0.5f);
      ((UIWidget) this.nameBottomLabel).color = new Color(1f, 1f, 1f);
      ((UIWidget) this.nameTopLabel).color = new Color(0.5f, 0.5f, 0.5f);
      ((UIWidget) this.nameBottomBox.Sprite).color = new Color(1f, 1f, 1f);
      ((UIWidget) this.nextTopSprite).color = new Color(0.5f, 0.5f, 0.5f);
      ((UIWidget) this.nextBottomSprite).color = new Color(1f, 1f, 1f);
    }
    else
    {
      for (int index = 0; index < this.textTopBox.Length; ++index)
      {
        ((UIWidget) this.textTopBox[index]).color = new Color(1f, 1f, 1f);
        ((UIWidget) this.textBottomBox[index]).color = new Color(0.5f, 0.5f, 0.5f);
        ((UIWidget) this.myselfSprite[index]).color = new Color(0.5f, 0.5f, 0.5f);
      }
      ((UIWidget) this.nameTopBox.Sprite).color = new Color(1f, 1f, 1f);
      ((UIWidget) this.nameTopLabel).color = new Color(1f, 1f, 1f);
      ((UIWidget) this.nameBottomLabel).color = new Color(0.5f, 0.5f, 0.5f);
      ((UIWidget) this.nameBottomBox.Sprite).color = new Color(0.5f, 0.5f, 0.5f);
      ((UIWidget) this.nextTopSprite).color = new Color(1f, 1f, 1f);
      ((UIWidget) this.nextBottomSprite).color = new Color(0.5f, 0.5f, 0.5f);
    }
  }

  public void setTextBox()
  {
    if (this.environment.current.text.pos != TextBlock.Position.BOTTOM || this.textFlame)
      return;
    this.myselfTextBoxContainer.SetActive(false);
    this.charaTextBoxContainer.SetActive(true);
  }

  public void setBottomTextArrow(int n)
  {
    if (this.textBoxBaseBottomObj == null)
      return;
    for (int index = 0; index < this.textBoxBaseBottomObj.Length; ++index)
    {
      if (Object.op_Inequality((Object) this.textBoxBaseBottomObj[index], (Object) null))
        this.textBoxBaseBottomObj[index].SetActive(index == n);
    }
  }

  public void setTopTextArrow(int n)
  {
    if (this.textBoxBaseTopObj == null)
      return;
    for (int index = 0; index < this.textBoxBaseTopObj.Length; ++index)
    {
      if (Object.op_Inequality((Object) this.textBoxBaseTopObj[index], (Object) null))
        this.textBoxBaseTopObj[index].SetActive(index == n);
    }
  }

  private string getNameBoxSprite(int nameType)
  {
    string nameBoxSprite;
    switch (nameType)
    {
      case 1:
        nameBoxSprite = "pink";
        break;
      case 2:
        nameBoxSprite = "blue";
        break;
      case 3:
        nameBoxSprite = "green";
        break;
      default:
        nameBoxSprite = "default";
        break;
    }
    return nameBoxSprite;
  }

  public object setBottomName(string s = null, int nameType = -1)
  {
    if (this.textFlame)
    {
      this.myselfBase.SetActive(true);
      ((Component) this.nameBottomBox).gameObject.SetActive(false);
    }
    else
    {
      this.nameBottomBox.SetSpriteName<string>(this.getNameBoxSprite(nameType));
      ((Component) this.nameBottomBox).gameObject.SetActive(true);
      this.myselfBase.SetActive(false);
    }
    if (s == "" || s == "ななし")
    {
      ((Component) this.nameBottomBox).gameObject.SetActive(false);
      this.myselfBase.SetActive(false);
      s = "";
    }
    this.setNameMsgBright();
    this.nameBottomLabel.SetTextLocalize(s);
    this.cutinName = s;
    return (object) s;
  }

  public object setTopName(string s, int nameType = -1)
  {
    this.setNameMsgBright();
    this.nameTopBox.SetSpriteName<string>(this.getNameBoxSprite(nameType));
    ((Component) this.nameTopBox).gameObject.SetActive(true);
    if (s == "" || s == "ななし")
    {
      ((Component) this.nameTopBox).gameObject.SetActive(false);
      s = "";
    }
    this.nameTopLabel.SetTextLocalize(s);
    this.cutinName = s;
    return (object) s;
  }

  private UILabel GetLabelIncludeText(string text, UILabel target, int offset)
  {
    string text1 = Object.op_Equality((Object) target, (Object) this.textBottomLabel) ? string.Format("[{0}]{1}", (object) StoryExecuter.ColorDefault.Bottom, (object) text.Substring(0, offset)) : string.Format("[{0}]{1}", (object) StoryExecuter.ColorDefault.Top, (object) text.Substring(0, offset));
    target.SetTextLocalize(text1);
    return target;
  }

  private int getBoxName(string name)
  {
    int boxName;
    switch (name)
    {
      case "normal":
        boxName = 0;
        break;
      case "toge":
        boxName = 1;
        break;
      case "moya":
        boxName = 2;
        break;
      default:
        boxName = 0;
        break;
    }
    return boxName;
  }

  private bool storyContinuous()
  {
    if (this.continuousDataList == null)
      return false;
    if (this.continuousPopupView)
      return true;
    Story0093Scene.ContinuousData continuousData1 = (Story0093Scene.ContinuousData) null;
    foreach (Story0093Scene.ContinuousData continuousData2 in this.continuousDataList)
    {
      if (continuousData2.scriptId_ == this.scriptId)
      {
        continuousData1 = continuousData2;
        break;
      }
    }
    if (continuousData1 == null || !continuousData1.continuousFlag_)
      return false;
    int nextNum = this.continuousDataList.IndexOf(continuousData1) + 1;
    if (nextNum >= this.continuousDataList.Count)
      return false;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1044");
    else
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    PopupCommonNoYes.Show("シナリオ連続再生", "連続再生を行いますか？", (Action) (() =>
    {
      this.backlog_manager.BacklogDestroy();
      this.StartCoroutine(this.ContinuousCharaReset(nextNum));
    }), (Action) (() =>
    {
      this._isFinished = true;
      this.StartCoroutine(this.ContinuousCancel());
    }), isNonSe: true);
    this.continuousPopupView = true;
    return true;
  }

  private IEnumerator ContinuousCancel()
  {
    yield return (object) null;
    this.backScene();
  }

  private IEnumerator ContinuousCharaReset(int nextNum)
  {
    if ((double) this.fadeControl.toAlpha == 0.0)
    {
      this.setColorAndTime(0.0f, 0.0f, 0.0f, 0.0f, 1f, this.fadeInTime_);
      this.startFade();
      yield return (object) new WaitForSeconds(this.fadeInTime_);
    }
    foreach (KeyValuePair<int, StoryExecuter.CharacterInfo> character in this.characterList)
      Object.Destroy((Object) character.Value.obj);
    this.characterList.Clear();
    foreach (KeyValuePair<int, StoryExecuter.ImageInfo> keyValuePair in this.imageInfo)
    {
      UI2DSprite component = keyValuePair.Value.obj.GetComponent<UI2DSprite>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.sprite2D = Resources.Load<Sprite>("sprites/1x1_black");
      keyValuePair.Value.obj.SetActive(false);
    }
    this.startMoveButtons(0, false, 0.0f);
    this.startMoveFrame(0, false, 0.0f);
    if (Object.op_Inequality((Object) this.bgObj, (Object) null))
      Object.Destroy((Object) this.bgObj);
    this.setSubColorAndTime(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    this.startSubFillrect();
    this.isWaitAndNext = false;
    Story0093Scene.continuousChangeScene009_3(false, this.continuousDataList[nextNum].scriptId_);
  }

  private void waitColorTime(float t)
  {
  }

  public object setCutinName(float time, int num)
  {
    if (Object.op_Equality((Object) this.cutinObj, (Object) null) || Object.op_Equality((Object) this.CNM, (Object) null))
      return (object) -1;
    this.cutIn = true;
    this.cutInNum = num;
    this.CNM.startWait = time;
    this.CNM.characterName = this.cutinName;
    switch (num)
    {
      case 1:
        this.cutinObj.SendMessage("StartLeft");
        break;
      case 2:
        this.cutinObj.SendMessage("StartCenter");
        break;
      case 3:
        this.cutinObj.SendMessage("StartRight");
        break;
    }
    return (object) num;
  }

  public void setTextFlame(int n, int think = -1)
  {
    if (n == 0)
    {
      this.myselfTextBoxContainer.SetActive(true);
      this.charaTextBoxContainer.SetActive(false);
      if (this.myselfBoxBase.Length == 2)
      {
        if (think == 0)
        {
          this.myselfBoxBase[0].SetActive(true);
          this.myselfBoxBase[1].SetActive(false);
        }
        else
        {
          this.myselfBoxBase[0].SetActive(false);
          this.myselfBoxBase[1].SetActive(true);
        }
      }
      this.textFlame = true;
    }
    else
    {
      this.charaTextBoxContainer.SetActive(true);
      this.myselfTextBoxContainer.SetActive(false);
      this.textFlame = false;
    }
    this.setBottomName();
  }

  public object setBackGround(string s)
  {
    this.doStartCoroutine(this.InitBackGround(s));
    return (object) s;
  }

  private IEnumerator InitBackGround(string s)
  {
    if (!Object.op_Equality((Object) this.bgObj, (Object) null))
    {
      UI2DSprite sprite = this.bgObj.GetComponent<UI2DSprite>();
      this.sbg = this.bgObj.GetComponent<StoryBG>();
      Future<Sprite> spriteF = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Prefabs/BackGround/" + s);
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sprite.sprite2D = spriteF.Result;
      ((UIWidget) sprite).SetDirty();
      this.sbg.namePrefab = s;
    }
  }

  public void setWait(float t, bool moveNext = false)
  {
    this.isWaitAndNext = moveNext;
    this.wait_command_time = t;
  }

  public void setColorAndTime(float r, float g, float b, float a, float to, float t)
  {
    this.fadeControl.r = r;
    this.fadeControl.g = g;
    this.fadeControl.b = b;
    this.fadeAlpha = a;
    this.fadeControl.fromAlpha = this.fadeAlpha;
    this.fadeControl.toAlpha = to;
    this.fadeControl.time = t;
  }

  public object startFlush(Color color, float time, int value)
  {
    this.flushControl.Start(color, time, value);
    return (object) "";
  }

  public void startFillrect()
  {
    this.fillrect = true;
    this.fadeControl.StartFillrect();
  }

  public void startFade()
  {
    this.fadeIn = (double) this.fadeAlpha == 1.0;
    this.fadeControl.StartFade();
  }

  public object setTextTopWindow(string s)
  {
    this.textContainerTop.SetActive(true);
    int boxName = this.getBoxName(s);
    for (int index = 0; index < this.textTopFlame.Length; ++index)
    {
      if (index == boxName)
      {
        this.textTopFlame[boxName].SetActive(true);
        if (boxName == 2 && this.textTop3Nozzle != null)
        {
          ((IEnumerable<GameObject>) this.textTop3Nozzle).Where<GameObject>((Func<GameObject, bool>) (x => Object.op_Inequality((Object) x, (Object) null))).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
          if (this.charaPos == 1 || this.charaPos == 2)
            this.setOnTextTopNozzle(0);
          else if (this.charaPos == 3)
            this.setOnTextTopNozzle(1);
          else
            this.setOnTextTopNozzle(2);
        }
      }
      else
        this.textTopFlame[index].SetActive(false);
    }
    return (object) s;
  }

  private void setOnTextTopNozzle(int index)
  {
    if (this.textTop3Nozzle.Length <= index || !Object.op_Inequality((Object) this.textTop3Nozzle[index], (Object) null))
      return;
    this.textTop3Nozzle[index].SetActive(true);
  }

  public object setTextBottomWindow(string s)
  {
    this.textContainerBottom.SetActive(true);
    int boxName = this.getBoxName(s);
    if (this.textFlame)
    {
      for (int index = 0; index < this.myselfTextBox.Length; ++index)
      {
        if (index == boxName)
          this.myselfTextBox[boxName].SetActive(true);
        else
          this.myselfTextBox[index].SetActive(false);
      }
    }
    else
    {
      this.charaTextBoxContainer.SetActive(true);
      for (int index = 0; index < this.myselfTextBox.Length; ++index)
      {
        if (index == 0)
          this.myselfTextBox[index].SetActive(true);
        else
          this.myselfTextBox[index].SetActive(false);
      }
      this.myselfTextBoxContainer.SetActive(false);
      for (int index = 0; index < this.textBottomFlame.Length; ++index)
      {
        if (index == boxName)
        {
          this.textBottomFlame[boxName].SetActive(true);
          if (boxName == 2 && this.textBottom3Nozzle != null)
          {
            ((IEnumerable<GameObject>) this.textBottom3Nozzle).Where<GameObject>((Func<GameObject, bool>) (x => Object.op_Inequality((Object) x, (Object) null))).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
            if (this.charaPos == 1 || this.charaPos == 2)
              this.setOnTextBottomNozzle(0);
            else if (this.charaPos == 3)
              this.setOnTextBottomNozzle(1);
            else
              this.setOnTextBottomNozzle(2);
          }
        }
        else
          this.textBottomFlame[index].SetActive(false);
      }
    }
    return (object) s;
  }

  private void setOnTextBottomNozzle(int index)
  {
    if (this.textBottom3Nozzle.Length <= index || !Object.op_Inequality((Object) this.textBottom3Nozzle[index], (Object) null))
      return;
    this.textBottom3Nozzle[index].SetActive(true);
  }

  public void setTextClose(TextBlock.Position type)
  {
    if (type == TextBlock.Position.TOP)
      this.textContainerTop.SetActive(false);
    else
      this.textContainerBottom.SetActive(false);
  }

  public void setTextSize(int size, bool b)
  {
    if (b)
      this.textTopLabel.fontSize = size;
    else
      this.textBottomLabel.fontSize = size;
  }

  private string TranslateColor(string color)
  {
    switch (color)
    {
      case "black":
        color = "000000";
        break;
      case "blue":
        color = "0000FF";
        break;
      case "green":
        color = "00FF00";
        break;
      case "nomal":
      case "normal":
        color = this.rgbTextNormal_;
        break;
      case "pink":
        color = "FFBFCC";
        break;
      case "red":
        color = "FF0000";
        break;
      case "white":
        color = "FFFFFF";
        break;
    }
    return color;
  }

  public void SetColorText(TextBlock.Position pos, string color)
  {
    string str = this.TranslateColor(color);
    if (pos == TextBlock.Position.TOP)
      StoryExecuter.ColorDefault.Top = str;
    else
      StoryExecuter.ColorDefault.Bottom = str;
  }

  public void SetTextAlgin(TextBlock.Position pos, string align)
  {
    UILabel uiLabel = pos == TextBlock.Position.TOP ? this.textTopLabel : this.textBottomLabel;
    NGUIText.Alignment alignment;
    switch (align)
    {
      case "center":
        alignment = (NGUIText.Alignment) 2;
        break;
      case "left":
        alignment = (NGUIText.Alignment) 1;
        break;
      case "right":
        alignment = (NGUIText.Alignment) 3;
        break;
      case "just":
        alignment = (NGUIText.Alignment) 4;
        break;
      case "auto":
        alignment = (NGUIText.Alignment) 0;
        break;
      default:
        alignment = (NGUIText.Alignment) 1;
        break;
    }
    uiLabel.alignment = alignment;
  }

  public void setTextShake(float t, bool b)
  {
    if (b)
    {
      this.textTopShake = this.textTopObj.GetComponent<TextShake>();
      this.textTopShake.timer = t;
      this.textTopObj.SendMessage("StartTextShake");
    }
    else
    {
      this.textBottomShake = this.textBottomObj.GetComponent<TextShake>();
      this.textBottomShake.timer = t;
      this.textBottomObj.SendMessage("StartTextShake");
    }
  }

  public void stopTextShake(bool b)
  {
    if (b)
      this.textTopObj.SendMessage("StopTextShake");
    else
      this.textBottomObj.SendMessage("StopTextShake");
  }

  public object setShake(float w, float t)
  {
    this.shakecontrol = this.windowObj.GetComponent<ShakeControl>();
    this.shakecontrol.wieght = w;
    this.shakecontrol.waitTime = t;
    this.windowObj.SendMessage("StartShake");
    return (object) t;
  }

  public void stopShake() => this.windowObj.SendMessage("StopShake");

  public object setStack(int n) => (object) n;

  public object setEmotion() => (object) "";

  public object deleteEmotion() => (object) "";

  public object setEmotionBright() => (object) "";

  public object setSe(string clip, float delay = 0.0f)
  {
    if (this.plsySEDataList == null)
      this.plsySEDataList = new List<PlaySEData>();
    this.sm = Singleton<NGSoundManager>.GetInstance();
    this.plsySEDataList.Add(new PlaySEData(clip, this.sm.playSE(clip, delay: delay)));
    return (object) clip;
  }

  public object stopSe(string clip, float delay = 0.0f)
  {
    if (this.plsySEDataList == null || this.plsySEDataList.Count <= 0)
      return (object) clip;
    PlaySEData playSeData = this.plsySEDataList.Find((Predicate<PlaySEData>) (x => x.clip == clip));
    if (playSeData == null)
      return (object) clip;
    this.plsySEDataList.Remove(playSeData);
    this.StartCoroutine(this.delayStopSe(playSeData.ch, delay));
    return (object) clip;
  }

  public IEnumerator delayStopSe(int ch, float delay = 0.0f)
  {
    yield return (object) new WaitForSeconds(delay);
    this.sm = Singleton<NGSoundManager>.GetInstance();
    this.sm.stopSE(ch);
  }

  public void stopSeAll()
  {
    if (this.plsySEDataList == null)
      return;
    this.plsySEDataList.ForEach((Action<PlaySEData>) (x => this.StartCoroutine(this.delayStopSe(x.ch))));
  }

  public object setVoice(string charaID, string name, float delay = 0.0f)
  {
    if (!this.skip_enable)
    {
      this.sm = Singleton<NGSoundManager>.GetInstance();
      int voiceno = this.sm.playVoiceByStringID("VO_" + charaID, name, delay: delay);
      this.addWaitAutoPlay((Func<bool>) (() => this.sm.IsVoicePlaying(voiceno)));
    }
    return (object) name;
  }

  public void stopVoiceAll() => Singleton<NGSoundManager>.GetInstance().StopVoice();

  public void setBgm(string s, float time = 0.7f)
  {
    Singleton<NGSoundManager>.GetInstance().PlayBgm(s, fadeInTime: time, fadeOutTime: time);
  }

  public void setBgmFile(string file, string s, float time = 0.7f)
  {
    Singleton<NGSoundManager>.GetInstance().PlayBgmFile(file, s, fadeInTime: time, fadeOutTime: time);
  }

  public void setNextBgmBlock(int blockIndex)
  {
    Singleton<NGSoundManager>.GetInstance().SetNextBGMBlock(0, blockIndex);
  }

  public void stopBgm()
  {
    this.sm = Singleton<NGSoundManager>.GetInstance();
    this.sm.stopBGMWithFadeOut(this.stopBGMTime);
  }

  public void StartSelect(SelectBlock sb)
  {
    this.selectObj.SetActive(true);
    for (int index = 0; index < 3; ++index)
    {
      if (index < sb.data.Count)
      {
        this.select[index].SetActive(true);
        this.SelectLabel[index].SetTextLocalize(sb.data[index].msg);
      }
    }
    if (sb.data.Count > 0)
      this.addWaitAutoPlay((Func<bool>) (() => true));
    this.InitStartSelect();
    this.quastion.SetTextLocalize(this.environment.current.text.text);
    this.skip_enable = false;
  }

  private void InitStartSelect()
  {
    foreach (GameObject gameObject in this.select)
    {
      UISprite componentInChildren = gameObject.GetComponentInChildren<UISprite>();
      if (!Object.op_Equality((Object) componentInChildren, (Object) null))
      {
        ((UIWidget) componentInChildren).color = Color.white;
        ((Component) componentInChildren).GetComponent<Collider>().enabled = true;
      }
    }
  }

  public void PopupStoryEffect(string label) => this.doStartCoroutine(this.ShowStoryEffect(label));

  private IEnumerator ShowStoryEffect(string label)
  {
    Singleton<CommonRoot>.GetInstance().isActiveBlackBGPanel = true;
    Future<GameObject> prefabF = Res.Prefabs.quest052_8.P0_Quest_Title.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result.Clone();
    prefab.GetComponent<QuestStartAnim>().StartAnim(label);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private void resetCoroutine() => this.coroutineCount = 0;

  private IEnumerator execCoroutine(IEnumerator e)
  {
    ++this.coroutineCount;
    while (e.MoveNext())
      yield return e.Current;
    --this.coroutineCount;
  }

  private void doStartCoroutine(IEnumerator e) => this.StartCoroutine(this.execCoroutine(e));

  public bool isRenderDone => this.coroutineCount == 0;

  public void PlayMovie(string fileName, bool enabledSkip)
  {
    if (this.skip_enable)
      return;
    NGSoundManager sm = Singleton<NGSoundManager>.GetInstance();
    string nowBgmName = sm.GetBgmName();
    ((Behaviour) this).enabled = false;
    this.movieObj.Attach(fileName, enabledSkip, (Action) (() =>
    {
      sm.PlayBgm(nowBgmName);
      this.movieObj.ShowMainPanel();
      StatusBarHelper.SetVisibility(false);
      ((Behaviour) this).enabled = true;
    }));
  }

  public override void onBackButton()
  {
    if (this.backlog_manager.IsEnable())
      return;
    this.showBackKeyToast();
  }

  public StoryExecuter.CharacterInfo setHenshin(
    int uniqueId,
    int dataId,
    int beforeId,
    int afterId)
  {
    StoryExecuter.CharacterInfo characterInfo1;
    StoryExecuter.CharacterInfo characterInfo2;
    StoryExecuter.CharacterInfo characterInfo3;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo1) && this.characterList.TryGetValue(beforeId, out characterInfo2) && this.characterList.TryGetValue(afterId, out characterInfo3) && characterInfo2.parentId == 0 && characterInfo3.parentId == 0)
    {
      characterInfo1 = new StoryExecuter.CharacterInfo();
      this.characterList.Add(uniqueId, characterInfo1);
      characterInfo1.obj = Object.Instantiate<GameObject>(this.CharacterPrefab);
      characterInfo1.obj.transform.localPosition = Vector3.zero;
      characterInfo1.obj.transform.localScale = Vector3.one;
      characterInfo1.obj.transform.parent = this.windowObj.transform;
      ((Behaviour) characterInfo1.obj.GetComponent<TweenPosition>()).enabled = false;
      characterInfo2.parentId = uniqueId;
      characterInfo3.parentId = uniqueId;
      characterInfo1.child = new int[2]{ beforeId, afterId };
      this.doStartCoroutine(characterInfo1.obj.GetComponent<HenshinControl>().coSetUnit(dataId, characterInfo2.obj, characterInfo3.obj));
    }
    return characterInfo1;
  }

  public void startHenshin(int uniqueId)
  {
    StoryExecuter.CharacterInfo characterInfo;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo))
      return;
    characterInfo.obj.GetComponent<HenshinControl>().startHenshin();
  }

  public void skipHenshin(int uniqueId)
  {
    StoryExecuter.CharacterInfo characterInfo;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo))
      return;
    characterInfo.obj.GetComponent<HenshinControl>().skipHenshin();
  }

  public StoryExecuter.CharacterInfo setPerson(int unique_id, int chara_id, int? job_id = null)
  {
    StoryExecuter.CharacterInfo characterInfo;
    if (!this.characterList.TryGetValue(unique_id, out characterInfo))
    {
      characterInfo = new StoryExecuter.CharacterInfo();
      this.characterList.Add(unique_id, characterInfo);
      characterInfo.obj = Object.Instantiate<GameObject>(this.CharacterPrefab);
      characterInfo.obj.transform.localPosition = new Vector3(500f, 0.0f, 0.0f);
      characterInfo.obj.transform.localScale = Vector3.one;
      characterInfo.obj.transform.parent = this.windowObj.transform;
      ((Behaviour) characterInfo.obj.GetComponent<TweenPosition>()).enabled = false;
      this.getUnit(unique_id, chara_id, this.characterList.Count<KeyValuePair<int, StoryExecuter.CharacterInfo>>(), job_id);
    }
    return characterInfo;
  }

  private void getUnit(int unique_id, int chara_id, int layer = 1, int? ext_id = null)
  {
    StoryExecuter.CharacterInfo character = this.characterList[unique_id];
    character.image = this.storyResource.GetCharacterPrefab(unique_id).Clone(character.obj.transform);
    if (chara_id > 999)
      MasterData.UnitUnit[chara_id].SetStoryData(character.image, ext_id: ext_id);
    character.obj.GetComponent<Clash>().windowObj = this.mainPanel;
    character.image.GetComponent<NGxMaskSpriteWithScale>().MainUI2DSprite.sprite2D = this.storyResource.GetLargeTexture(unique_id);
    this.setMaskChange(character);
    UIWidget component1 = character.image.GetComponent<UIWidget>();
    component1.depth = 5;
    Transform[] componentsInChildren = ((Component) character.image.transform).GetComponentsInChildren<Transform>();
    foreach (Component component2 in ((IEnumerable<Transform>) componentsInChildren).Where<Transform>((Func<Transform, bool>) (v => ((Object) v).name == "face")))
      component2.GetComponent<UIWidget>().depth = component1.depth + 1;
    foreach (Component component3 in ((IEnumerable<Transform>) componentsInChildren).Where<Transform>((Func<Transform, bool>) (v => ((Object) v).name == "eye")))
      component3.GetComponent<UIWidget>().depth = component1.depth + 2;
  }

  public void setMaskChange(int id) => this.setMaskChange(this.setPerson(id, id));

  public void setMaskChange(StoryExecuter.CharacterInfo chara)
  {
    if (Object.op_Equality((Object) chara.image, (Object) null))
      return;
    NGxMaskSpriteWithScale component = chara.image.GetComponent<NGxMaskSpriteWithScale>();
    if (Object.op_Equality((Object) component, (Object) null))
      return;
    int index = Mathf.Clamp(chara.pos - 1, 0, this.mSprite.Length - 1);
    component.maskTexture = this.mSprite[index];
  }

  public void setCharaPosition(int id, int pos)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    chara.pos = pos;
    chara.obj.transform.localPosition = this.positions[pos - 1];
    this.setMaskChange(chara);
  }

  public void getCharaPosition(int id, int? jobid = null)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id, jobid);
    this.charaPos = chara.pos;
    this.setMaskChange(chara);
  }

  public void setFace(int id, string s)
  {
    StoryExecuter.CharacterInfo characterInfo = this.setPerson(id, id);
    characterInfo.faceName = s;
    NGxFaceSprite component = characterInfo.image.GetComponent<NGxFaceSprite>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    this.doStartCoroutine(component.ChangeFace(s));
  }

  public void setEye(int id, string s)
  {
    StoryExecuter.CharacterInfo characterInfo = this.setPerson(id, id);
    characterInfo.eyeName = s;
    NGxEyeSprite component = characterInfo.image.GetComponent<NGxEyeSprite>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    ((Behaviour) component).enabled = true;
    ((Component) component.EyeUI2DSprite).gameObject.SetActive(true);
    this.doStartCoroutine(component.ChangeEye(s));
  }

  public void setCharaMoveIn(int id, float time, float pos)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    Vector3 localPosition = chara.obj.transform.localPosition;
    chara.obj.transform.localPosition = new Vector3(pos, 0.0f, 0.0f);
    if (this.skip_enable)
      TweenPosition.Begin(chara.obj, 0.1f, localPosition);
    else
      TweenPosition.Begin(chara.obj, time, localPosition);
    chara.pos = (double) localPosition.x >= 0.0 ? ((double) localPosition.x <= 0.0 ? 2 : 4) : 0;
    this.setMaskChange(chara);
  }

  public void setCharaMoveOut(int id, float time, float pos)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    if (this.skip_enable)
      TweenPosition.Begin(chara.obj, 0.1f, new Vector3(pos, 0.0f, 0.0f));
    else
      TweenPosition.Begin(chara.obj, time, new Vector3(pos, 0.0f, 0.0f));
    this.setMaskChange(chara);
  }

  public void setCharaScale(int id, float size, float time)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    if (this.skip_enable)
      TweenScale.Begin(chara.obj, 0.1f, new Vector3(size, size, 1f));
    else
      TweenScale.Begin(chara.obj, time, new Vector3(size, size, 1f));
    this.setMaskChange(chara);
  }

  public void setJump(int id)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    this.jump = chara.obj.GetComponent<Jump>();
    this.jump.state = Jump.JumpEffect.Start;
    this.setMaskChange(chara);
  }

  public void setClash(int id)
  {
    Clash component = this.setPerson(id, id).obj.GetComponent<Clash>();
    if (this.skip_enable)
      component.isSkip = true;
    component.state = Clash.State.Start;
  }

  public void setMoveChara(int id, int pos, float time)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    this.charaPos = chara.pos = pos;
    TweenPosition.Begin(chara.obj, this.skip_enable ? 0.1f : time, this.positions[pos - 1]);
    this.setMaskChange(chara);
  }

  public void setCharaReversal(int id, bool b)
  {
    this.setPerson(id, id).obj.transform.localRotation = new Quaternion(0.0f, b ? 180f : 0.0f, 0.0f, 0.0f);
  }

  public void setCharaBrightness(int id, float c, float t)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    GameObject image = chara.image;
    TweenColor component1 = image.GetComponent<TweenColor>();
    GameObject gameObject = ((Component) image.transform.Find("face")).gameObject;
    TweenColor component2 = gameObject.GetComponent<TweenColor>();
    if (Object.op_Equality((Object) component1, (Object) null) && Object.op_Equality((Object) component2, (Object) null))
    {
      image.AddComponent<TweenColor>();
      gameObject.AddComponent<TweenColor>();
    }
    if (this.skip_enable)
    {
      TweenColor.Begin(image, 0.1f, new Color(c, c, c));
      TweenColor.Begin(gameObject, 0.1f, new Color(c, c, c));
    }
    else
    {
      TweenColor.Begin(image, t, new Color(c, c, c));
      TweenColor.Begin(gameObject, t, new Color(c, c, c));
    }
    this.setMaskChange(chara);
  }

  public void setCharaAlpha(int id, float alpha, float time)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    TweenAlpha.Begin(chara.obj, this.skip_enable ? 0.1f : time, alpha);
    this.setMaskChange(chara);
  }

  public void setCharaLayer(int id, int depth)
  {
    StoryExecuter.CharacterInfo chara = this.setPerson(id, id);
    UIWidget w = chara.image.GetComponent<UIWidget>();
    w.depth = (depth + 1) * 5;
    Transform[] componentsInChildren = ((Component) chara.image.transform).GetComponentsInChildren<Transform>();
    ((IEnumerable<Transform>) componentsInChildren).Where<Transform>((Func<Transform, bool>) (v => ((Object) v).name == "face")).ForEach<Transform>((Action<Transform>) (v => ((Component) v).GetComponent<UIWidget>().depth = w.depth + 1));
    ((IEnumerable<Transform>) componentsInChildren).Where<Transform>((Func<Transform, bool>) (v => ((Object) v).name == "eye")).ForEach<Transform>((Action<Transform>) (v => ((Component) v).GetComponent<UIWidget>().depth = w.depth + 2));
    this.setMaskChange(chara);
  }

  public void deleteUnit(int id)
  {
    StoryExecuter.CharacterInfo info;
    if (!this.characterList.TryGetValue(id, out info))
      return;
    if (info.child != null && info.child.Length != 0)
    {
      foreach (int key in this.getChildIdsInCharacterInfo(info))
        this.characterList.Remove(key);
    }
    StoryExecuter.CharacterInfo characterInfo;
    if (info.parentId != 0 && this.characterList.TryGetValue(info.parentId, out characterInfo))
      characterInfo.child = ((IEnumerable<int>) characterInfo.child).Where<int>((Func<int, bool>) (i => i != id)).ToArray<int>();
    Object.Destroy((Object) info.obj);
    this.characterList.Remove(id);
  }

  private List<int> getChildIdsInCharacterInfo(StoryExecuter.CharacterInfo info)
  {
    List<int> idsInCharacterInfo = new List<int>();
    foreach (int key in info.child)
    {
      StoryExecuter.CharacterInfo character = this.characterList[key];
      if (character.child != null && character.child.Length != 0)
        idsInCharacterInfo.AddRange((IEnumerable<int>) this.getChildIdsInCharacterInfo(character));
      idsInCharacterInfo.Add(key);
    }
    return idsInCharacterInfo;
  }

  public void SetMaskEnable(int id, bool enable)
  {
    StoryExecuter.CharacterInfo characterInfo = this.setPerson(id, id);
    NGxMaskSpriteWithScale component = Object.op_Inequality((Object) characterInfo.image, (Object) null) ? characterInfo.image.GetComponent<NGxMaskSpriteWithScale>() : (NGxMaskSpriteWithScale) null;
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.SetMaskEnable(enable);
  }

  public void stopDistinction()
  {
  }

  public void setUnitDistinction(int id, int which)
  {
  }

  public StoryExecuter.CharacterInfo setEmotion(
    int uniqueId,
    int dataId,
    int noColor,
    int parentId,
    int offsetX,
    int offsetY)
  {
    StoryExecuter.CharacterInfo characterInfo1;
    StoryExecuter.CharacterInfo characterInfo2;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo1) && this.characterList.TryGetValue(parentId, out characterInfo2))
    {
      characterInfo1 = new StoryExecuter.CharacterInfo();
      this.characterList.Add(uniqueId, characterInfo1);
      characterInfo1.obj = Object.Instantiate<GameObject>(this.CharacterPrefab);
      characterInfo1.obj.transform.parent = characterInfo2.obj.transform;
      characterInfo1.obj.transform.localPosition = Vector2.op_Implicit(new Vector2((float) offsetX, (float) offsetY));
      characterInfo1.obj.transform.localScale = Vector3.one;
      characterInfo1.obj.transform.localRotation = Quaternion.identity;
      ((Behaviour) characterInfo1.obj.GetComponent<TweenPosition>()).enabled = false;
      characterInfo1.parentId = parentId;
      StoryExecuter.CharacterInfo characterInfo3 = characterInfo2;
      int[] numArray;
      if (characterInfo2.child == null)
        numArray = new int[1]{ uniqueId };
      else
        numArray = ((IEnumerable<int>) characterInfo2.child).Concat<int>((IEnumerable<int>) new int[1]
        {
          uniqueId
        }).ToArray<int>();
      characterInfo3.child = numArray;
      this.doStartCoroutine(characterInfo1.obj.GetComponent<StoryEffectControl>().coInitializeEmotion(dataId, noColor));
    }
    return characterInfo1;
  }

  public StoryExecuter.CharacterInfo setEnvEffect(int uniqueId, int dataId, int noColor)
  {
    StoryExecuter.CharacterInfo characterInfo;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo))
    {
      characterInfo = new StoryExecuter.CharacterInfo();
      this.characterList.Add(uniqueId, characterInfo);
      characterInfo.obj = Object.Instantiate<GameObject>(this.CharacterPrefab);
      characterInfo.obj.transform.parent = this.windowObj.transform;
      characterInfo.obj.transform.localPosition = Vector3.zero;
      characterInfo.obj.transform.localScale = Vector3.one;
      characterInfo.obj.transform.localRotation = Quaternion.identity;
      ((Behaviour) characterInfo.obj.GetComponent<TweenPosition>()).enabled = false;
      this.doStartCoroutine(characterInfo.obj.GetComponent<StoryEffectControl>().coInitializeEnvironment(dataId, noColor));
    }
    return characterInfo;
  }

  public StoryExecuter.CharacterInfo setEffect(
    int uniqueId,
    int dataId,
    int noColor,
    int positionX,
    int positionY)
  {
    StoryExecuter.CharacterInfo characterInfo;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo))
    {
      characterInfo = new StoryExecuter.CharacterInfo();
      this.characterList.Add(uniqueId, characterInfo);
      characterInfo.obj = Object.Instantiate<GameObject>(this.CharacterPrefab);
      characterInfo.obj.transform.parent = this.windowObj.transform;
      characterInfo.obj.transform.localPosition = Vector2.op_Implicit(new Vector2((float) positionX, (float) positionY));
      characterInfo.obj.transform.localScale = Vector3.one;
      characterInfo.obj.transform.localRotation = Quaternion.identity;
      ((Behaviour) characterInfo.obj.GetComponent<TweenPosition>()).enabled = false;
      this.doStartCoroutine(characterInfo.obj.GetComponent<StoryEffectControl>().coInitializeEffect(dataId, noColor));
    }
    return characterInfo;
  }

  public void startEffect(int uniqueId)
  {
    StoryExecuter.CharacterInfo characterInfo;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo))
      return;
    characterInfo.obj.GetComponent<StoryEffectControl>().startEffect();
  }

  public void skipEffect(int uniqueId)
  {
    StoryExecuter.CharacterInfo characterInfo;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo))
      return;
    characterInfo.obj.GetComponent<StoryEffectControl>().skipEffect();
  }

  public void changeEffect(int uniqueId, int noPattern, int noColor)
  {
    StoryExecuter.CharacterInfo characterInfo;
    if (!this.characterList.TryGetValue(uniqueId, out characterInfo))
      return;
    characterInfo.obj.GetComponent<StoryEffectControl>().changeEffect(noPattern, noColor);
  }

  public GameObject topButtons => this.topButtons_;

  public void setSubDepth(int index)
  {
    if (this.subFillrectDepths_ == null || index < 0 || index >= this.subFillrectDepths_.Length || !Object.op_Inequality((Object) this.fadeSubControl_, (Object) null))
      return;
    this.fadeSubControl_.setDepth(this.subFillrectDepths_[index]);
  }

  public void setSubColorAndTime(float r, float g, float b, float a, float to, float t)
  {
    if (Object.op_Equality((Object) this.fadeSubControl_, (Object) null))
      return;
    this.fadeSubControl_.r = r;
    this.fadeSubControl_.g = g;
    this.fadeSubControl_.b = b;
    this.fadeSubControl_.fromAlpha = a;
    this.fadeSubControl_.toAlpha = to;
    this.fadeSubControl_.time = t;
  }

  public void startSubFillrect()
  {
    this.fillrectSub_ = true;
    if (!Object.op_Inequality((Object) this.fadeSubControl_, (Object) null))
      return;
    this.fadeSubControl_.StartFillrect();
  }

  private void fadeSubNextExit()
  {
    if (!this.fillrectSub_)
      return;
    this.fillrectSub_ = false;
    if (!Object.op_Inequality((Object) this.fadeSubControl_, (Object) null))
      return;
    this.fadeSubControl_.time = 0.0f;
    this.fadeSubControl_.StartFillrect();
  }

  public void startMoveFrame(int index, bool toOut, float duration)
  {
    if (Object.op_Equality((Object) this.frameTop_, (Object) null) || Object.op_Equality((Object) this.frameBottom_, (Object) null))
      return;
    if (toOut)
    {
      if (index < 0 || this.frameOutPositions_ == null || this.frameOutPositions_.Length <= index)
        return;
      this.moveEndDeleteControl(this.frameTop_, duration, Vector2.op_Implicit(this.frameOutPositions_[index].top_));
      this.moveEndDeleteControl(this.frameBottom_, duration, Vector2.op_Implicit(this.frameOutPositions_[index].bottom_));
    }
    else
    {
      this.moveEndDeleteControl(this.frameTop_, duration, Vector3.zero);
      this.moveEndDeleteControl(this.frameBottom_, duration, Vector3.zero);
    }
  }

  public void startMoveButtons(int index, bool toOut, float duration)
  {
    if (Object.op_Equality((Object) this.topButtons_, (Object) null))
      return;
    if (toOut)
    {
      if (index < 0 || this.buttonOutPositions_ == null || this.buttonOutPositions_.Length <= index)
        return;
      this.moveEndDeleteControl(this.topButtons_, duration, Vector2.op_Implicit(this.buttonOutPositions_[index]));
    }
    else
      this.moveEndDeleteControl(this.topButtons_, duration, Vector3.zero);
  }

  private void moveEndDeleteControl(GameObject go, float duration, Vector3 to)
  {
    TweenPosition cntl = TweenPosition.Begin(go, duration, to);
    EventDelegate.Add(((UITweener) cntl).onFinished, (EventDelegate.Callback) (() => Object.Destroy((Object) cntl)), true);
  }

  public void setImageName(int id, string name)
  {
    if (!this.imageInfo.ContainsKey(id))
      this.imageInfo.Add(id, new StoryExecuter.ImageInfo());
    if (name == "")
    {
      this.imageObj[id].SetActive(false);
    }
    else
    {
      this.imageObj[id].SetActive(true);
      this.imageInfo[id].obj = this.imageObj[id];
      this.imageInfo[id].name = name;
      Singleton<ResourceManager>.GetInstance().Load<Sprite>("AssetBundle/Resources/EventImages/" + name).RunOn<Sprite>((MonoBehaviour) this, (Action<Sprite>) (s =>
      {
        UI2DSprite component = this.imageInfo[id].obj.GetComponent<UI2DSprite>();
        component.sprite2D = s;
        Rect textureRect1 = s.textureRect;
        ((UIWidget) component).width = (int) ((Rect) ref textureRect1).width;
        Rect textureRect2 = s.textureRect;
        ((UIWidget) component).height = (int) ((Rect) ref textureRect2).height;
        ((Component) component).transform.localPosition = new Vector3(2500f, 0.0f, 0.0f);
      }));
    }
  }

  public void setImagePriority(int id, int priority)
  {
    if (!this.imageInfo.ContainsKey(id))
      this.imageInfo.Add(id, new StoryExecuter.ImageInfo());
    this.imageInfo[id].priority = priority;
    ((UIWidget) this.imageInfo[id].obj.GetComponent<UI2DSprite>()).depth = priority + 100;
  }

  public void setImagePosition(int id, float x, float y)
  {
    if (!this.imageInfo.ContainsKey(id))
      this.imageInfo.Add(id, new StoryExecuter.ImageInfo());
    this.imageInfo[id].x = x;
    this.imageInfo[id].y = y;
    this.imageInfo[id].obj.transform.localPosition = new Vector3(x, y, 0.0f);
  }

  public void setImageAlpha(int id, float alpha, float time)
  {
    if (!this.imageInfo.ContainsKey(id))
      this.imageInfo.Add(id, new StoryExecuter.ImageInfo());
    this.imageInfo[id].alpha = alpha;
    TweenAlpha.Begin(this.imageInfo[id].obj, this.skip_enable ? 0.1f : time, alpha);
  }

  public void setImageScale(int id, float scale, float time)
  {
    if (!this.imageInfo.ContainsKey(id))
      this.imageInfo.Add(id, new StoryExecuter.ImageInfo());
    this.imageInfo[id].scale = scale;
    TweenScale.Begin(this.imageInfo[id].obj, this.skip_enable ? 0.1f : time, new Vector3(scale, scale, 1f));
  }

  public void setImageMoveIn(int id, float time, float x, float y)
  {
    if (!this.imageInfo.ContainsKey(id))
      this.imageInfo.Add(id, new StoryExecuter.ImageInfo());
    GameObject gameObject = this.imageInfo[id].obj;
    Vector3 localPosition = gameObject.transform.localPosition;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(localPosition.x, localPosition.y, localPosition.z);
    gameObject.transform.localPosition = new Vector3(x, y, localPosition.z);
    TweenPosition.Begin(gameObject, this.skip_enable ? 0.1f : time, vector3);
  }

  public void setImageMoveOut(int id, float time, float x, float y)
  {
    if (!this.imageInfo.ContainsKey(id))
      this.imageInfo.Add(id, new StoryExecuter.ImageInfo());
    GameObject gameObject = this.imageInfo[id].obj;
    Vector3 localPosition = gameObject.transform.localPosition;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(x, y, localPosition.z);
    TweenPosition.Begin(gameObject, this.skip_enable ? 0.1f : time, vector3);
  }

  public enum ScriptMode
  {
    Normal,
    Date,
  }

  private static class ColorDefault
  {
    public static string Top { get; set; }

    public static string Bottom { get; set; }
  }

  public class CharacterInfo
  {
    public GameObject obj;
    public GameObject image;
    public int pos;
    public string faceName = "normal";
    public string eyeName = "normal";
    public int parentId;
    public int[] child;
  }

  [Serializable]
  private class FramePositions
  {
    public Vector2 top_ = Vector2.zero;
    public Vector2 bottom_ = Vector2.zero;
  }

  private class ImageInfo
  {
    public GameObject obj;
    public string name;
    public int priority;
    public float x;
    public float y;
    public float alpha;
    public float scale;
  }
}
