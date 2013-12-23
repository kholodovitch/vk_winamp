namespace VkAudio.Classes
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using VkAudio.Forms;

    public static class ThreadWorker
    {
        private static string lastText;
        private static bool lastWait;

        public static void IncrementProgress(int value)
        {
            LoadingForm.IncrementProgress(value);
        }

        public static void PerformOperation(IWin32Window Owner, ThreadOperation o, bool WaitForNext, string LoadingText, int? ProgressMaxValue, EventHandler CancelHandler, Action SuccessHandler)
        {
            bool result = false;
            lastText = LoadingText;
            lastWait = WaitForNext;
            LoadingForm.ShowBlockingLoading(Owner, WaitForNext, LoadingText, ProgressMaxValue, CancelHandler);
            Exception exception = null;
            Thread th = null;
            EventHandler canceller = delegate (object ls, EventArgs le) {
                LoadingForm.HideBlockingLoading();
                th.Abort();
                if (CancelHandler != null)
                {
                    CancelHandler(null, new EventArgs());
                }
            };
            ThreadOperation finalizer = delegate {
                LoadingForm.HideBlockingLoading();
                LoadingForm.Cancel -= canceller;
                if (((exception != null) && !exception.Message.Contains("request was aborted")) && !(exception is UserException))
                {
                    string msg = string.Format("Type:\r\n{0}\r\n\r\nMessage:\r\n{1}\r\n\r\nStackTrace:\r\n{2}", exception.GetType(), exception.Message, exception.StackTrace);
                    new BigMessageBoxForm("Ошибка", msg).ShowDialog(Owner);
                }
                if ((exception != null) && (exception is UserException))
                {
                    MessageBox.Show(Owner, exception.Message, "Ошибка");
                }
                if (result)
                {
                    SuccessHandler.Invoke();
                }
            };
            th = new Thread(delegate {
                try
                {
                    o();
                    result = true;
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                }
                if ((Owner != null) && (Owner is Form))
                {
                    (Owner as Form).InvokeOperation(finalizer);
                }
                else
                {
                    finalizer();
                }
            });
            LoadingForm.Cancel += canceller;
            th.Start();
        }

        public static Action PerformSimpleOperation(Form Owner, ThreadOperation o, Action onEnd)
        {
            <>c__DisplayClass3 class2;
            bool result = true;
            Exception exception = null;
            new Thread(delegate {
                try
                {
                    o();
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    result = false;
                }
                Owner.InvokeOperation(delegate {
                    if (((exception != null) && !exception.Message.Contains("request was aborted")) && !(exception is UserException))
                    {
                        throw exception;
                    }
                    if ((exception != null) && (exception is UserException))
                    {
                        MessageBox.Show(Owner, exception.Message, "Ошибка");
                        result = false;
                    }
                    if (result && (onEnd != null))
                    {
                        try
                        {
                            onEnd.Invoke();
                        }
                        catch (Exception exception3)
                        {
                            MessageBox.Show(Owner, exception3.Message, "Ошибка");
                        }
                    }
                });
            }).Start();
            return new Action(class2, (IntPtr) this.<PerformSimpleOperation>b__2);
        }

        public static void UpdateStatus(string LoadingText)
        {
            LoadingForm.UpdateStatus(LoadingText);
        }
    }
}

