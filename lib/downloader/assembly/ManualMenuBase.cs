// Decompiled with JetBrains decompiler
// Type: ManualMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Utility/Behaviour/ManualMenuBase")]
public abstract class ManualMenuBase : BackButtonMenuBase
{
  [SerializeField]
  protected NGxScrollMasonry scroll_;
  private float height_;
  private List<Texture2D> textures_;

  protected abstract string getTitle();

  protected abstract ManualMenuBase.BodyParam[] getBodies();

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  protected IEnumerator doInitialize()
  {
    ManualMenuBase.BodyParam[] bodies = this.getBodies();
    IEnumerator e = this.loadDetailResource(bodies);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.height_ = 0.0f;
    int panel_width = (int) ((Component) this.scroll_.Scroll).GetComponent<UIPanel>().width;
    Future<GameObject> textPrefabF = Res.Prefabs.dynamic_display.masonryTextBox.Load<GameObject>();
    e = textPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> spritePrefabF = Res.Prefabs.dynamic_display.masonrySpriteBox.Load<GameObject>();
    e = spritePrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> parameterPrefabF = Res.Prefabs.dynamic_display.dir_hime_param.Load<GameObject>();
    e = parameterPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject textPrefab = textPrefabF.Result;
    GameObject spritePrefab = spritePrefabF.Result;
    GameObject parameterPrefab = parameterPrefabF.Result;
    this.setTitleText(this.getTitle(), ((Component) this.scroll_.Scroll).transform, textPrefab);
    for (int i = 0; i < bodies.Length; ++i)
    {
      ManualMenuBase.BodyParam bodyParam = bodies[i];
      if (!string.IsNullOrEmpty(bodyParam.image_url))
      {
        if (Object.op_Equality((Object) this.textures_[i], (Object) null))
          Debug.LogWarning((object) ("画像の取得失敗 path:" + bodyParam.image_url));
        else
          this.addSprite(Sprite.Create(this.textures_[i], new Rect(0.0f, 0.0f, (float) ((Texture) this.textures_[i]).width, (float) ((Texture) this.textures_[i]).height), new Vector2(0.0f, 0.0f), 1f, 100U, (SpriteMeshType) 0), spritePrefab, bodyParam.scene_name, bodyParam.param_name, bodyParam.image_width, bodyParam.image_height);
      }
      if (!string.IsNullOrEmpty(bodyParam.body))
        this.addText(bodyParam.body, panel_width, textPrefab);
      if (bodyParam.extra_type.HasValue && bodyParam.extra_type.Value > 0 && bodyParam.extra_type.Value == 1 && bodyParam.extra_id.HasValue && bodyParam.extra_id.Value > 0)
      {
        e = this.addParameter(bodyParam.extra_id.Value, bodyParam.extra_position.HasValue ? bodyParam.extra_position.Value : 3, parameterPrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    this.textures_.Clear();
    this.textures_ = (List<Texture2D>) null;
  }

  private IEnumerator loadDetailResource(ManualMenuBase.BodyParam[] data)
  {
    this.textures_ = new List<Texture2D>(data.Length);
    List<int> unitIDs = new List<int>();
    for (int i = 0; i < data.Length; ++i)
    {
      int errorCount = 0;
      ManualMenuBase.BodyParam body = data[i];
      Texture2D texture = (Texture2D) null;
      if (!string.IsNullOrEmpty(body.image_url))
      {
        if (Object.op_Equality((Object) texture, (Object) null))
        {
          texture = (Texture2D) null;
          while (Object.op_Equality((Object) texture, (Object) null) && errorCount < 3)
          {
            Dictionary<string, object> requestResult = new Dictionary<string, object>();
            yield return (object) WWWUtil.RequestAndCache(body.image_url, (Action<Dictionary<string, object>>) (result => requestResult = result));
            if (string.IsNullOrEmpty(((WWW) requestResult["www"]).error))
            {
              texture = (Texture2D) requestResult["texture"];
              break;
            }
            ++errorCount;
          }
        }
        if (Object.op_Equality((Object) texture, (Object) null))
        {
          this.textures_.Add((Texture2D) null);
          continue;
        }
      }
      this.textures_.Add(texture);
      if (body.extra_id.HasValue && body.extra_type.HasValue && body.extra_type.Value == 1 && MasterData.UnitUnit.ContainsKey(body.extra_id.Value))
        unitIDs.Add(body.extra_id.Value);
      body = (ManualMenuBase.BodyParam) null;
      texture = (Texture2D) null;
    }
    IEnumerator e = OnDemandDownload.WaitLoadUnitResource(unitIDs.Select<int, UnitUnit>((Func<int, UnitUnit>) (x => MasterData.UnitUnit[x])), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void setTitleText(string titleText, Transform parent, GameObject textPrefab)
  {
    if (titleText == null)
      return;
    GameObject gameObject = Object.Instantiate<GameObject>(textPrefab);
    gameObject.SetActive(true);
    UILabel componentInChildren1 = gameObject.GetComponentInChildren<UILabel>();
    UIWidget componentInChildren2 = gameObject.GetComponentInChildren<UIWidget>();
    componentInChildren1.text = titleText;
    componentInChildren1.fontSize = 30;
    int num1 = titleText.Length * componentInChildren1.fontSize + titleText.Length;
    int num2 = componentInChildren1.fontSize + componentInChildren1.spacingY;
    int num3 = num1;
    int num4 = num2;
    componentInChildren2.SetDimensions(num3, num4);
    ((UIWidget) componentInChildren1).SetDimensions(num1, num2);
    ((UIWidget) componentInChildren1).pivot = (UIWidget.Pivot) 4;
    this.scroll_.Add(gameObject);
  }

  private void addSprite(
    Sprite image,
    GameObject spritePrefab,
    string sceneName,
    string paramName,
    int? width,
    int? height)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(spritePrefab);
    gameObject.SetActive(true);
    UI2DSprite componentInChildren1 = gameObject.GetComponentInChildren<UI2DSprite>();
    UIWidget componentInChildren2 = gameObject.GetComponentInChildren<UIWidget>();
    int width1;
    if (!width.HasValue)
    {
      Rect textureRect = image.textureRect;
      width1 = (int) ((Rect) ref textureRect).width;
    }
    else
      width1 = width.Value;
    int num1 = width1;
    int height1;
    if (!height.HasValue)
    {
      Rect textureRect = image.textureRect;
      height1 = (int) ((Rect) ref textureRect).height;
    }
    else
      height1 = height.Value;
    int num2 = height1;
    ((UIWidget) componentInChildren1).SetDimensions(num1, num2);
    componentInChildren2.SetDimensions(num1, num2);
    componentInChildren1.sprite2D = image;
    this.scroll_.Add(gameObject);
    Startup00012ButtonManager component1 = gameObject.GetComponent<Startup00012ButtonManager>();
    UIButton component2 = gameObject.GetComponent<UIButton>();
    if (!Object.op_Inequality((Object) component2, (Object) null))
      return;
    if (Object.op_Inequality((Object) component1, (Object) null))
    {
      if (string.IsNullOrEmpty(sceneName))
      {
        ((Behaviour) component2).enabled = false;
      }
      else
      {
        ((Behaviour) component2).enabled = true;
        component1.scene = sceneName;
        component1.param = paramName;
      }
    }
    else
      ((Behaviour) component2).enabled = false;
  }

  private void addText(string bodytext, int width, GameObject textPrefab)
  {
    this.setText(bodytext, width, textPrefab);
    this.scroll_.ResolvePosition();
  }

  private void setText(string text, int width, GameObject textPrefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(textPrefab);
    UILabel componentInChildren1 = gameObject.GetComponentInChildren<UILabel>();
    UIWidget componentInChildren2 = gameObject.GetComponentInChildren<UIWidget>();
    ((UIWidget) componentInChildren1).SetDimensions(width, 2);
    componentInChildren1.SetTextLocalize(text);
    this.height_ += (float) ((UIWidget) componentInChildren1).height;
    int num = width;
    int height = ((UIWidget) componentInChildren1).height;
    componentInChildren2.SetDimensions(num, height);
    this.scroll_.Add(gameObject);
  }

  private IEnumerator addParameter(int id, int position, GameObject parameterPrefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(parameterPrefab);
    this.scroll_.Add(gameObject);
    IEnumerator e = gameObject.GetComponent<DetailUnitParameter>().Init(id, position);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected class BodyParam
  {
    public string body;
    public int? image_height;
    public string image_url;
    public int? image_width;
    public int? extra_type;
    public int? extra_id;
    public int? extra_position;
    public string scene_name;
    public string param_name;
  }
}
