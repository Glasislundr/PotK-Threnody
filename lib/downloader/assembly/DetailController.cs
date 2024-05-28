// Decompiled with JetBrains decompiler
// Type: DetailController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public static class DetailController
{
  private static float height;
  private static List<Texture2D> textures;

  public static void Release()
  {
    if (DetailController.textures == null)
      return;
    DetailController.textures.Clear();
    DetailController.textures = (List<Texture2D>) null;
  }

  private static IEnumerator LoadDetailResource(List<DetailControllerData> data)
  {
    int dataSize = data.Count<DetailControllerData>();
    DetailController.textures = new List<Texture2D>(dataSize);
    List<int> unitIDs = new List<int>();
    for (int i = 0; i < dataSize; ++i)
    {
      int errorCount = 0;
      DetailControllerData body = data[i];
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
          DetailController.textures.Add((Texture2D) null);
          continue;
        }
      }
      DetailController.textures.Add(texture);
      if (body.extra_id.HasValue && body.extra_type.HasValue && body.extra_type.Value == 1 && MasterData.UnitUnit.ContainsKey(body.extra_id.Value))
        unitIDs.Add(body.extra_id.Value);
      body = (DetailControllerData) null;
      texture = (Texture2D) null;
    }
    IEnumerator e = OnDemandDownload.WaitLoadUnitResource(unitIDs.Select<int, UnitUnit>((Func<int, UnitUnit>) (x => MasterData.UnitUnit[x])), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, string title, GuildBankHowto[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = DetailController.Init(Scroll, title, data, DetailController.textures);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, string title, GuildRaidHowto[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = DetailController.Init(Scroll, title, data, DetailController.textures);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, string title, TowerHowto[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = DetailController.Init(Scroll, title, data, DetailController.textures);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, string title, ClassRankingHowto[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = DetailController.Init(Scroll, title, data, DetailController.textures);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, string title, DescriptionBodies[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = DetailController.Init(Scroll, title, data, DetailController.textures);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    QuestScoreCampaignDescriptionBlockBodies[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = DetailController.Init(Scroll, title, data, DetailController.textures);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, QuestExtraDescription[] bodys)
  {
    List<DetailControllerData> data = ((IEnumerable<QuestExtraDescription>) bodys).Select<QuestExtraDescription, DetailControllerData>((Func<QuestExtraDescription, DetailControllerData>) (b => new DetailControllerData(b))).ToList<DetailControllerData>();
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = DetailController.Init(Scroll, (string) null, data, DetailController.textures);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    GachaDescriptionBodies[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, string title, CoinProductDetail[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, string title, CoinBonusDetail[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    SimplePackDescription[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    BeginnerPackDescription[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    StepupPackDescription[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    WeeklyPackDescription[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    MonthlyPackDescription[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(NGxScrollMasonry Scroll, string title, PackDescription[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) Scroll, (Object) null))
    {
      e = DetailController.Init(Scroll, title, data, DetailController.textures);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    OfficialInformationArticleBodies[] bodys)
  {
    List<DetailControllerData> data = new List<DetailControllerData>();
    int length = bodys.Length;
    for (int index = 0; index < length; ++index)
      data.Add(new DetailControllerData(bodys[index]));
    IEnumerator e = DetailController.LoadDetailResource(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = DetailController.Init(Scroll, title, data, DetailController.textures);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private static IEnumerator Init(
    NGxScrollMasonry Scroll,
    string title,
    List<DetailControllerData> bodys,
    List<Texture2D> textures)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    DetailController.height = 0.0f;
    int panel_width = (int) ((Component) Scroll.Scroll).GetComponent<UIPanel>().width;
    Future<GameObject> textPrefabF = Res.Prefabs.dynamic_display.masonryTextBox.Load<GameObject>();
    IEnumerator e = textPrefabF.Wait();
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
    DetailController.SetTitleText(Scroll, title, ((Component) Scroll.Scroll).transform, textPrefab);
    for (int i = 0; i < bodys.Count; ++i)
    {
      DetailControllerData body = bodys[i];
      if (!string.IsNullOrEmpty(body.image_url))
      {
        if (Object.op_Equality((Object) textures[i], (Object) null))
          Debug.LogWarning((object) ("画像の取得失敗 path:" + body.image_url));
        else
          DetailController.addSprite(Scroll, Sprite.Create(textures[i], new Rect(0.0f, 0.0f, (float) ((Texture) textures[i]).width, (float) ((Texture) textures[i]).height), new Vector2(0.0f, 0.0f), 1f, 100U, (SpriteMeshType) 0), spritePrefab, body.scene_name, body.param_name, body.image_width, body.image_height);
      }
      if (!string.IsNullOrEmpty(body.body))
        DetailController.addText(Scroll, body.body, panel_width, textPrefab);
      if (body.extra_type.HasValue && body.extra_type.Value > 0 && body.extra_type.Value == 1 && body.extra_id.HasValue && body.extra_id.Value > 0)
      {
        e = DetailController.addParameter(Scroll, body.extra_id.Value, body.extra_position.HasValue ? body.extra_position.Value : 3, parameterPrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private static void SetTitleText(
    NGxScrollMasonry Scroll,
    string titleText,
    Transform parent,
    GameObject textPrefab)
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
    Scroll.Add(gameObject);
  }

  private static void addSprite(
    NGxScrollMasonry Scroll,
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
    Scroll.Add(gameObject);
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

  private static void addText(
    NGxScrollMasonry Scroll,
    string bodytext,
    int width,
    GameObject textPrefab)
  {
    DetailController.SetText(Scroll, bodytext, width, textPrefab);
    Scroll.ResolvePosition();
  }

  private static void SetText(
    NGxScrollMasonry Scroll,
    string text,
    int width,
    GameObject textPrefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(textPrefab);
    UILabel componentInChildren1 = gameObject.GetComponentInChildren<UILabel>();
    UIWidget componentInChildren2 = gameObject.GetComponentInChildren<UIWidget>();
    ((UIWidget) componentInChildren1).SetDimensions(width, 2);
    componentInChildren1.SetTextLocalize(text);
    DetailController.height += (float) ((UIWidget) componentInChildren1).height;
    int num = width;
    int height = ((UIWidget) componentInChildren1).height;
    componentInChildren2.SetDimensions(num, height);
    Scroll.Add(gameObject);
  }

  private static int GetLineCount(string s, int fontsize, int width)
  {
    int lineCount = Mathf.CeilToInt((float) s.Trim().Length / Mathf.Floor((float) (width / fontsize)));
    if (lineCount == 0)
      lineCount = 1;
    return lineCount;
  }

  private static IEnumerator addParameter(
    NGxScrollMasonry Scroll,
    int id,
    int position,
    GameObject parameterPrefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(parameterPrefab);
    Scroll.Add(gameObject);
    IEnumerator e = gameObject.GetComponent<DetailUnitParameter>().Init(id, position);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
