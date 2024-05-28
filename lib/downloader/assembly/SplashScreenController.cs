// Decompiled with JetBrains decompiler
// Type: SplashScreenController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (StartScript))]
public class SplashScreenController : MonoBehaviour
{
  [SerializeField]
  private float m_waitSeconds = 3f;
  [SerializeField]
  private float m_fadeOutSeconds = 0.25f;
  [SerializeField]
  private Sprite m_splashImageSprite;
  [SerializeField]
  private bool m_showOnStart;
  [SerializeField]
  private bool m_allowSkip = true;
  private Image m_splashImage;
  private SplashScreenController.Status m_status;

  public bool isFinished => this.m_status == SplashScreenController.Status.Finished;

  private void Start()
  {
    if (!this.m_showOnStart)
      return;
    this.ShowSplashScreen();
  }

  private void Update()
  {
    if (!this.m_allowSkip || !Input.GetButtonDown("Fire1"))
      return;
    Object.Destroy((Object) this.m_splashImage);
    this.m_status = SplashScreenController.Status.Finished;
  }

  public void ShowSplashScreen()
  {
    this.Initialize();
    this.StartCoroutine(this.Wait(this.m_waitSeconds));
    this.StartCoroutine(this.FadeOutSplashScreen(this.m_fadeOutSeconds));
  }

  private void Initialize()
  {
    if (Object.op_Equality((Object) this.m_splashImage, (Object) null))
    {
      GameObject gameObject = new GameObject();
      RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
      ((Transform) rectTransform).position = new Vector3((float) Screen.width / 2f, (float) Screen.height / 2f, 0.0f);
      rectTransform.sizeDelta = new Vector2((float) Screen.width, (float) Screen.height);
      rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
      rectTransform.anchorMax = new Vector2(1f, 1f);
      rectTransform.pivot = new Vector2(0.5f, 0.5f);
      this.m_splashImage = gameObject.AddComponent<Image>();
      Canvas canvas = Object.FindObjectOfType<Canvas>();
      if (Object.op_Equality((Object) canvas, (Object) null))
      {
        canvas = ((Component) new GameObject().AddComponent<RectTransform>()).gameObject.AddComponent<Canvas>();
        canvas.renderMode = (RenderMode) 0;
      }
      ((Component) this.m_splashImage).transform.SetParent((Transform) ((Component) canvas).GetComponent<RectTransform>());
      rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
      ((Transform) rectTransform).localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }
    if (!Object.op_Inequality((Object) this.m_splashImage, (Object) null) || !Object.op_Inequality((Object) this.m_splashImageSprite, (Object) null))
      return;
    ((Graphic) this.m_splashImage).color = Color.white;
    this.m_splashImage.preserveAspect = Application.isEditor;
    this.m_splashImage.sprite = this.m_splashImageSprite;
  }

  private IEnumerator Wait(float seconds)
  {
    this.m_status = SplashScreenController.Status.Waiting;
    yield return (object) new WaitForSeconds(seconds);
    this.m_status = SplashScreenController.Status.None;
  }

  private IEnumerator FadeOutSplashScreen(float fadeSeconds)
  {
    while (this.m_status == SplashScreenController.Status.Waiting)
      yield return (object) new WaitForEndOfFrame();
    float elapsedSeconds = 0.0f;
    Color color = ((Graphic) this.m_splashImage).color;
    while ((double) elapsedSeconds < (double) fadeSeconds)
    {
      color.a = Mathf.Max(Mathf.SmoothStep(1f, 0.0f, elapsedSeconds / fadeSeconds), 0.0f);
      ((Graphic) this.m_splashImage).color = color;
      elapsedSeconds += Time.deltaTime;
      yield return (object) new WaitForEndOfFrame();
    }
    Object.Destroy((Object) this.m_splashImage);
    this.m_status = SplashScreenController.Status.Finished;
  }

  private enum Status
  {
    None,
    Waiting,
    Finished,
  }
}
