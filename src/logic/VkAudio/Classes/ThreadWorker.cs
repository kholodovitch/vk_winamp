// Type: VkAudio.Classes.ThreadWorker
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Threading;
using System.Windows.Forms;
using VkAudio.Forms;

namespace VkAudio.Classes
{
  public static class ThreadWorker
  {
    private static string lastText;
    private static bool lastWait;

    static ThreadWorker()
    {
    }

    public static Action PerformSimpleOperation(Form Owner, ThreadOperation o, Action onEnd)
    {
      bool result = true;
      Exception exception = (Exception) null;
      Thread th = new Thread((ThreadStart) (() =>
      {
	      try
	      {
		      o();
	      }
	      catch (ThreadAbortException exception_0)
	      {
	      }
	      catch (Exception exception_1)
	      {
		      result = false;
		      ControlExtension.InvokeOperation((Control) Owner, (ThreadOperation) (() =>
			      {
				      if (exception_1 != null && !exception_1.Message.Contains("request was aborted") && !(exception_1 is UserException))
					      throw exception_1;
				      if (exception_1 != null && exception_1 is UserException)
				      {
					      int temp_58 = (int) MessageBox.Show((IWin32Window) Owner, exception_1.Message, "Ошибка");
					      result = false;
				      }
				      if (!result)
					      return;
				      if (onEnd == null)
					      return;
				      try
				      {
					      onEnd();
				      }
				      catch (Exception exception_2)
				      {
					      int temp_48 = (int) MessageBox.Show((IWin32Window) Owner, exception_2.Message, "Ошибка");
				      }
			      }));
	      }
      }));
      th.Start();
      return (Action) (() =>
      {
        if (th == null)
          return;
        th.Abort();
      });
    }

    public static void PerformOperation(IWin32Window Owner, ThreadOperation o, bool WaitForNext, string LoadingText, int? ProgressMaxValue, EventHandler CancelHandler, Action SuccessHandler)
    {
      bool result = false;
      ThreadWorker.lastText = LoadingText;
      ThreadWorker.lastWait = WaitForNext;
      LoadingForm.ShowBlockingLoading(Owner, WaitForNext, LoadingText, ProgressMaxValue, CancelHandler);
      Exception exception = (Exception) null;
      Thread th = (Thread) null;
      EventHandler canceller = (EventHandler) ((ls, le) =>
      {
        LoadingForm.HideBlockingLoading();
        th.Abort();
        if (CancelHandler == null)
          return;
        CancelHandler((object) null, new EventArgs());
      });
      ThreadOperation finalizer = (ThreadOperation) (() =>
      {
        LoadingForm.HideBlockingLoading();
        LoadingForm.Cancel -= canceller;
        if (exception != null && !exception.Message.Contains("request was aborted") && !(exception is UserException))
        {
          int temp_93 = (int) new BigMessageBoxForm("Ошибка", string.Format("Type:\r\n{0}\r\n\r\nMessage:\r\n{1}\r\n\r\nStackTrace:\r\n{2}", (object) exception.GetType(), (object) exception.Message, (object) exception.StackTrace)).ShowDialog(Owner);
        }
        if (exception != null && exception is UserException)
        {
          int temp_68 = (int) MessageBox.Show(Owner, exception.Message, "Ошибка");
        }
        if (!result)
          return;
        SuccessHandler();
      });
      th = new Thread((ThreadStart) (() =>
      {
        try
        {
          o();
          result = true;
        }
        catch (ThreadAbortException exception_0)
        {
        }
        catch (Exception exception_1)
        {
        }
        if (Owner != null && Owner is Form)
          ControlExtension.InvokeOperation((Control) (Owner as Form), finalizer);
        else
          finalizer();
      }));
      LoadingForm.Cancel += canceller;
      th.Start();
    }

    public static void IncrementProgress(int value)
    {
      LoadingForm.IncrementProgress(value);
    }

    public static void UpdateStatus(string LoadingText)
    {
      LoadingForm.UpdateStatus(LoadingText);
    }
  }
}
