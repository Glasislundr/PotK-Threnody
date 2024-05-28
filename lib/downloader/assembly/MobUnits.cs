// Decompiled with JetBrains decompiler
// Type: MobUnits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class MobUnits : MonoBehaviour
{
  public const int MOBCHARA_MAX_ID = 999;

  public static Future<Sprite> LoadSpriteLarge(int id)
  {
    return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/MobUnits/{0}/unit_large", (object) id));
  }

  private static string GetSpriteFacePath(int id, string name)
  {
    return string.Format("AssetBundle/Resources/MobUnits/{0}/Face/{1}", (object) id, (object) name);
  }

  public static bool HasSpriteFace(int id, string name)
  {
    return Singleton<ResourceManager>.GetInstance().Contains(MobUnits.GetSpriteFacePath(id, name));
  }

  public static Future<Sprite> LoadSpriteFace(int id, string name)
  {
    return Singleton<ResourceManager>.GetInstance().Load<Sprite>(MobUnits.GetSpriteFacePath(id, name));
  }

  private static string GetSpriteEyePath(int id, string name)
  {
    return string.Format("AssetBundle/Resources/MobUnits/{0}/Eye/{1}", (object) id, (object) name);
  }

  public static bool HasSpriteEye(int id, string name)
  {
    return Singleton<ResourceManager>.GetInstance().Contains(MobUnits.GetSpriteEyePath(id, name));
  }

  public static Future<Sprite> LoadSpriteEye(int id, string name)
  {
    return Singleton<ResourceManager>.GetInstance().Load<Sprite>(MobUnits.GetSpriteEyePath(id, name));
  }

  public static Future<Sprite> LoadSpriteBasic(int id)
  {
    return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/MobUnits/{0}/unit_large", (object) id));
  }

  public static Future<GameObject> LoadStory(int id)
  {
    return Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("MobUnits/{0}/StoryUnitPrefab", (object) id));
  }
}
