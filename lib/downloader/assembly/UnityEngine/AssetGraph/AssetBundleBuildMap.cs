// Decompiled with JetBrains decompiler
// Type: UnityEngine.AssetGraph.AssetBundleBuildMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace UnityEngine.AssetGraph
{
  public class AssetBundleBuildMap : ScriptableObject
  {
    [SerializeField]
    private List<AssetBundleBuildMap.AssetBundleEntry> m_assetBundles;
    private static AssetBundleBuildMap s_map;

    public static AssetBundleBuildMap GetBuildMap()
    {
      if (Object.op_Equality((Object) AssetBundleBuildMap.s_map, (Object) null) && !AssetBundleBuildMap.Load())
      {
        AssetBundleBuildMap.s_map = ScriptableObject.CreateInstance<AssetBundleBuildMap>();
        AssetBundleBuildMap.s_map.m_assetBundles = new List<AssetBundleBuildMap.AssetBundleEntry>();
      }
      return AssetBundleBuildMap.s_map;
    }

    private static bool Load() => false;

    public static void SetMapDirty()
    {
    }

    internal static string MakeFullName(string assetBundleName, string variantName)
    {
      if (string.IsNullOrEmpty(assetBundleName))
        return "";
      return string.IsNullOrEmpty(variantName) ? assetBundleName.ToLower() : string.Format("{0}.{1}", (object) assetBundleName.ToLower(), (object) variantName.ToLower());
    }

    internal static string[] FullNameToNameAndVariant(string assetBundleFullName)
    {
      assetBundleFullName = assetBundleFullName.ToLower();
      int length = assetBundleFullName.LastIndexOf('.');
      return length > 0 && length + 1 < assetBundleFullName.Length ? new string[2]
      {
        assetBundleFullName.Substring(0, length),
        assetBundleFullName.Substring(length + 1)
      } : new string[2]{ assetBundleFullName, "" };
    }

    public AssetBundleBuildMap.AssetBundleEntry GetAssetBundle(
      string registererId,
      string assetBundleFullName)
    {
      AssetBundleBuildMap.AssetBundleEntry assetBundle = this.m_assetBundles.Find((Predicate<AssetBundleBuildMap.AssetBundleEntry>) (v => v.m_fullName == assetBundleFullName));
      if (assetBundle == null)
      {
        string[] nameAndVariant = AssetBundleBuildMap.FullNameToNameAndVariant(assetBundleFullName);
        assetBundle = new AssetBundleBuildMap.AssetBundleEntry(registererId, nameAndVariant[0], nameAndVariant[1]);
        this.m_assetBundles.Add(assetBundle);
        AssetBundleBuildMap.SetMapDirty();
      }
      return assetBundle;
    }

    public void Clear()
    {
      this.m_assetBundles.Clear();
      AssetBundleBuildMap.SetMapDirty();
    }

    public void ClearFromId(string id)
    {
      this.m_assetBundles.RemoveAll((Predicate<AssetBundleBuildMap.AssetBundleEntry>) (e => e.m_registererId == id));
    }

    public AssetBundleBuildMap.AssetBundleEntry GetAssetBundleWithNameAndVariant(
      string registererId,
      string assetBundleName,
      string variantName)
    {
      return this.GetAssetBundle(registererId, AssetBundleBuildMap.MakeFullName(assetBundleName, variantName));
    }

    public string[] GetAssetPathsFromAssetBundleAndAssetName(
      string assetbundleName,
      string assetName)
    {
      assetName = assetName.ToLower();
      return this.m_assetBundles.Where<AssetBundleBuildMap.AssetBundleEntry>((Func<AssetBundleBuildMap.AssetBundleEntry, bool>) (ab => ab.m_fullName == assetbundleName)).SelectMany<AssetBundleBuildMap.AssetBundleEntry, string>((Func<AssetBundleBuildMap.AssetBundleEntry, IEnumerable<string>>) (ab => ab.GetAssetFromAssetName(assetName))).ToArray<string>();
    }

    public string[] GetAssetPathsFromAssetBundle(string assetBundleName)
    {
      assetBundleName = assetBundleName.ToLower();
      return this.m_assetBundles.Where<AssetBundleBuildMap.AssetBundleEntry>((Func<AssetBundleBuildMap.AssetBundleEntry, bool>) (e => e.m_fullName == assetBundleName)).SelectMany<AssetBundleBuildMap.AssetBundleEntry, AssetBundleBuildMap.AssetBundleEntry.AssetPathString>((Func<AssetBundleBuildMap.AssetBundleEntry, IEnumerable<AssetBundleBuildMap.AssetBundleEntry.AssetPathString>>) (e => (IEnumerable<AssetBundleBuildMap.AssetBundleEntry.AssetPathString>) e.m_assets)).Select<AssetBundleBuildMap.AssetBundleEntry.AssetPathString, string>((Func<AssetBundleBuildMap.AssetBundleEntry.AssetPathString, string>) (s => s.original)).ToArray<string>();
    }

    public string GetAssetBundleName(string assetPath)
    {
      AssetBundleBuildMap.AssetBundleEntry assetBundleEntry = this.m_assetBundles.Find((Predicate<AssetBundleBuildMap.AssetBundleEntry>) (e => e.m_assets.Contains(new AssetBundleBuildMap.AssetBundleEntry.AssetPathString(assetPath))));
      return assetBundleEntry != null ? assetBundleEntry.m_fullName : string.Empty;
    }

    public string GetImplicitAssetBundleName(string assetPath)
    {
      return this.GetAssetBundleName(assetPath);
    }

    public string[] GetAllAssetBundleNames()
    {
      return this.m_assetBundles.Select<AssetBundleBuildMap.AssetBundleEntry, string>((Func<AssetBundleBuildMap.AssetBundleEntry, string>) (e => e.m_fullName)).ToArray<string>();
    }

    [Serializable]
    public class AssetBundleEntry
    {
      [SerializeField]
      internal string m_assetBundleName;
      [SerializeField]
      internal string m_assetBundleVariantName;
      [SerializeField]
      internal string m_fullName;
      [SerializeField]
      internal List<AssetBundleBuildMap.AssetBundleEntry.AssetPathString> m_assets;
      [SerializeField]
      public string m_registererId;

      public string Name => this.m_assetBundleName;

      public string Variant => this.m_assetBundleVariantName;

      public string FullName => this.m_fullName;

      public AssetBundleEntry(string registererId, string assetBundleName, string variantName)
      {
        this.m_registererId = registererId;
        this.m_assetBundleName = assetBundleName.ToLower();
        this.m_assetBundleVariantName = variantName.ToLower();
        this.m_fullName = AssetBundleBuildMap.MakeFullName(assetBundleName, variantName);
        this.m_assets = new List<AssetBundleBuildMap.AssetBundleEntry.AssetPathString>();
      }

      public void Clear()
      {
        this.m_assets.Clear();
        AssetBundleBuildMap.SetMapDirty();
      }

      public void AddAssets(string id, IEnumerable<string> assets)
      {
        foreach (string asset in assets)
          this.m_assets.Add(new AssetBundleBuildMap.AssetBundleEntry.AssetPathString(asset));
        AssetBundleBuildMap.SetMapDirty();
      }

      public IEnumerable<string> GetAssetFromAssetName(string assetName)
      {
        assetName = assetName.ToLower();
        return this.m_assets.Where<AssetBundleBuildMap.AssetBundleEntry.AssetPathString>((Func<AssetBundleBuildMap.AssetBundleEntry.AssetPathString, bool>) (a => Path.GetFileNameWithoutExtension(a.lower) == assetName)).Select<AssetBundleBuildMap.AssetBundleEntry.AssetPathString, string>((Func<AssetBundleBuildMap.AssetBundleEntry.AssetPathString, string>) (s => s.original));
      }

      [Serializable]
      internal struct AssetPathString
      {
        [SerializeField]
        public string original;
        [SerializeField]
        public string lower;

        internal AssetPathString(string s)
        {
          this.original = s;
          if (!string.IsNullOrEmpty(s))
            this.lower = s.ToLower();
          else
            this.lower = s;
        }
      }
    }
  }
}
