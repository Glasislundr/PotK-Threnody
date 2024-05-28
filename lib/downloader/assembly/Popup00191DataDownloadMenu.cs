// Decompiled with JetBrains decompiler
// Type: Popup00191DataDownloadMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using gu3.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Popup00191DataDownloadMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtVoiceDownloadAlert;
  [SerializeField]
  private UILabel txtVoiceDownloadSize;
  [SerializeField]
  private SpreadColorButton voiceDownloadBtn;
  [SerializeField]
  private UILabel txtMovieDownloadAlert;
  [SerializeField]
  private UILabel txtMovieDownloadSize;
  [SerializeField]
  private SpreadColorButton movieDownloadBtn;
  [SerializeField]
  private UILabel txtEtcDownloadAlert;
  [SerializeField]
  private UILabel txtEtcDownloadSize;
  [SerializeField]
  private SpreadColorButton etcDownloadBtn;
  [SerializeField]
  private UILabel txtVoiceDeleteSize;
  [SerializeField]
  private SpreadColorButton voiceDeleteBtn;
  [SerializeField]
  private UILabel txtMovieDeleteSize;
  [SerializeField]
  private SpreadColorButton movieDeleteBtn;
  private List<string> voiceDownloadPaths;
  private List<string> movieDownloadPaths;
  private List<string> etcDownloadPaths;
  private List<string> voiceDeleteFileNames;
  private List<string> movieDeleteFileNames;
  private long voiceDownloadSize;
  private long movieDownloadSize;
  private long etcDownloadSize;
  private long voiceDeleteSize;
  private long movieDeleteSize;
  private Story00191Menu menu;

  public void Init(Story00191Menu menu)
  {
    this.menu = menu;
    HashSet<string> stringSet1 = new HashSet<string>();
    HashSet<string> stringSet2 = !Persist.fileMoved.Data.isAllMoved ? new HashSet<string>((IEnumerable<string>) FileManager.GetEntries(DLC.ResourceDirectory)) : DLC.GetEntries();
    ResourceInfo resourceInfo = Singleton<ResourceManager>.GetInstance().ResourceInfo;
    this.voiceDownloadPaths = new List<string>();
    this.movieDownloadPaths = new List<string>();
    this.etcDownloadPaths = new List<string>();
    this.voiceDeleteFileNames = new List<string>();
    this.movieDeleteFileNames = new List<string>();
    this.voiceDownloadSize = 0L;
    this.movieDownloadSize = 0L;
    this.etcDownloadSize = 0L;
    this.voiceDeleteSize = 0L;
    this.movieDeleteSize = 0L;
    foreach (ResourceInfo.Resource resource in resourceInfo)
    {
      ResourceInfo.Value obj = resource._value;
      switch (obj._path_type)
      {
        case ResourceInfo.PathType.AssetBundle:
          if (!stringSet2.Contains(obj._file_name))
          {
            this.etcDownloadPaths.Add(resource._key);
            this.etcDownloadSize += (long) obj._file_size;
            continue;
          }
          continue;
        case ResourceInfo.PathType.StreamingAssets:
          bool flag = stringSet2.Contains(obj._file_name);
          if (ResourceManager.IsMobVoice(resource._key))
          {
            if (flag)
            {
              this.voiceDeleteFileNames.Add(obj._file_name);
              this.voiceDeleteSize += (long) obj._file_size;
              continue;
            }
            this.voiceDownloadPaths.Add(resource._key);
            this.voiceDownloadSize += (long) obj._file_size;
            continue;
          }
          if (resource.IsMovie())
          {
            if (flag)
            {
              this.movieDeleteFileNames.Add(obj._file_name);
              this.movieDeleteSize += (long) obj._file_size;
              continue;
            }
            this.movieDownloadPaths.Add(resource._key);
            this.movieDownloadSize += (long) obj._file_size;
            continue;
          }
          if (!flag)
          {
            this.etcDownloadPaths.Add(resource._key);
            this.etcDownloadSize += (long) obj._file_size;
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    ulong freeSize = Sys.GetAvailableStorageBytes();
    Action<long, UILabel, UILabel, SpreadColorButton> action1 = (Action<long, UILabel, UILabel, SpreadColorButton>) ((size, txtSize, txtAlert, btn) =>
    {
      ((Component) txtAlert).gameObject.SetActive(false);
      if ((ulong) size > freeSize || size <= 0L)
      {
        ((Component) txtAlert).gameObject.SetActive(size > 0L);
        ((UIButtonColor) btn).isEnabled = false;
      }
      txtSize.SetTextLocalize(this.GetHummableDownLoadSize(size));
    });
    action1(this.voiceDownloadSize, this.txtVoiceDownloadSize, this.txtVoiceDownloadAlert, this.voiceDownloadBtn);
    action1(this.movieDownloadSize, this.txtMovieDownloadSize, this.txtMovieDownloadAlert, this.movieDownloadBtn);
    action1(this.etcDownloadSize, this.txtEtcDownloadSize, this.txtEtcDownloadAlert, this.etcDownloadBtn);
    Action<long, UILabel, SpreadColorButton> action2 = (Action<long, UILabel, SpreadColorButton>) ((size, txtSize, btn) =>
    {
      ((UIButtonColor) btn).isEnabled = size > 0L;
      txtSize.SetTextLocalize(this.GetHummableDeleteSize(size));
    });
    action2(this.voiceDeleteSize, this.txtVoiceDeleteSize, this.voiceDeleteBtn);
    action2(this.movieDeleteSize, this.txtMovieDeleteSize, this.movieDeleteBtn);
  }

  public void IbtnVoiceDownload()
  {
    this.StartCoroutine(this.Download(this.voiceDownloadPaths, this.GetPopupDownLoadMessage(Consts.GetInstance().POPUP_DOWNLOAD_VOICE_MESSAGE, this.voiceDownloadSize)));
  }

  public void IbtnMovieDownload()
  {
    this.StartCoroutine(this.Download(this.movieDownloadPaths, this.GetPopupDownLoadMessage(Consts.GetInstance().POPUP_DOWNLOAD_MOVIE_MESSAGE, this.movieDownloadSize)));
  }

  public void IbtnEtcDownload()
  {
    this.StartCoroutine(this.Download(this.etcDownloadPaths, this.GetPopupDownLoadMessage(Consts.GetInstance().POPUP_DOWNLOAD_ETC_MESSAGE, this.etcDownloadSize)));
  }

  public void IbtnVoiceDelete()
  {
    this.StartCoroutine(this.Delete(this.voiceDeleteFileNames, this.GetPopupDeleteMessage(Consts.GetInstance().POPUP_DELETE_VOICE_MESSAGE, this.voiceDeleteSize)));
  }

  public void IbtnMovieDelete()
  {
    this.StartCoroutine(this.Delete(this.movieDeleteFileNames, this.GetPopupDeleteMessage(Consts.GetInstance().POPUP_DELETE_MOVIE_MESSAGE, this.movieDeleteSize)));
  }

  private IEnumerator Download(List<string> paths, string popupMessage)
  {
    bool? popup_yes_select = new bool?();
    PopupCommonYesNo.Show(Consts.GetInstance().POPUP_DOWNLOAD_TITLE, popupMessage, (Action) (() => popup_yes_select = new bool?(true)), (Action) (() => popup_yes_select = new bool?(false)));
    while (!popup_yes_select.HasValue)
      yield return (object) null;
    if (popup_yes_select.Value)
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
      IEnumerator e = this.menu.DownloadContents(paths, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) new WaitForSeconds(1f);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      ModalWindow.Show(Consts.GetInstance().POPUP_DOWNLOAD_TITLE, Consts.GetInstance().POPUP_DOWNLOAD_DONE, (Action) (() =>
      {
        this.menu.IsPush = false;
        this.menu.IbtnDetaDownload();
      }));
    }
  }

  private IEnumerator Delete(List<string> fileNames, string popupMessage)
  {
    bool? popup_yes_select = new bool?();
    PopupCommonYesNo.Show(Consts.GetInstance().POPUP_DELETE_TITLE, popupMessage, (Action) (() => popup_yes_select = new bool?(true)), (Action) (() => popup_yes_select = new bool?(false)));
    while (!popup_yes_select.HasValue)
      yield return (object) null;
    if (popup_yes_select.Value)
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
      IEnumerator e = this.menu.DeleteContents(fileNames);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) new WaitForSeconds(1f);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      ModalWindow.Show(Consts.GetInstance().POPUP_DELETE_TITLE, Consts.GetInstance().POPUP_DELETE_DONE, (Action) (() =>
      {
        this.menu.IsPush = false;
        this.menu.IbtnDetaDownload();
      }));
    }
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();

  private string GetPopupDownLoadMessage(string message, long size)
  {
    return string.Format(message, (object) this.GetHummableDownLoadSize(size));
  }

  private string GetPopupDeleteMessage(string message, long size)
  {
    return string.Format(message, (object) this.GetHummableDeleteSize(size));
  }

  private string GetHummableDownLoadSize(long size)
  {
    return Consts.Format(Consts.GetInstance().POPUP_DOWNLOAD_SIZE, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) Consts.GetInstance().hummableSize(size)
      }
    });
  }

  private string GetHummableDeleteSize(long size)
  {
    return Consts.Format(Consts.GetInstance().POPUP_DOWNLOAD_SIZE, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) Consts.GetInstance().hummableSize(size)
      }
    });
  }
}
