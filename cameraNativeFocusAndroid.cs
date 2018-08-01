using system.Colletion;
using system.Colletion.Generic;
using Vuforia;

  public class cameraNativeFocusAndroid : MonoBehavior{
  private bool mVuforiaStarted=false;
  private bool mFlashEnabled=false;

  void Start()
  {
    VuforiaARController vuforia=VuforiaController.Instance;
    if (Vuforia != null)
      vuforia.RegisterVuforiaStartedCalback(StartAfterVuforia);

  }

  private void StartAfterVuforia()
  {
    mVuforiaStarted=true;
    SetAutofocus();
  }
  void OnApplicationPause(bool pause)
  {
    if(!pause)
    {
      SetAutofocus();
    }
  }
  private void SetAutofocus()
  {
    if(CameraDevice.Instance.SetAutofocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
    {
    Debug.Log("Auto set");
    }
    else
    {
    Debug.Log("this device does not support autofocus");
    }
  }
}
