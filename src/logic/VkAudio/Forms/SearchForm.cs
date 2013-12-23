namespace VkAudio.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;
    using VkAudio;
    using VkAudio.Classes;
    using VkAudio.Controls;
    using VkAudio.Properties;

    public class SearchForm : Form
    {
        private List<VkSong> _SelectedSongs;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCopyUrl;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox cbDownloadImmediate;
        private System.Windows.Forms.CheckBox cbPerformer;
        private ColumnHeader chDuration;
        private ColumnHeader chSong;
        private ContextMenuStrip cmsSearchResults;
        private IContainer components;
        private bool doneScrolling;
        private System.Windows.Forms.GroupBox gbPreview;
        private System.Windows.Forms.GroupBox gbResults;
        private System.Windows.Forms.GroupBox gbSearch;
        private int lastSelectedProgress;
        private string lastUrl;
        private Label lNameCaption;
        private Label lPreviewHeader;
        private Label lPreviewTime;
        private Label lResult;
        private Label lSearchMode;
        private ListViewEx lvResults;
        private Panel panBottom;
        private Panel panMain;
        private PictureBox pbBassLoading;
        private PictureBox pbLoading;
        private object posChangeLocker;
        private bool posInternalChanging;
        private PlayerState previewState;
        private Timer previewTimer;
        private string query;
        private System.Windows.Forms.RadioButton rbGroupPerson;
        private System.Windows.Forms.RadioButton rbMyAudio;
        private System.Windows.Forms.RadioButton rbSearch;
        private bool requestingAddSongs;
        private TrackBarEx tbPreviewProgress;
        private System.Windows.Forms.TextBox tbSearch;
        private ToolStripSeparator toolStripSeparator1;
        private int totalCount;
        private ToolStripMenuItem tsmiCheckAll;
        private ToolStripMenuItem tsmiCheckSelected;
        private ToolStripMenuItem tsmiInvertCheck;
        private ToolStripMenuItem tsmiPreview;
        private ToolStripMenuItem tsmiSaveChecked;
        private ToolStripMenuItem tsmiSaveSelected;
        private ToolStripMenuItem tsmiShowLyrics;
        private ToolStripMenuItem tsmiUncheckAll;

        public SearchForm()
        {
            ListViewItemSelectionChangedEventHandler handler = null;
            ItemCheckedEventHandler handler2 = null;
            this.lastUrl = string.Empty;
            Timer timer = new Timer {
                Interval = 100
            };
            this.previewTimer = timer;
            this.query = "";
            this.posChangeLocker = new object();
            this.previewState = PlayerState.NotSet;
            this.lastSelectedProgress = -1;
            this.InitializeComponent();
            this.tbSearch.Focus();
            this.tsmiSaveSelected.Enabled = this.tsmiSaveChecked.Enabled = this.tsmiShowLyrics.Enabled = false;
            if (handler == null)
            {
                handler = delegate (object ls, ListViewItemSelectionChangedEventArgs le) {
                    this.tsmiSaveSelected.Enabled = this.lvResults.SelectedItems.Count > 0;
                    if (CS$<>9__CachedAnonymousMethodDelegate7 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate7 = new Func<ListViewItem, VkSong>(null, (IntPtr) <.ctor>b__2);
                    }
                    if (CS$<>9__CachedAnonymousMethodDelegate8 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate8 = new Func<VkSong, bool>(null, (IntPtr) <.ctor>b__3);
                    }
                    this.tsmiShowLyrics.Enabled = Enumerable.Any<VkSong>(Enumerable.Select<ListViewItem, VkSong>(this.lvResults.SelectedItems.OfType<ListViewItem>(), CS$<>9__CachedAnonymousMethodDelegate7), CS$<>9__CachedAnonymousMethodDelegate8);
                };
            }
            this.lvResults.ItemSelectionChanged += handler;
            if (handler2 == null)
            {
                handler2 = (ItemCheckedEventHandler) ((ls, le) => (this.tsmiSaveChecked.Enabled = this.lvResults.CheckedItems.Count > 0));
            }
            this.lvResults.ItemChecked += handler2;
            this.rbSearch.CheckedChanged += new EventHandler(this.ModeRadio_CheckedChanged);
            this.rbMyAudio.CheckedChanged += new EventHandler(this.ModeRadio_CheckedChanged);
            this.rbGroupPerson.CheckedChanged += new EventHandler(this.ModeRadio_CheckedChanged);
            SetDoubleBuffered(this.lvResults);
            SetDoubleBuffered(this.tbPreviewProgress);
            SetDoubleBuffered(this);
            if (Application.RenderWithVisualStyles)
            {
                VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Button.GroupBox.Normal);
                this.lPreviewHeader.ForeColor = this.lPreviewTime.ForeColor = renderer.GetColor(ColorProperty.TextColor);
            }
            else
            {
                this.lPreviewHeader.Top += 2;
                this.lPreviewTime.Top--;
            }
            this.previewTimer.Tick += new EventHandler(this.previewTimer_Tick);
        }

        private void AddSongs(List<VkSong> songs)
        {
            IEnumerable<ListViewItem> enumerable = this.lvResults.Items.OfType<ListViewItem>();
            List<ListViewItem> list = new List<ListViewItem>();
            using (List<VkSong>.Enumerator enumerator = songs.GetEnumerator())
            {
                Func<ListViewItem, bool> func = null;
                VkSong s;
                while (enumerator.MoveNext())
                {
                    s = enumerator.Current;
                    if (func == null)
                    {
                        <>c__DisplayClass19 class2;
                        func = new Func<ListViewItem, bool>(class2, (IntPtr) this.<AddSongs>b__17);
                    }
                    if (!Enumerable.Any<ListViewItem>(enumerable, func))
                    {
                        TimeSpan span = new TimeSpan(0, 0, s.Duration);
                        ListViewItem item = new ListViewItem(new string[] { s.Artist + " - " + s.Title, span.ToString(@"mm\:ss") }) {
                            Tag = s,
                            ForeColor = (s.LyricsID > 0) ? Color.Blue : Color.Black
                        };
                        list.Add(item);
                    }
                }
            }
            this.lvResults.Items.AddRange(list.ToArray());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnCopyUrl_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.lastUrl))
            {
                Clipboard.SetText(this.lastUrl);
            }
            else
            {
                MessageBox.Show(this, "Поиск не производился!");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Action successHandler = null;
            List<VkSong> songs;
            if (this.lvResults.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "Ничего не выбрано!");
            }
            else
            {
                Settings settings = Settings.Load();
                settings.DownloadImmediate = this.cbDownloadImmediate.Checked;
                settings.Save();
                if (CS$<>9__CachedAnonymousMethodDelegatec == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegatec = new Func<ListViewItem, VkSong>(null, (IntPtr) <btnOk_Click>b__9);
                }
                songs = Enumerable.Select<ListViewItem, VkSong>(this.lvResults.CheckedItems.OfType<ListViewItem>(), CS$<>9__CachedAnonymousMethodDelegatec).ToList<VkSong>();
                if (this.cbDownloadImmediate.Checked)
                {
                    if (successHandler == null)
                    {
                        <>c__DisplayClassf classf;
                        successHandler = new Action(classf, (IntPtr) this.<btnOk_Click>b__a);
                    }
                    songs.Download(this, Common.SongsCachePath, true, successHandler);
                }
                else
                {
                    if (Common.ProcessStreaming)
                    {
                        MessageBox.Show("Not Implemented!");
                    }
                    else
                    {
                        songs.ForEach((Action<VkSong>) (vs => (vs.LocalFilePath = "")));
                    }
                    this.SelectedSongs.AddRange(songs);
                    base.Close();
                }
            }
        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            if (!this.pbBassLoading.Visible)
            {
                VkSong tag = this.gbPreview.Tag as VkSong;
                if ((this.previewState == PlayerState.NotSet) || (this.previewState == PlayerState.Stopped))
                {
                    this.StartSong(tag);
                }
                else if (this.previewState == PlayerState.Playing)
                {
                    this.btnPlayPause.BackgroundImage = Resources.play;
                    VkAudioClass.Instance.InvokeStateChangedEvent(2);
                    this.previewState = PlayerState.Paused;
                }
                else
                {
                    this.btnPlayPause.BackgroundImage = Resources.pause;
                    VkAudioClass.Instance.InvokeStateChangedEvent(2);
                    this.previewState = PlayerState.Playing;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.MakeSearch();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if ((this.previewState == PlayerState.Playing) || (this.previewState == PlayerState.Paused))
            {
                this.StopPreviewPlayback(true);
            }
        }

        private void cbPerformer_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.query))
            {
                this.tbSearch.Text = this.query;
                this.MakeSearch();
            }
        }

        private void ClearList()
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (ListViewItem item in this.lvResults.Items)
            {
                if (item.Checked)
                {
                    list.Add(item);
                    item.BackColor = Color.LightGreen;
                }
            }
            this.lvResults.Items.Clear();
            this.lvResults.Items.AddRange(list.ToArray());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private int GetActiveResultsCount()
        {
            if (CS$<>9__CachedAnonymousMethodDelegate28 == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate28 = new Func<ListViewItem, bool>(null, (IntPtr) <GetActiveResultsCount>b__27);
            }
            return Enumerable.Where<ListViewItem>(this.lvResults.Items.OfType<ListViewItem>(), CS$<>9__CachedAnonymousMethodDelegate28).Count<ListViewItem>();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SearchForm));
            this.panMain = new Panel();
            this.pbBassLoading = new PictureBox();
            this.lPreviewHeader = new Label();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.pbLoading = new PictureBox();
            this.lResult = new Label();
            this.btnCopyUrl = new System.Windows.Forms.Button();
            this.cmsSearchResults = new ContextMenuStrip(this.components);
            this.tsmiSaveSelected = new ToolStripMenuItem();
            this.tsmiSaveChecked = new ToolStripMenuItem();
            this.tsmiShowLyrics = new ToolStripMenuItem();
            this.tsmiPreview = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tsmiCheckAll = new ToolStripMenuItem();
            this.tsmiUncheckAll = new ToolStripMenuItem();
            this.tsmiInvertCheck = new ToolStripMenuItem();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.cbPerformer = new System.Windows.Forms.CheckBox();
            this.rbMyAudio = new System.Windows.Forms.RadioButton();
            this.rbGroupPerson = new System.Windows.Forms.RadioButton();
            this.rbSearch = new System.Windows.Forms.RadioButton();
            this.lSearchMode = new Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.lNameCaption = new Label();
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.lPreviewTime = new Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.panBottom = new Panel();
            this.cbDownloadImmediate = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tsmiCheckSelected = new ToolStripMenuItem();
            this.lvResults = new ListViewEx();
            this.chSong = new ColumnHeader();
            this.chDuration = new ColumnHeader();
            this.tbPreviewProgress = new TrackBarEx();
            this.panMain.SuspendLayout();
            ((ISupportInitialize) this.pbBassLoading).BeginInit();
            this.gbResults.SuspendLayout();
            ((ISupportInitialize) this.pbLoading).BeginInit();
            this.cmsSearchResults.SuspendLayout();
            this.gbSearch.SuspendLayout();
            this.gbPreview.SuspendLayout();
            this.panBottom.SuspendLayout();
            this.tbPreviewProgress.BeginInit();
            base.SuspendLayout();
            this.panMain.Controls.Add(this.pbBassLoading);
            this.panMain.Controls.Add(this.lPreviewHeader);
            this.panMain.Controls.Add(this.gbResults);
            this.panMain.Controls.Add(this.gbSearch);
            this.panMain.Controls.Add(this.gbPreview);
            this.panMain.Dock = DockStyle.Fill;
            this.panMain.Location = new Point(0, 0);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new Padding(3, 3, 3, 0);
            this.panMain.Size = new Size(0x1a1, 0x1d7);
            this.panMain.TabIndex = 1;
            this.pbBassLoading.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.pbBassLoading.Image = Resources.ajax_loader;
            this.pbBassLoading.InitialImage = Resources.ajax_loader;
            this.pbBassLoading.Location = new Point(0x18d, 0x1a7);
            this.pbBassLoading.Name = "pbBassLoading";
            this.pbBassLoading.Size = new Size(12, 12);
            this.pbBassLoading.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pbBassLoading.TabIndex = 15;
            this.pbBassLoading.TabStop = false;
            this.pbBassLoading.Visible = false;
            this.lPreviewHeader.AutoSize = true;
            this.lPreviewHeader.Enabled = false;
            this.lPreviewHeader.Location = new Point(10, 420);
            this.lPreviewHeader.Name = "lPreviewHeader";
            this.lPreviewHeader.Size = new Size(0x2f, 13);
            this.lPreviewHeader.TabIndex = 14;
            this.lPreviewHeader.Text = "Превью";
            this.gbResults.Controls.Add(this.pbLoading);
            this.gbResults.Controls.Add(this.lResult);
            this.gbResults.Controls.Add(this.btnCopyUrl);
            this.gbResults.Controls.Add(this.lvResults);
            this.gbResults.Dock = DockStyle.Fill;
            this.gbResults.Location = new Point(3, 0x5b);
            this.gbResults.Name = "gbResults";
            this.gbResults.Size = new Size(0x19b, 0x14b);
            this.gbResults.TabIndex = 3;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Результаты";
            this.pbLoading.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.pbLoading.Image = Resources.ajax_loader;
            this.pbLoading.InitialImage = Resources.ajax_loader;
            this.pbLoading.Location = new Point(0x184, 0x134);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new Size(0x12, 0x12);
            this.pbLoading.TabIndex = 11;
            this.pbLoading.TabStop = false;
            this.pbLoading.Visible = false;
            this.lResult.AutoSize = true;
            this.lResult.Location = new Point(9, 0x12);
            this.lResult.Name = "lResult";
            this.lResult.Size = new Size(0x44, 13);
            this.lResult.TabIndex = 9;
            this.lResult.Text = "Ожидание...";
            this.btnCopyUrl.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnCopyUrl.FlatStyle = FlatStyle.Flat;
            this.btnCopyUrl.Location = new Point(3, 0x130);
            this.btnCopyUrl.Name = "btnCopyUrl";
            this.btnCopyUrl.Size = new Size(0xa4, 0x17);
            this.btnCopyUrl.TabIndex = 8;
            this.btnCopyUrl.Text = "Скопировать URL в буфер";
            this.btnCopyUrl.UseVisualStyleBackColor = true;
            this.btnCopyUrl.Click += new EventHandler(this.btnCopyUrl_Click);
            this.cmsSearchResults.Items.AddRange(new ToolStripItem[] { this.tsmiSaveSelected, this.tsmiSaveChecked, this.tsmiShowLyrics, this.tsmiPreview, this.toolStripSeparator1, this.tsmiCheckAll, this.tsmiCheckSelected, this.tsmiUncheckAll, this.tsmiInvertCheck });
            this.cmsSearchResults.Name = "cmsSearchResults";
            this.cmsSearchResults.Size = new Size(0xcd, 0xba);
            this.tsmiSaveSelected.Name = "tsmiSaveSelected";
            this.tsmiSaveSelected.Size = new Size(0xcc, 0x16);
            this.tsmiSaveSelected.Text = "Скачать выделенные";
            this.tsmiSaveSelected.Click += new EventHandler(this.tsmiSaveSelected_Click);
            this.tsmiSaveChecked.Name = "tsmiSaveChecked";
            this.tsmiSaveChecked.Size = new Size(0xcc, 0x16);
            this.tsmiSaveChecked.Text = "Скачать выбранные";
            this.tsmiSaveChecked.Click += new EventHandler(this.tsmiSaveChecked_Click);
            this.tsmiShowLyrics.Name = "tsmiShowLyrics";
            this.tsmiShowLyrics.Size = new Size(0xcc, 0x16);
            this.tsmiShowLyrics.Text = "Просмотреть текст песни";
            this.tsmiShowLyrics.Click += new EventHandler(this.tsmiShowLyrics_Click);
            this.tsmiPreview.Name = "tsmiPreview";
            this.tsmiPreview.Size = new Size(0xcc, 0x16);
            this.tsmiPreview.Text = "Превью аудиозаписи";
            this.tsmiPreview.Click += new EventHandler(this.tsmiPreview_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(0xc9, 6);
            this.tsmiCheckAll.Name = "tsmiCheckAll";
            this.tsmiCheckAll.Size = new Size(0xcc, 0x16);
            this.tsmiCheckAll.Text = "Выбрать всё";
            this.tsmiCheckAll.Click += new EventHandler(this.tsmiCheckAll_Click);
            this.tsmiUncheckAll.Name = "tsmiUncheckAll";
            this.tsmiUncheckAll.Size = new Size(0xcc, 0x16);
            this.tsmiUncheckAll.Text = "Отменить выбор";
            this.tsmiUncheckAll.Click += new EventHandler(this.tsmiUncheckAll_Click);
            this.tsmiInvertCheck.Name = "tsmiInvertCheck";
            this.tsmiInvertCheck.Size = new Size(0xcc, 0x16);
            this.tsmiInvertCheck.Text = "Инвертировать выбор";
            this.tsmiInvertCheck.Click += new EventHandler(this.tsmiInvertCheck_Click);
            this.gbSearch.Controls.Add(this.cbPerformer);
            this.gbSearch.Controls.Add(this.rbMyAudio);
            this.gbSearch.Controls.Add(this.rbGroupPerson);
            this.gbSearch.Controls.Add(this.rbSearch);
            this.gbSearch.Controls.Add(this.lSearchMode);
            this.gbSearch.Controls.Add(this.btnSearch);
            this.gbSearch.Controls.Add(this.tbSearch);
            this.gbSearch.Controls.Add(this.lNameCaption);
            this.gbSearch.Dock = DockStyle.Top;
            this.gbSearch.Location = new Point(3, 3);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new Size(0x19b, 0x58);
            this.gbSearch.TabIndex = 2;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Поиск";
            this.cbPerformer.AutoSize = true;
            this.cbPerformer.Location = new Point(0x4c, 0x26);
            this.cbPerformer.Name = "cbPerformer";
            this.cbPerformer.Size = new Size(0x9a, 0x11);
            this.cbPerformer.TabIndex = 4;
            this.cbPerformer.Text = "Только по исполнителям";
            this.cbPerformer.UseVisualStyleBackColor = true;
            this.cbPerformer.CheckedChanged += new EventHandler(this.cbPerformer_CheckedChanged);
            this.rbMyAudio.AutoSize = true;
            this.rbMyAudio.Location = new Point(0x125, 0x12);
            this.rbMyAudio.Name = "rbMyAudio";
            this.rbMyAudio.Size = new Size(0x72, 0x11);
            this.rbMyAudio.TabIndex = 3;
            this.rbMyAudio.Text = "Мои аудиозаписи";
            this.rbMyAudio.UseVisualStyleBackColor = true;
            this.rbGroupPerson.AutoSize = true;
            this.rbGroupPerson.Location = new Point(140, 0x12);
            this.rbGroupPerson.Name = "rbGroupPerson";
            this.rbGroupPerson.Size = new Size(0x92, 0x11);
            this.rbGroupPerson.TabIndex = 2;
            this.rbGroupPerson.Text = "Аудио группы/человека";
            this.rbGroupPerson.UseVisualStyleBackColor = true;
            this.rbSearch.AutoSize = true;
            this.rbSearch.Checked = true;
            this.rbSearch.Location = new Point(0x4c, 0x12);
            this.rbSearch.Name = "rbSearch";
            this.rbSearch.Size = new Size(0x39, 0x11);
            this.rbSearch.TabIndex = 1;
            this.rbSearch.TabStop = true;
            this.rbSearch.Text = "Поиск";
            this.rbSearch.UseVisualStyleBackColor = true;
            this.lSearchMode.AutoSize = true;
            this.lSearchMode.Location = new Point(9, 20);
            this.lSearchMode.Name = "lSearchMode";
            this.lSearchMode.Size = new Size(0x2d, 13);
            this.lSearchMode.TabIndex = 2;
            this.lSearchMode.Text = "Режим:";
            this.btnSearch.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnSearch.BackgroundImage = Resources.arrow_right;
            this.btnSearch.BackgroundImageLayout = ImageLayout.Center;
            this.btnSearch.FlatStyle = FlatStyle.Popup;
            this.btnSearch.Location = new Point(0x17f, 0x3b);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(20, 20);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.tbSearch.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbSearch.BorderStyle = BorderStyle.FixedSingle;
            this.tbSearch.Location = new Point(0x4b, 0x3b);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new Size(0x12e, 20);
            this.tbSearch.TabIndex = 5;
            this.tbSearch.KeyPress += new KeyPressEventHandler(this.tbSearch_KeyPress);
            this.lNameCaption.AutoSize = true;
            this.lNameCaption.Location = new Point(9, 0x3e);
            this.lNameCaption.Name = "lNameCaption";
            this.lNameCaption.Size = new Size(60, 13);
            this.lNameCaption.TabIndex = 0;
            this.lNameCaption.Text = "Название:";
            this.gbPreview.Controls.Add(this.lPreviewTime);
            this.gbPreview.Controls.Add(this.btnStop);
            this.gbPreview.Controls.Add(this.btnPlayPause);
            this.gbPreview.Controls.Add(this.tbPreviewProgress);
            this.gbPreview.Dock = DockStyle.Bottom;
            this.gbPreview.Enabled = false;
            this.gbPreview.Location = new Point(3, 0x1a6);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.Size = new Size(0x19b, 0x31);
            this.gbPreview.TabIndex = 13;
            this.gbPreview.TabStop = false;
            this.lPreviewTime.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.lPreviewTime.AutoSize = true;
            this.lPreviewTime.Enabled = false;
            this.lPreviewTime.Location = new Point(0x14f, 0);
            this.lPreviewTime.Name = "lPreviewTime";
            this.lPreviewTime.Size = new Size(60, 13);
            this.lPreviewTime.TabIndex = 0x10;
            this.lPreviewTime.Text = "0:00 / 0:00";
            this.lPreviewTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStop.BackgroundImage = Resources.stop;
            this.btnStop.BackgroundImageLayout = ImageLayout.Center;
            this.btnStop.FlatStyle = FlatStyle.Popup;
            this.btnStop.Location = new Point(0x22, 0x13);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new Size(20, 20);
            this.btnStop.TabIndex = 10;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new EventHandler(this.btnStop_Click);
            this.btnPlayPause.BackgroundImage = Resources.play;
            this.btnPlayPause.BackgroundImageLayout = ImageLayout.Center;
            this.btnPlayPause.FlatStyle = FlatStyle.Popup;
            this.btnPlayPause.Location = new Point(8, 0x13);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new Size(20, 20);
            this.btnPlayPause.TabIndex = 9;
            this.btnPlayPause.UseVisualStyleBackColor = true;
            this.btnPlayPause.Click += new EventHandler(this.btnPlayPause_Click);
            this.panBottom.Controls.Add(this.cbDownloadImmediate);
            this.panBottom.Controls.Add(this.btnCancel);
            this.panBottom.Controls.Add(this.btnOk);
            this.panBottom.Dock = DockStyle.Bottom;
            this.panBottom.Location = new Point(0, 0x1d7);
            this.panBottom.Name = "panBottom";
            this.panBottom.Size = new Size(0x1a1, 0x2a);
            this.panBottom.TabIndex = 8;
            this.cbDownloadImmediate.AutoSize = true;
            this.cbDownloadImmediate.FlatStyle = FlatStyle.Flat;
            this.cbDownloadImmediate.Location = new Point(11, 12);
            this.cbDownloadImmediate.Name = "cbDownloadImmediate";
            this.cbDownloadImmediate.Size = new Size(0x9a, 0x11);
            this.cbDownloadImmediate.TabIndex = 12;
            this.cbDownloadImmediate.Text = "Скачать сразу полностью";
            this.cbDownloadImmediate.UseVisualStyleBackColor = true;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Location = new Point(0x13d, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x5c, 0x17);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnOk.FlatStyle = FlatStyle.Flat;
            this.btnOk.Location = new Point(0xdb, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x5c, 0x17);
            this.btnOk.TabIndex = 13;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.tsmiCheckSelected.Name = "tsmiCheckSelected";
            this.tsmiCheckSelected.Size = new Size(0xcc, 0x16);
            this.tsmiCheckSelected.Text = "Выбрать выделенные";
            this.tsmiCheckSelected.Click += new EventHandler(this.tsmiCheckSelected_Click);
            this.lvResults.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvResults.BorderStyle = BorderStyle.FixedSingle;
            this.lvResults.CheckBoxes = true;
            this.lvResults.Columns.AddRange(new ColumnHeader[] { this.chSong, this.chDuration });
            this.lvResults.ContextMenuStrip = this.cmsSearchResults;
            this.lvResults.FullRowSelect = true;
            this.lvResults.GridLines = true;
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new Point(3, 40);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new Size(0x195, 0x105);
            this.lvResults.TabIndex = 7;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = View.Details;
            this.lvResults.Scroll += new EventHandler(this.lvResults_Scroll);
            this.lvResults.MouseDoubleClick += new MouseEventHandler(this.lvResults_MouseDoubleClick);
            this.chSong.Text = "Аудиозапись";
            this.chSong.Width = 0x125;
            this.chDuration.Text = "Длительность";
            this.chDuration.Width = 0x59;
            this.tbPreviewProgress.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbPreviewProgress.AutoSize = false;
            this.tbPreviewProgress.LargeChange = 0;
            this.tbPreviewProgress.Location = new Point(0x3a, 15);
            this.tbPreviewProgress.Maximum = 100;
            this.tbPreviewProgress.Name = "tbPreviewProgress";
            this.tbPreviewProgress.Size = new Size(350, 0x1f);
            this.tbPreviewProgress.SmallChange = 0;
            this.tbPreviewProgress.TabIndex = 11;
            this.tbPreviewProgress.TickFrequency = 10;
            this.tbPreviewProgress.ValueChanged += new EventHandler(this.tbPreviewProgress_ValueChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x1a1, 0x201);
            base.Controls.Add(this.panMain);
            base.Controls.Add(this.panBottom);
            this.DoubleBuffered = true;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x1a9, 540);
            base.Name = "SearchForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Поиск песен";
            base.FormClosed += new FormClosedEventHandler(this.SearchForm_FormClosed);
            base.Load += new EventHandler(this.SearchForm_Load);
            base.Shown += new EventHandler(this.SearchForm_Shown);
            base.Resize += new EventHandler(this.SearchForm_Resize);
            this.panMain.ResumeLayout(false);
            this.panMain.PerformLayout();
            ((ISupportInitialize) this.pbBassLoading).EndInit();
            this.gbResults.ResumeLayout(false);
            this.gbResults.PerformLayout();
            ((ISupportInitialize) this.pbLoading).EndInit();
            this.cmsSearchResults.ResumeLayout(false);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.gbPreview.ResumeLayout(false);
            this.gbPreview.PerformLayout();
            this.panBottom.ResumeLayout(false);
            this.panBottom.PerformLayout();
            this.tbPreviewProgress.EndInit();
            base.ResumeLayout(false);
        }

        private void InitPreview(VkSong song, bool startPlaying)
        {
            if ((this.previewState == PlayerState.Paused) || (this.previewState == PlayerState.Playing))
            {
                this.StopPreviewPlayback(false);
            }
            this.gbPreview.Tag = song;
            this.tbPreviewProgress.Minimum = 0;
            this.tbPreviewProgress.Maximum = song.Duration;
            this.tbPreviewProgress.Value = 0;
            this.tbPreviewProgress.Enabled = false;
            this.gbPreview.Enabled = this.lPreviewHeader.Enabled = this.lPreviewTime.Enabled = true;
            if (startPlaying)
            {
                this.StartSong(song);
            }
            this.SetPositionTitle(song, 0);
        }

        private void lvResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = this.lvResults.HitTest(e.Location);
            if (info.Item != null)
            {
                this.InitPreview(info.Item.Tag as VkSong, true);
            }
        }

        private void lvResults_Scroll(object sender, EventArgs e)
        {
            if (((!this.doneScrolling && (!this.requestingAddSongs && (this.GetActiveResultsCount() <= 900))) && ((!this.rbMyAudio.Checked && !this.rbGroupPerson.Checked) || (this.GetActiveResultsCount() <= 50))) && (this.totalCount > 50))
            {
                int num = 1;
                for (int i = this.lvResults.TopItem.Index + 1; i < this.lvResults.Items.Count; i++)
                {
                    if (this.lvResults.Items[i].Bounds.Bottom >= this.lvResults.Bounds.Bottom)
                    {
                        break;
                    }
                    num++;
                }
                if ((this.lvResults.TopItem.Index + (num * 2)) >= this.lvResults.Items.Count)
                {
                    <>c__DisplayClass2b classb;
                    this.requestingAddSongs = true;
                    this.pbLoading.Visible = true;
                    this.rbSearch.Enabled = this.rbMyAudio.Enabled = this.rbGroupPerson.Enabled = this.btnSearch.Enabled = this.tbSearch.Enabled = false;
                    List<VkSong> newSongs = null;
                    int offset = this.RoundOffset(this.lvResults.Items.Count);
                    ThreadWorker.PerformSimpleOperation(this, delegate {
                        if (this.rbSearch.Checked)
                        {
                            newSongs = VkTools.GetSongsOffset(this.query, this.lastUrl, offset);
                        }
                        if (this.rbMyAudio.Checked)
                        {
                            newSongs = VkTools.GetMySongsRest();
                        }
                        if (this.rbGroupPerson.Checked)
                        {
                            newSongs = VkTools.GetGroupPersonSongsRest(this.query);
                        }
                    }, new Action(classb, (IntPtr) this.<lvResults_Scroll>b__2a));
                }
            }
        }

        private void MakeSearch()
        {
            <>c__DisplayClass15 class2;
            List<VkSong> songs = null;
            string text = this.tbSearch.Text;
            ThreadWorker.PerformOperation(this, delegate {
                this.query = "";
                if (this.rbSearch.Checked)
                {
                    songs = VkTools.FindSongs(this.tbSearch.Text, this.cbPerformer.Checked, out this.totalCount);
                    this.lastUrl = VkTools.GenerateUrl(this.tbSearch.Text, this.cbPerformer.Checked);
                }
                if (this.rbMyAudio.Checked)
                {
                    songs = VkTools.GetMySongs(out this.totalCount);
                }
                if (this.rbGroupPerson.Checked)
                {
                    songs = VkTools.GetGroupUserSongs(this.tbSearch.Text, out this.totalCount);
                    this.lastUrl = VkTools.GetGroupPersonAudioAddress(this.tbSearch.Text);
                }
            }, false, "Идёт поиск...", null, (ls, le) => VkTools.FreeResources(), new Action(class2, (IntPtr) this.<MakeSearch>b__13));
        }

        private void ModeRadio_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton button = sender as System.Windows.Forms.RadioButton;
            if (button.Checked)
            {
                if ((button == this.rbSearch) || (button == this.rbMyAudio))
                {
                    this.lNameCaption.Text = "Название:";
                }
                else
                {
                    this.lNameCaption.Text = "Страница:";
                }
                this.tbSearch.Enabled = button != this.rbMyAudio;
                this.btnCopyUrl.Enabled = button != this.rbMyAudio;
                this.lvResults.SuspendLayout();
                this.ClearList();
                this.lvResults.ResumeLayout(true);
                this.lastUrl = "";
                this.query = "";
                this.totalCount = 0;
                this.lResult.Text = "Ожидание...";
                this.cbPerformer.Enabled = button == this.rbSearch;
                if (button == this.rbMyAudio)
                {
                    this.MakeSearch();
                }
            }
        }

        public void NotifyPositionChanged(int Position)
        {
            lock (this.posChangeLocker)
            {
                if (((this.lastSelectedProgress == -1) || (Math.Abs((int) (this.lastSelectedProgress - Position)) <= 1)) && (Position != (this.tbPreviewProgress.Value - 1)))
                {
                    this.tbPreviewProgress.Enabled = true;
                    if (this.lastSelectedProgress > Position)
                    {
                        Position = this.lastSelectedProgress;
                    }
                    else
                    {
                        this.lastSelectedProgress = -1;
                    }
                    if ((this.gbPreview.Enabled && (Position <= this.tbPreviewProgress.Maximum)) && ((Position > -1) && (Position != this.tbPreviewProgress.Value)))
                    {
                        this.posInternalChanging = true;
                        this.tbPreviewProgress.Value = Position;
                        VkSong tag = this.gbPreview.Tag as VkSong;
                        this.SetPositionTitle(tag, Position);
                        this.posInternalChanging = false;
                    }
                }
            }
        }

        public void NotifyStateChanged(int State)
        {
            if (State == 2)
            {
                if (this.previewState == PlayerState.Playing)
                {
                    this.btnPlayPause.BackgroundImage = Resources.play;
                    this.previewState = PlayerState.Paused;
                }
                else
                {
                    this.btnPlayPause.BackgroundImage = Resources.pause;
                    this.previewState = PlayerState.Playing;
                }
            }
        }

        private string PositionToString(int pos)
        {
            int num = pos / 60;
            int num2 = pos - (num * 60);
            return (num.ToString() + ":" + num2.ToString().PadLeft(2, '0'));
        }

        private void previewTimer_Tick(object sender, EventArgs e)
        {
            VkAudioClass.Instance.InvokePositionRequested();
        }

        private void ResetPreview()
        {
            this.gbPreview.Enabled = this.lPreviewTime.Enabled = this.lPreviewHeader.Enabled = false;
            this.gbPreview.Tag = null;
            this.lPreviewHeader.Text = "Превью";
            this.btnPlayPause.BackgroundImage = Resources.play;
            using (Graphics graphics = base.CreateGraphics())
            {
                string text = "0:00 / 0:00";
                int num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewTime.Font).Width);
                this.lPreviewTime.Left = (this.gbPreview.Width - num) - this.lPreviewHeader.Left;
                this.lPreviewTime.Text = text;
            }
        }

        private int RoundOffset(int offset)
        {
            int num = offset;
            if ((num % 100) != 0)
            {
                num += 100 - (num % 100);
            }
            return num;
        }

        private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CurrentForm = null;
            if ((this.previewState == PlayerState.Playing) || (this.previewState == PlayerState.Paused))
            {
                this.StopPreviewPlayback(true);
            }
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            Settings settings = Settings.Load();
            this.cbDownloadImmediate.Checked = settings.DownloadImmediate;
            this.cbDownloadImmediate.Visible = Common.ProcessStreaming;
            this.chSong.Width = (this.lvResults.Width - this.chDuration.Width) - SystemInformation.VerticalScrollBarWidth;
            this.tbSearch.Focus();
            CurrentForm = this;
            this.SetTimeTitle(0, 0);
        }

        private void SearchForm_Resize(object sender, EventArgs e)
        {
            this.chSong.Width = (this.lvResults.Width - this.chDuration.Width) - SystemInformation.VerticalScrollBarWidth;
            if (this.gbPreview.Tag != null)
            {
                VkSong tag = this.gbPreview.Tag as VkSong;
                using (Graphics graphics = base.CreateGraphics())
                {
                    string text = "Превью | " + tag.ToString();
                    int num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewHeader.Font).Width);
                    if ((num + this.lPreviewHeader.Left) >= (this.lPreviewTime.Left - 10))
                    {
                        string str2 = tag.ToString();
                        while ((num + this.lPreviewHeader.Left) >= (this.lPreviewTime.Left - 10))
                        {
                            str2 = str2.Remove(str2.Length - 1);
                            text = "Превью | " + str2 + "...";
                            num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewHeader.Font).Width);
                        }
                    }
                    this.lPreviewHeader.Text = text;
                }
            }
        }

        private void SearchForm_Shown(object sender, EventArgs e)
        {
            this.tbSearch.Focus();
        }

        public static void SetDoubleBuffered(Control c)
        {
            if (!SystemInformation.TerminalServerSession)
            {
                typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, true, null);
            }
        }

        private void SetPositionTitle(VkSong song, int pos)
        {
            using (Graphics graphics = base.CreateGraphics())
            {
                this.SetTimeTitle(pos, song.Duration);
                string text = "Превью | " + song.ToString();
                int num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewHeader.Font).Width);
                if ((num + this.lPreviewHeader.Left) >= (this.lPreviewTime.Left - 10))
                {
                    string str2 = song.ToString();
                    while ((num + this.lPreviewHeader.Left) >= (this.lPreviewTime.Left - 10))
                    {
                        str2 = str2.Remove(str2.Length - 1);
                        text = "Превью | " + str2 + "...";
                        num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewHeader.Font).Width);
                    }
                }
                this.lPreviewHeader.Text = text;
            }
        }

        private void SetTimeTitle(int pos, int duration)
        {
            using (Graphics graphics = base.CreateGraphics())
            {
                string text = this.PositionToString(pos) + " / " + this.PositionToString(duration);
                int num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewTime.Font).Width);
                this.lPreviewTime.Left = (this.gbPreview.Width - num) - this.lPreviewHeader.Left;
                this.lPreviewTime.Text = text;
            }
        }

        private void StartSong(VkSong song)
        {
            this.lastSelectedProgress = -1;
            this.tbPreviewProgress.Enabled = false;
            VkAudioClass.Instance.InvokeLoadUrl(song.URL);
            this.btnPlayPause.BackgroundImage = Resources.pause;
            this.previewState = PlayerState.Playing;
            this.previewTimer.Start();
        }

        public void StopPreviewPlayback(bool notifyClient)
        {
            this.previewTimer.Stop();
            VkSong tag = this.gbPreview.Tag as VkSong;
            if (notifyClient)
            {
                VkAudioClass.Instance.InvokeStateChangedEvent(0);
            }
            this.btnPlayPause.BackgroundImage = Resources.play;
            this.previewState = PlayerState.Stopped;
            this.tbPreviewProgress.Value = 0;
            this.SetPositionTitle(tag, this.tbPreviewProgress.Value);
        }

        private void tbPreviewProgress_ValueChanged(object sender, EventArgs e)
        {
            lock (this.posChangeLocker)
            {
                if (!this.posInternalChanging && ((this.previewState == PlayerState.Playing) || (this.previewState == PlayerState.Paused)))
                {
                    VkSong tag = this.gbPreview.Tag as VkSong;
                    this.SetPositionTitle(tag, this.tbPreviewProgress.Value);
                    Application.DoEvents();
                    this.lastSelectedProgress = this.tbPreviewProgress.Value;
                    VkAudioClass.Instance.InvokePositionChangedEvent(this.tbPreviewProgress.Value);
                }
            }
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == '\r') || (e.KeyChar == '\n'))
            {
                this.MakeSearch();
            }
        }

        private void tsmiCheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvResults.Items)
            {
                item.Checked = true;
            }
        }

        private void tsmiCheckSelected_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvResults.SelectedItems)
            {
                item.Checked = true;
            }
        }

        private void tsmiInvertCheck_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvResults.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void tsmiPreview_Click(object sender, EventArgs e)
        {
            if (CS$<>9__CachedAnonymousMethodDelegate2f == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate2f = new Func<ListViewItem, VkSong>(null, (IntPtr) <tsmiPreview_Click>b__2e);
            }
            VkSong song = Enumerable.Select<ListViewItem, VkSong>(this.lvResults.SelectedItems.OfType<ListViewItem>(), CS$<>9__CachedAnonymousMethodDelegate2f).FirstOrDefault<VkSong>();
            if (song != null)
            {
                this.InitPreview(song, true);
            }
        }

        private void tsmiSaveChecked_Click(object sender, EventArgs e)
        {
            if (CS$<>9__CachedAnonymousMethodDelegate1e == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate1e = new Func<ListViewItem, VkSong>(null, (IntPtr) <tsmiSaveChecked_Click>b__1d);
            }
            Enumerable.Select<ListViewItem, VkSong>(this.lvResults.CheckedItems.OfType<ListViewItem>(), CS$<>9__CachedAnonymousMethodDelegate1e).ToList<VkSong>().DownloadInterfaced(this);
        }

        private void tsmiSaveSelected_Click(object sender, EventArgs e)
        {
            if (CS$<>9__CachedAnonymousMethodDelegate1c == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate1c = new Func<ListViewItem, VkSong>(null, (IntPtr) <tsmiSaveSelected_Click>b__1b);
            }
            Enumerable.Select<ListViewItem, VkSong>(this.lvResults.SelectedItems.OfType<ListViewItem>(), CS$<>9__CachedAnonymousMethodDelegate1c).ToList<VkSong>().DownloadInterfaced(this);
        }

        private void tsmiShowLyrics_Click(object sender, EventArgs e)
        {
            Action<string> successHandler = null;
            if (CS$<>9__CachedAnonymousMethodDelegate22 == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate22 = new Func<ListViewItem, VkSong>(null, (IntPtr) <tsmiShowLyrics_Click>b__1f);
            }
            if (CS$<>9__CachedAnonymousMethodDelegate23 == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate23 = new Func<VkSong, bool>(null, (IntPtr) <tsmiShowLyrics_Click>b__20);
            }
            VkSong s = Enumerable.FirstOrDefault<VkSong>(Enumerable.Select<ListViewItem, VkSong>(this.lvResults.SelectedItems.OfType<ListViewItem>(), CS$<>9__CachedAnonymousMethodDelegate22), CS$<>9__CachedAnonymousMethodDelegate23);
            if (s != null)
            {
                if (successHandler == null)
                {
                    successHandler = (Action<string>) (str => new BigMessageBoxForm("Текст песни " + s.ToString(), str).ShowDialog(this));
                }
                s.GetLyrics(this, successHandler);
            }
        }

        private void tsmiUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvResults.Items)
            {
                item.Checked = false;
            }
        }

        private void UpdateResultsStatus()
        {
            this.lResult.Text = (this.totalCount == 0) ? "Ничего не найдено" : ("Найдено аудиозаписей: " + this.totalCount.ToString());
        }

        public static SearchForm CurrentForm
        {
            [CompilerGenerated]
            get
            {
                return <CurrentForm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                <CurrentForm>k__BackingField = value;
            }
        }

        public List<VkSong> SelectedSongs
        {
            get
            {
                return (this._SelectedSongs ?? (this._SelectedSongs = new List<VkSong>()));
            }
        }
    }
}

