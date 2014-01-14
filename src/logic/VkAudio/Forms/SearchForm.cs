// Type: VkAudio.Forms.SearchForm
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using VkAudio;
using VkAudio.Classes;
using VkAudio.Controls;
using VkAudio.Properties;

namespace VkAudio.Forms
{
  public class SearchForm : Form
  {
    private string lastUrl = string.Empty;
    private Timer previewTimer = new Timer()
    {
      Interval = 100
    };
    private string query = "";
    private object posChangeLocker = new object();
    private PlayerState previewState = PlayerState.NotSet;
    private int lastSelectedProgress = -1;
    private IContainer components;
    private Panel panMain;
    private System.Windows.Forms.GroupBox gbResults;
    private ListViewEx lvResults;
    private ColumnHeader chSong;
    private ColumnHeader chDuration;
    private ContextMenuStrip cmsSearchResults;
    private ToolStripMenuItem tsmiSaveSelected;
    private ToolStripMenuItem tsmiSaveChecked;
    private System.Windows.Forms.Button btnCopyUrl;
    private ToolStripMenuItem tsmiShowLyrics;
    private Label lResult;
    private PictureBox pbLoading;
    private System.Windows.Forms.GroupBox gbSearch;
    private System.Windows.Forms.RadioButton rbMyAudio;
    private System.Windows.Forms.RadioButton rbGroupPerson;
    private System.Windows.Forms.RadioButton rbSearch;
    private Label lSearchMode;
    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.TextBox tbSearch;
    private Label lNameCaption;
    private Panel panBottom;
    private System.Windows.Forms.CheckBox cbDownloadImmediate;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.GroupBox gbPreview;
    private TrackBarEx tbPreviewProgress;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnPlayPause;
    private ToolStripMenuItem tsmiPreview;
    private Label lPreviewTime;
    private Label lPreviewHeader;
    private PictureBox pbBassLoading;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem tsmiCheckAll;
    private ToolStripMenuItem tsmiUncheckAll;
    private ToolStripMenuItem tsmiInvertCheck;
    private System.Windows.Forms.CheckBox cbPerformer;
    private ToolStripMenuItem tsmiCheckSelected;
    private List<VkSong> _SelectedSongs;
    private int totalCount;
    private bool requestingAddSongs;
    private bool doneScrolling;
    private bool posInternalChanging;

    public List<VkSong> SelectedSongs
    {
      get
      {
        return this._SelectedSongs ?? (this._SelectedSongs = new List<VkSong>());
      }
    }

    public static SearchForm CurrentForm { get; private set; }

    public SearchForm()
    {
      this.InitializeComponent();
      this.tbSearch.Focus();
      this.tsmiSaveSelected.Enabled = this.tsmiSaveChecked.Enabled = this.tsmiShowLyrics.Enabled = false;
      this.lvResults.ItemSelectionChanged += (ListViewItemSelectionChangedEventHandler) ((ls, le) =>
      {
        this.tsmiSaveSelected.Enabled = this.lvResults.SelectedItems.Count > 0;
        this.tsmiShowLyrics.Enabled = Enumerable.Any<VkSong>(Enumerable.Select<ListViewItem, VkSong>(Enumerable.OfType<ListViewItem>((IEnumerable) this.lvResults.SelectedItems), (Func<ListViewItem, VkSong>) (lvi => lvi.Tag as VkSong)), (Func<VkSong, bool>) (vs => vs.LyricsID > 0));
      });
      this.lvResults.ItemChecked += (ItemCheckedEventHandler) ((ls, le) => this.tsmiSaveChecked.Enabled = this.lvResults.CheckedItems.Count > 0);
      this.rbSearch.CheckedChanged += new EventHandler(this.ModeRadio_CheckedChanged);
      this.rbMyAudio.CheckedChanged += new EventHandler(this.ModeRadio_CheckedChanged);
      this.rbGroupPerson.CheckedChanged += new EventHandler(this.ModeRadio_CheckedChanged);
      SearchForm.SetDoubleBuffered((Control) this.lvResults);
      SearchForm.SetDoubleBuffered((Control) this.tbPreviewProgress);
      SearchForm.SetDoubleBuffered((Control) this);
      if (Application.RenderWithVisualStyles)
      {
        this.lPreviewHeader.ForeColor = this.lPreviewTime.ForeColor = new VisualStyleRenderer(VisualStyleElement.Button.GroupBox.Normal).GetColor(ColorProperty.TextColor);
      }
      else
      {
        Label label1 = this.lPreviewHeader;
        int num1 = label1.Top + 2;
        label1.Top = num1;
        Label label2 = this.lPreviewTime;
        int num2 = label2.Top - 1;
        label2.Top = num2;
      }
      this.previewTimer.Tick += new EventHandler(this.previewTimer_Tick);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
	  System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
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
      this.gbResults.SuspendLayout();
      this.cmsSearchResults.SuspendLayout();
      this.gbSearch.SuspendLayout();
      this.gbPreview.SuspendLayout();
      this.panBottom.SuspendLayout();
      this.tbPreviewProgress.BeginInit();
      this.SuspendLayout();
      this.panMain.Controls.Add((Control) this.pbBassLoading);
      this.panMain.Controls.Add((Control) this.lPreviewHeader);
      this.panMain.Controls.Add((Control) this.gbResults);
      this.panMain.Controls.Add((Control) this.gbSearch);
      this.panMain.Controls.Add((Control) this.gbPreview);
      this.panMain.Dock = DockStyle.Fill;
      this.panMain.Location = new Point(0, 0);
      this.panMain.Name = "panMain";
      this.panMain.Padding = new Padding(3, 3, 3, 0);
      this.panMain.Size = new Size(417, 471);
      this.panMain.TabIndex = 1;
      this.pbBassLoading.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.pbBassLoading.Image = (Image) Resources.ajax_loader;
      this.pbBassLoading.InitialImage = (Image) Resources.ajax_loader;
      this.pbBassLoading.Location = new Point(397, 423);
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
      this.lPreviewHeader.Size = new Size(47, 13);
      this.lPreviewHeader.TabIndex = 14;
      this.lPreviewHeader.Text = "Превью";
      this.gbResults.Controls.Add((Control) this.pbLoading);
      this.gbResults.Controls.Add((Control) this.lResult);
      this.gbResults.Controls.Add((Control) this.btnCopyUrl);
      this.gbResults.Controls.Add((Control) this.lvResults);
      this.gbResults.Dock = DockStyle.Fill;
      this.gbResults.Location = new Point(3, 91);
      this.gbResults.Name = "gbResults";
      this.gbResults.Size = new Size(411, 331);
      this.gbResults.TabIndex = 3;
      this.gbResults.TabStop = false;
      this.gbResults.Text = "Результаты";
      this.pbLoading.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.pbLoading.Image = (Image) Resources.ajax_loader;
      this.pbLoading.InitialImage = (Image) Resources.ajax_loader;
      this.pbLoading.Location = new Point(388, 308);
      this.pbLoading.Name = "pbLoading";
      this.pbLoading.Size = new Size(18, 18);
      this.pbLoading.TabIndex = 11;
      this.pbLoading.TabStop = false;
      this.pbLoading.Visible = false;
      this.lResult.AutoSize = true;
      this.lResult.Location = new Point(9, 18);
      this.lResult.Name = "lResult";
      this.lResult.Size = new Size(68, 13);
      this.lResult.TabIndex = 9;
      this.lResult.Text = "Ожидание...";
      this.btnCopyUrl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnCopyUrl.FlatStyle = FlatStyle.Flat;
      this.btnCopyUrl.Location = new Point(3, 304);
      this.btnCopyUrl.Name = "btnCopyUrl";
      this.btnCopyUrl.Size = new Size(164, 23);
      this.btnCopyUrl.TabIndex = 8;
      this.btnCopyUrl.Text = "Скопировать URL в буфер";
      this.btnCopyUrl.UseVisualStyleBackColor = true;
      this.btnCopyUrl.Click += new EventHandler(this.btnCopyUrl_Click);
      this.cmsSearchResults.Items.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.tsmiSaveSelected,
        (ToolStripItem) this.tsmiSaveChecked,
        (ToolStripItem) this.tsmiShowLyrics,
        (ToolStripItem) this.tsmiPreview,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.tsmiCheckAll,
        (ToolStripItem) this.tsmiCheckSelected,
        (ToolStripItem) this.tsmiUncheckAll,
        (ToolStripItem) this.tsmiInvertCheck
      });
      this.cmsSearchResults.Name = "cmsSearchResults";
      this.cmsSearchResults.Size = new Size(205, 186);
      this.tsmiSaveSelected.Name = "tsmiSaveSelected";
      this.tsmiSaveSelected.Size = new Size(204, 22);
      this.tsmiSaveSelected.Text = "Скачать выделенные";
      this.tsmiSaveSelected.Click += new EventHandler(this.tsmiSaveSelected_Click);
      this.tsmiSaveChecked.Name = "tsmiSaveChecked";
      this.tsmiSaveChecked.Size = new Size(204, 22);
      this.tsmiSaveChecked.Text = "Скачать выбранные";
      this.tsmiSaveChecked.Click += new EventHandler(this.tsmiSaveChecked_Click);
      this.tsmiShowLyrics.Name = "tsmiShowLyrics";
      this.tsmiShowLyrics.Size = new Size(204, 22);
      this.tsmiShowLyrics.Text = "Просмотреть текст песни";
      this.tsmiShowLyrics.Click += new EventHandler(this.tsmiShowLyrics_Click);
      this.tsmiPreview.Name = "tsmiPreview";
      this.tsmiPreview.Size = new Size(204, 22);
      this.tsmiPreview.Text = "Превью аудиозаписи";
      this.tsmiPreview.Click += new EventHandler(this.tsmiPreview_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(201, 6);
      this.tsmiCheckAll.Name = "tsmiCheckAll";
      this.tsmiCheckAll.Size = new Size(204, 22);
      this.tsmiCheckAll.Text = "Выбрать всё";
      this.tsmiCheckAll.Click += new EventHandler(this.tsmiCheckAll_Click);
      this.tsmiUncheckAll.Name = "tsmiUncheckAll";
      this.tsmiUncheckAll.Size = new Size(204, 22);
      this.tsmiUncheckAll.Text = "Отменить выбор";
      this.tsmiUncheckAll.Click += new EventHandler(this.tsmiUncheckAll_Click);
      this.tsmiInvertCheck.Name = "tsmiInvertCheck";
      this.tsmiInvertCheck.Size = new Size(204, 22);
      this.tsmiInvertCheck.Text = "Инвертировать выбор";
      this.tsmiInvertCheck.Click += new EventHandler(this.tsmiInvertCheck_Click);
      this.gbSearch.Controls.Add((Control) this.cbPerformer);
      this.gbSearch.Controls.Add((Control) this.rbMyAudio);
      this.gbSearch.Controls.Add((Control) this.rbGroupPerson);
      this.gbSearch.Controls.Add((Control) this.rbSearch);
      this.gbSearch.Controls.Add((Control) this.lSearchMode);
      this.gbSearch.Controls.Add((Control) this.btnSearch);
      this.gbSearch.Controls.Add((Control) this.tbSearch);
      this.gbSearch.Controls.Add((Control) this.lNameCaption);
      this.gbSearch.Dock = DockStyle.Top;
      this.gbSearch.Location = new Point(3, 3);
      this.gbSearch.Name = "gbSearch";
      this.gbSearch.Size = new Size(411, 88);
      this.gbSearch.TabIndex = 2;
      this.gbSearch.TabStop = false;
      this.gbSearch.Text = "Поиск";
      this.cbPerformer.AutoSize = true;
      this.cbPerformer.Location = new Point(76, 38);
      this.cbPerformer.Name = "cbPerformer";
      this.cbPerformer.Size = new Size(154, 17);
      this.cbPerformer.TabIndex = 4;
      this.cbPerformer.Text = "Только по исполнителям";
      this.cbPerformer.UseVisualStyleBackColor = true;
      this.cbPerformer.CheckedChanged += new EventHandler(this.cbPerformer_CheckedChanged);
      this.rbMyAudio.AutoSize = true;
      this.rbMyAudio.Location = new Point(293, 18);
      this.rbMyAudio.Name = "rbMyAudio";
      this.rbMyAudio.Size = new Size(114, 17);
      this.rbMyAudio.TabIndex = 3;
      this.rbMyAudio.Text = "Мои аудиозаписи";
      this.rbMyAudio.UseVisualStyleBackColor = true;
      this.rbGroupPerson.AutoSize = true;
      this.rbGroupPerson.Location = new Point(140, 18);
      this.rbGroupPerson.Name = "rbGroupPerson";
      this.rbGroupPerson.Size = new Size(146, 17);
      this.rbGroupPerson.TabIndex = 2;
      this.rbGroupPerson.Text = "Аудио группы/человека";
      this.rbGroupPerson.UseVisualStyleBackColor = true;
      this.rbSearch.AutoSize = true;
      this.rbSearch.Checked = true;
      this.rbSearch.Location = new Point(76, 18);
      this.rbSearch.Name = "rbSearch";
      this.rbSearch.Size = new Size(57, 17);
      this.rbSearch.TabIndex = 1;
      this.rbSearch.TabStop = true;
      this.rbSearch.Text = "Поиск";
      this.rbSearch.UseVisualStyleBackColor = true;
      this.lSearchMode.AutoSize = true;
      this.lSearchMode.Location = new Point(9, 20);
      this.lSearchMode.Name = "lSearchMode";
      this.lSearchMode.Size = new Size(45, 13);
      this.lSearchMode.TabIndex = 2;
      this.lSearchMode.Text = "Режим:";
      this.btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSearch.BackgroundImage = (Image) Resources.arrow_right;
      this.btnSearch.BackgroundImageLayout = ImageLayout.Center;
      this.btnSearch.FlatStyle = FlatStyle.Popup;
      this.btnSearch.Location = new Point(383, 59);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(20, 20);
      this.btnSearch.TabIndex = 6;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.tbSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.tbSearch.BorderStyle = BorderStyle.FixedSingle;
      this.tbSearch.Location = new Point(75, 59);
      this.tbSearch.Name = "tbSearch";
      this.tbSearch.Size = new Size(302, 20);
      this.tbSearch.TabIndex = 5;
      this.tbSearch.KeyPress += new KeyPressEventHandler(this.tbSearch_KeyPress);
      this.lNameCaption.AutoSize = true;
      this.lNameCaption.Location = new Point(9, 62);
      this.lNameCaption.Name = "lNameCaption";
      this.lNameCaption.Size = new Size(60, 13);
      this.lNameCaption.TabIndex = 0;
      this.lNameCaption.Text = "Название:";
      this.gbPreview.Controls.Add((Control) this.lPreviewTime);
      this.gbPreview.Controls.Add((Control) this.btnStop);
      this.gbPreview.Controls.Add((Control) this.btnPlayPause);
      this.gbPreview.Controls.Add((Control) this.tbPreviewProgress);
      this.gbPreview.Dock = DockStyle.Bottom;
      this.gbPreview.Enabled = false;
      this.gbPreview.Location = new Point(3, 422);
      this.gbPreview.Name = "gbPreview";
      this.gbPreview.Size = new Size(411, 49);
      this.gbPreview.TabIndex = 13;
      this.gbPreview.TabStop = false;
      this.lPreviewTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lPreviewTime.AutoSize = true;
      this.lPreviewTime.Enabled = false;
      this.lPreviewTime.Location = new Point(335, 0);
      this.lPreviewTime.Name = "lPreviewTime";
      this.lPreviewTime.Size = new Size(60, 13);
      this.lPreviewTime.TabIndex = 16;
      this.lPreviewTime.Text = "0:00 / 0:00";
      this.lPreviewTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btnStop.BackgroundImage = (Image) Resources.stop;
      this.btnStop.BackgroundImageLayout = ImageLayout.Center;
      this.btnStop.FlatStyle = FlatStyle.Popup;
      this.btnStop.Location = new Point(34, 19);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(20, 20);
      this.btnStop.TabIndex = 10;
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.btnPlayPause.BackgroundImage = (Image) Resources.play;
      this.btnPlayPause.BackgroundImageLayout = ImageLayout.Center;
      this.btnPlayPause.FlatStyle = FlatStyle.Popup;
      this.btnPlayPause.Location = new Point(8, 19);
      this.btnPlayPause.Name = "btnPlayPause";
      this.btnPlayPause.Size = new Size(20, 20);
      this.btnPlayPause.TabIndex = 9;
      this.btnPlayPause.UseVisualStyleBackColor = true;
      this.btnPlayPause.Click += new EventHandler(this.btnPlayPause_Click);
      this.panBottom.Controls.Add((Control) this.cbDownloadImmediate);
      this.panBottom.Controls.Add((Control) this.btnCancel);
      this.panBottom.Controls.Add((Control) this.btnOk);
      this.panBottom.Dock = DockStyle.Bottom;
      this.panBottom.Location = new Point(0, 471);
      this.panBottom.Name = "panBottom";
      this.panBottom.Size = new Size(417, 42);
      this.panBottom.TabIndex = 8;
      this.cbDownloadImmediate.AutoSize = true;
      this.cbDownloadImmediate.FlatStyle = FlatStyle.Flat;
      this.cbDownloadImmediate.Location = new Point(11, 12);
      this.cbDownloadImmediate.Name = "cbDownloadImmediate";
      this.cbDownloadImmediate.Size = new Size(154, 17);
      this.cbDownloadImmediate.TabIndex = 12;
      this.cbDownloadImmediate.Text = "Скачать сразу полностью";
      this.cbDownloadImmediate.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.FlatStyle = FlatStyle.Flat;
      this.btnCancel.Location = new Point(317, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(92, 23);
      this.btnCancel.TabIndex = 14;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOk.FlatStyle = FlatStyle.Flat;
      this.btnOk.Location = new Point(219, 9);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(92, 23);
      this.btnOk.TabIndex = 13;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.tsmiCheckSelected.Name = "tsmiCheckSelected";
      this.tsmiCheckSelected.Size = new Size(204, 22);
      this.tsmiCheckSelected.Text = "Выбрать выделенные";
      this.tsmiCheckSelected.Click += new EventHandler(this.tsmiCheckSelected_Click);
      this.lvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvResults.BorderStyle = BorderStyle.FixedSingle;
      this.lvResults.CheckBoxes = true;
      this.lvResults.Columns.AddRange(new ColumnHeader[2]
      {
        this.chSong,
        this.chDuration
      });
      this.lvResults.ContextMenuStrip = this.cmsSearchResults;
      this.lvResults.FullRowSelect = true;
      this.lvResults.GridLines = true;
      this.lvResults.HideSelection = false;
      this.lvResults.Location = new Point(3, 40);
      this.lvResults.Name = "lvResults";
      this.lvResults.Size = new Size(405, 261);
      this.lvResults.TabIndex = 7;
      this.lvResults.UseCompatibleStateImageBehavior = false;
      this.lvResults.View = View.Details;
      this.lvResults.Scroll += new EventHandler(this.lvResults_Scroll);
      this.lvResults.MouseDoubleClick += new MouseEventHandler(this.lvResults_MouseDoubleClick);
      this.chSong.Text = "Аудиозапись";
      this.chSong.Width = 293;
      this.chDuration.Text = "Длительность";
      this.chDuration.Width = 89;
      this.tbPreviewProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.tbPreviewProgress.AutoSize = false;
      this.tbPreviewProgress.LargeChange = 0;
      this.tbPreviewProgress.Location = new Point(58, 15);
      this.tbPreviewProgress.Maximum = 100;
      this.tbPreviewProgress.Name = "tbPreviewProgress";
      this.tbPreviewProgress.Size = new Size(350, 31);
      this.tbPreviewProgress.SmallChange = 0;
      this.tbPreviewProgress.TabIndex = 11;
      this.tbPreviewProgress.TickFrequency = 10;
      this.tbPreviewProgress.ValueChanged += new EventHandler(this.tbPreviewProgress_ValueChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.BackColor = Color.White;
      this.ClientSize = new Size(417, 513);
      this.Controls.Add((Control) this.panMain);
      this.Controls.Add((Control) this.panBottom);
      this.DoubleBuffered = true;
	  this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.MinimumSize = new Size(425, 540);
      this.Name = "SearchForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Поиск песен";
      this.FormClosed += new FormClosedEventHandler(this.SearchForm_FormClosed);
      this.Load += new EventHandler(this.SearchForm_Load);
      this.Shown += new EventHandler(this.SearchForm_Shown);
      this.Resize += new EventHandler(this.SearchForm_Resize);
      this.panMain.ResumeLayout(false);
      this.panMain.PerformLayout();
      this.gbResults.ResumeLayout(false);
      this.gbResults.PerformLayout();
      this.cmsSearchResults.ResumeLayout(false);
      this.gbSearch.ResumeLayout(false);
      this.gbSearch.PerformLayout();
      this.gbPreview.ResumeLayout(false);
      this.gbPreview.PerformLayout();
      this.panBottom.ResumeLayout(false);
      this.panBottom.PerformLayout();
      this.tbPreviewProgress.EndInit();
      this.ResumeLayout(false);
    }

    private void previewTimer_Tick(object sender, EventArgs e)
    {
      VkAudioClass.Instance.InvokePositionRequested();
    }

    public static void SetDoubleBuffered(Control c)
    {
      if (SystemInformation.TerminalServerSession)
        return;
      typeof (Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) c, (object) true, (object[]) null);
    }

    private void ModeRadio_CheckedChanged(object sender, EventArgs e)
    {
      System.Windows.Forms.RadioButton radioButton = sender as System.Windows.Forms.RadioButton;
      if (!radioButton.Checked)
        return;
      if (radioButton == this.rbSearch || radioButton == this.rbMyAudio)
        this.lNameCaption.Text = "Название:";
      else
        this.lNameCaption.Text = "Страница:";
      this.tbSearch.Enabled = radioButton != this.rbMyAudio;
      this.btnCopyUrl.Enabled = radioButton != this.rbMyAudio;
      this.lvResults.SuspendLayout();
      this.ClearList();
      this.lvResults.ResumeLayout(true);
      this.lastUrl = "";
      this.query = "";
      this.totalCount = 0;
      this.lResult.Text = "Ожидание...";
      this.cbPerformer.Enabled = radioButton == this.rbSearch;
      if (radioButton != this.rbMyAudio)
        return;
      this.MakeSearch();
    }

    private void SearchForm_Resize(object sender, EventArgs e)
    {
      this.chSong.Width = this.lvResults.Width - this.chDuration.Width - SystemInformation.VerticalScrollBarWidth;
      if (this.gbPreview.Tag == null)
        return;
      VkSong vkSong = this.gbPreview.Tag as VkSong;
      using (Graphics graphics = this.CreateGraphics())
      {
        string text = "Превью | " + vkSong.ToString();
        int num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewHeader.Font).Width);
        if (num + this.lPreviewHeader.Left >= this.lPreviewTime.Left - 10)
        {
          string str = vkSong.ToString();
          for (; num + this.lPreviewHeader.Left >= this.lPreviewTime.Left - 10; num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewHeader.Font).Width))
          {
            str = str.Remove(str.Length - 1);
            text = "Превью | " + str + "...";
          }
        }
        this.lPreviewHeader.Text = text;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.lvResults.CheckedItems.Count == 0)
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "Ничего не выбрано!");
      }
      else
      {
        Settings settings = Settings.Load();
        settings.DownloadImmediate = this.cbDownloadImmediate.Checked;
        settings.Save();
        List<VkSong> songs = Enumerable.ToList<VkSong>(Enumerable.Select<ListViewItem, VkSong>(Enumerable.OfType<ListViewItem>((IEnumerable) this.lvResults.CheckedItems), (Func<ListViewItem, VkSong>) (lvi => lvi.Tag as VkSong)));
        if (this.cbDownloadImmediate.Checked)
        {
          VkSongListExtension.Download(songs, (IWin32Window) this, Common.SongsCachePath, true, (Action) (() =>
          {
            this.SelectedSongs.AddRange((IEnumerable<VkSong>) songs);
            this.Close();
          }));
        }
        else
        {
          if (Common.ProcessStreaming)
          {
            int num2 = (int) MessageBox.Show("Not Implemented!");
          }
          else
            songs.ForEach((Action<VkSong>) (vs => vs.LocalFilePath = ""));
          this.SelectedSongs.AddRange((IEnumerable<VkSong>) songs);
          this.Close();
        }
      }
    }

    private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
    {
      if ((int) e.KeyChar != 13 && (int) e.KeyChar != 10)
        return;
      this.MakeSearch();
    }

    private void UpdateResultsStatus()
    {
      this.lResult.Text = this.totalCount == 0 ? "Ничего не найдено" : "Найдено аудиозаписей: " + this.totalCount.ToString();
    }

    private void ClearList()
    {
      List<ListViewItem> list = new List<ListViewItem>();
      foreach (ListViewItem listViewItem in this.lvResults.Items)
      {
        if (listViewItem.Checked)
        {
          list.Add(listViewItem);
          listViewItem.BackColor = Color.LightGreen;
        }
      }
      this.lvResults.Items.Clear();
      this.lvResults.Items.AddRange(list.ToArray());
    }

    private void MakeSearch()
    {
      List<VkSong> songs = (List<VkSong>) null;
      string text = this.tbSearch.Text;
      ThreadWorker.PerformOperation((IWin32Window) this, (ThreadOperation) (() =>
      {
        this.query = "";
        if (this.rbSearch.Checked)
        {
          songs = VkTools.FindSongs(this.tbSearch.Text, this.cbPerformer.Checked, out this.totalCount);
#if !DEBUG
#error Add logic
#endif
          this.lastUrl = string.Empty;
        }
        if (this.rbMyAudio.Checked)
        {
	        songs = VkTools.GetMySongs();
	        totalCount = songs.Count;
        }
        if (!this.rbGroupPerson.Checked)
          return;
        songs = VkTools.GetGroupUserSongs(this.tbSearch.Text, out this.totalCount);
#if !DEBUG
#error Add logic
#endif
		this.lastUrl = string.Empty;
      }), false, "Идёт поиск...", new int?(), (EventHandler) ((ls, le) => VkTools.FreeResources()), (Action) (() =>
      {
        this.query = this.tbSearch.Text;
        this.lvResults.SuspendLayout();
        this.ClearList();
        if (songs != null)
          this.AddSongs(songs);
        this.lvResults.ResumeLayout(true);
        this.UpdateResultsStatus();
        if (songs != null && songs.Count > 0)
        {
          this.lvResults.Select();
          this.lvResults.SelectedIndices.Clear();
          this.lvResults.SelectedIndices.Add(0);
        }
        this.doneScrolling = false;
      }));
    }

    private void AddSongs(List<VkSong> songs)
    {
      IEnumerable<ListViewItem> source = Enumerable.OfType<ListViewItem>((IEnumerable) this.lvResults.Items);
      List<ListViewItem> list = new List<ListViewItem>();
      using (List<VkSong>.Enumerator enumerator = songs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          VkSong s = enumerator.Current;
          if (!Enumerable.Any<ListViewItem>(source, (Func<ListViewItem, bool>) (i => (i.Tag as VkSong).ID.Equals(s.ID, StringComparison.CurrentCultureIgnoreCase))))
          {
            TimeSpan timeSpan = new TimeSpan(0, 0, s.Duration);
            list.Add(new ListViewItem(new string[2]
            {
              s.Artist + " - " + s.Title,
              timeSpan.ToString("mm\\:ss")
            })
            {
              Tag = (object) s,
              ForeColor = s.LyricsID > 0 ? Color.Blue : Color.Black
            });
          }
        }
      }
      this.lvResults.Items.AddRange(list.ToArray());
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.MakeSearch();
    }

    private void SearchForm_Load(object sender, EventArgs e)
    {
      this.cbDownloadImmediate.Checked = Settings.Load().DownloadImmediate;
      this.cbDownloadImmediate.Visible = Common.ProcessStreaming;
      this.chSong.Width = this.lvResults.Width - this.chDuration.Width - SystemInformation.VerticalScrollBarWidth;
      this.tbSearch.Focus();
      SearchForm.CurrentForm = this;
      this.SetTimeTitle(0, 0);
    }

    private void tsmiSaveSelected_Click(object sender, EventArgs e)
    {
      VkSongListExtension.DownloadInterfaced(Enumerable.ToList<VkSong>(Enumerable.Select<ListViewItem, VkSong>(Enumerable.OfType<ListViewItem>((IEnumerable) this.lvResults.SelectedItems), (Func<ListViewItem, VkSong>) (lvi => lvi.Tag as VkSong))), (IWin32Window) this);
    }

    private void tsmiSaveChecked_Click(object sender, EventArgs e)
    {
      VkSongListExtension.DownloadInterfaced(Enumerable.ToList<VkSong>(Enumerable.Select<ListViewItem, VkSong>(Enumerable.OfType<ListViewItem>((IEnumerable) this.lvResults.CheckedItems), (Func<ListViewItem, VkSong>) (lvi => lvi.Tag as VkSong))), (IWin32Window) this);
    }

    private void btnCopyUrl_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.lastUrl))
      {
        Clipboard.SetText(this.lastUrl);
      }
      else
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Поиск не производился!");
      }
    }

    private void tsmiShowLyrics_Click(object sender, EventArgs e)
    {
      VkSong s = Enumerable.FirstOrDefault<VkSong>(Enumerable.Select<ListViewItem, VkSong>(Enumerable.OfType<ListViewItem>((IEnumerable) this.lvResults.SelectedItems), (Func<ListViewItem, VkSong>) (lvi => lvi.Tag as VkSong)), (Func<VkSong, bool>) (vs => vs.LyricsID > 0));
      if (s == null)
        return;
      int num;
      s.GetLyrics((Form) this, (Action<string>) (str => num = (int) new BigMessageBoxForm("Текст песни " + s.ToString(), str).ShowDialog((IWin32Window) this)));
    }

    private void SearchForm_Shown(object sender, EventArgs e)
    {
      this.tbSearch.Focus();
    }

    private int RoundOffset(int offset)
    {
      int num = offset;
      if (num % 100 != 0)
        num += 100 - num % 100;
      return num;
    }

    private int GetActiveResultsCount()
    {
      return Enumerable.Count<ListViewItem>(Enumerable.Where<ListViewItem>(Enumerable.OfType<ListViewItem>((IEnumerable) this.lvResults.Items), (Func<ListViewItem, bool>) (lvi => lvi.BackColor != Color.LightGreen)));
    }

    private void lvResults_Scroll(object sender, EventArgs e)
    {
      if (this.doneScrolling || this.requestingAddSongs || this.GetActiveResultsCount() > 900 || (this.rbMyAudio.Checked || this.rbGroupPerson.Checked) && this.GetActiveResultsCount() > 50 || this.totalCount <= 50)
        return;
      int num = 1;
      for (int index = this.lvResults.TopItem.Index + 1; index < this.lvResults.Items.Count && this.lvResults.Items[index].Bounds.Bottom < this.lvResults.Bounds.Bottom; ++index)
        ++num;
      if (this.lvResults.TopItem.Index + num * 2 < this.lvResults.Items.Count)
        return;
      this.requestingAddSongs = true;
      this.pbLoading.Visible = true;
      this.rbSearch.Enabled = this.rbMyAudio.Enabled = this.rbGroupPerson.Enabled = this.btnSearch.Enabled = this.tbSearch.Enabled = false;
      List<VkSong> newSongs = (List<VkSong>) null;
      int offset = this.RoundOffset(this.lvResults.Items.Count);
      ThreadWorker.PerformSimpleOperation((Form) this, (ThreadOperation) (() =>
      {
        if (this.rbSearch.Checked)
          newSongs = VkTools.GetSongsOffset(this.query, this.lastUrl, offset);
        if (this.rbMyAudio.Checked)
          newSongs = VkTools.GetMySongsRest();
        if (!this.rbGroupPerson.Checked)
          return;
        newSongs = VkTools.GetGroupPersonSongsRest(this.query);
      }), (Action) (() =>
      {
        if (newSongs.Count > 950)
          newSongs = Enumerable.ToList<VkSong>(Enumerable.Take<VkSong>((IEnumerable<VkSong>) newSongs, 950));
        if (newSongs != null)
        {
          this.lvResults.SuspendLayout();
          this.AddSongs(newSongs);
          this.lvResults.ResumeLayout(true);
        }
        if (newSongs == null || newSongs.Count == 0)
          this.doneScrolling = true;
        this.UpdateResultsStatus();
        this.pbLoading.Visible = false;
        this.requestingAddSongs = false;
        this.rbSearch.Enabled = this.rbMyAudio.Enabled = this.rbGroupPerson.Enabled = this.btnSearch.Enabled = true;
        this.tbSearch.Enabled = !this.rbMyAudio.Checked;
      }));
    }

    private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      SearchForm.CurrentForm = (SearchForm) null;
      if (this.previewState != PlayerState.Playing && this.previewState != PlayerState.Paused)
        return;
      this.StopPreviewPlayback(true);
    }

    public void NotifyPositionChanged(int Position)
    {
      lock (this.posChangeLocker)
      {
        if (this.lastSelectedProgress != -1 && Math.Abs(this.lastSelectedProgress - Position) > 1 || Position == this.tbPreviewProgress.Value - 1)
          return;
        this.tbPreviewProgress.Enabled = true;
        if (this.lastSelectedProgress > Position)
          Position = this.lastSelectedProgress;
        else
          this.lastSelectedProgress = -1;
        if (!this.gbPreview.Enabled || Position > this.tbPreviewProgress.Maximum || (Position <= -1 || Position == this.tbPreviewProgress.Value))
          return;
        this.posInternalChanging = true;
        this.tbPreviewProgress.Value = Position;
        this.SetPositionTitle(this.gbPreview.Tag as VkSong, Position);
        this.posInternalChanging = false;
      }
    }

    public void NotifyStateChanged(int State)
    {
      if (State != 2)
        return;
      if (this.previewState == PlayerState.Playing)
      {
        this.btnPlayPause.BackgroundImage = (Image) Resources.play;
        this.previewState = PlayerState.Paused;
      }
      else
      {
        this.btnPlayPause.BackgroundImage = (Image) Resources.pause;
        this.previewState = PlayerState.Playing;
      }
    }

    private string PositionToString(int pos)
    {
      int num1 = pos / 60;
      int num2 = pos - num1 * 60;
      return num1.ToString() + ":" + num2.ToString().PadLeft(2, '0');
    }

    private void btnPlayPause_Click(object sender, EventArgs e)
    {
      if (this.pbBassLoading.Visible)
        return;
      VkSong song = this.gbPreview.Tag as VkSong;
      if (this.previewState == PlayerState.NotSet || this.previewState == PlayerState.Stopped)
        this.StartSong(song);
      else if (this.previewState == PlayerState.Playing)
      {
        this.btnPlayPause.BackgroundImage = (Image) Resources.play;
        VkAudioClass.Instance.InvokeStateChangedEvent(2);
        this.previewState = PlayerState.Paused;
      }
      else
      {
        this.btnPlayPause.BackgroundImage = (Image) Resources.pause;
        VkAudioClass.Instance.InvokeStateChangedEvent(2);
        this.previewState = PlayerState.Playing;
      }
    }

    private void InitPreview(VkSong song, bool startPlaying)
    {
      if (this.previewState == PlayerState.Paused || this.previewState == PlayerState.Playing)
        this.StopPreviewPlayback(false);
      this.gbPreview.Tag = (object) song;
      this.tbPreviewProgress.Minimum = 0;
      this.tbPreviewProgress.Maximum = song.Duration;
      this.tbPreviewProgress.Value = 0;
      this.tbPreviewProgress.Enabled = false;
      this.gbPreview.Enabled = this.lPreviewHeader.Enabled = this.lPreviewTime.Enabled = true;
      if (startPlaying)
        this.StartSong(song);
      this.SetPositionTitle(song, 0);
    }

    private void StartSong(VkSong song)
    {
      this.lastSelectedProgress = -1;
      this.tbPreviewProgress.Enabled = false;
      VkAudioClass.Instance.InvokeLoadUrl(song.URL);
      this.btnPlayPause.BackgroundImage = (Image) Resources.pause;
      this.previewState = PlayerState.Playing;
      this.previewTimer.Start();
    }

    private void SetTimeTitle(int pos, int duration)
    {
      using (Graphics graphics = this.CreateGraphics())
      {
        string text = this.PositionToString(pos) + " / " + this.PositionToString(duration);
        this.lPreviewTime.Left = this.gbPreview.Width - (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewTime.Font).Width) - this.lPreviewHeader.Left;
        this.lPreviewTime.Text = text;
      }
    }

    private void SetPositionTitle(VkSong song, int pos)
    {
      using (Graphics graphics = this.CreateGraphics())
      {
        this.SetTimeTitle(pos, song.Duration);
        string text = "Превью | " + song.ToString();
        int num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewHeader.Font).Width);
        if (num + this.lPreviewHeader.Left >= this.lPreviewTime.Left - 10)
        {
          string str = song.ToString();
          for (; num + this.lPreviewHeader.Left >= this.lPreviewTime.Left - 10; num = (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewHeader.Font).Width))
          {
            str = str.Remove(str.Length - 1);
            text = "Превью | " + str + "...";
          }
        }
        this.lPreviewHeader.Text = text;
      }
    }

    private void ResetPreview()
    {
      this.gbPreview.Enabled = this.lPreviewTime.Enabled = this.lPreviewHeader.Enabled = false;
      this.gbPreview.Tag = (object) null;
      this.lPreviewHeader.Text = "Превью";
      this.btnPlayPause.BackgroundImage = (Image) Resources.play;
      using (Graphics graphics = this.CreateGraphics())
      {
        string text = "0:00 / 0:00";
        this.lPreviewTime.Left = this.gbPreview.Width - (int) Math.Ceiling((double) graphics.MeasureString(text, this.lPreviewTime.Font).Width) - this.lPreviewHeader.Left;
        this.lPreviewTime.Text = text;
      }
    }

    private void lvResults_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      ListViewHitTestInfo listViewHitTestInfo = this.lvResults.HitTest(e.Location);
      if (listViewHitTestInfo.Item == null)
        return;
      this.InitPreview(listViewHitTestInfo.Item.Tag as VkSong, true);
    }

    private void tsmiPreview_Click(object sender, EventArgs e)
    {
      VkSong song = Enumerable.FirstOrDefault<VkSong>(Enumerable.Select<ListViewItem, VkSong>(Enumerable.OfType<ListViewItem>((IEnumerable) this.lvResults.SelectedItems), (Func<ListViewItem, VkSong>) (lvi => lvi.Tag as VkSong)));
      if (song == null)
        return;
      this.InitPreview(song, true);
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
      if (this.previewState != PlayerState.Playing && this.previewState != PlayerState.Paused)
        return;
      this.StopPreviewPlayback(true);
    }

    public void StopPreviewPlayback(bool notifyClient)
    {
      this.previewTimer.Stop();
      VkSong song = this.gbPreview.Tag as VkSong;
      if (notifyClient)
        VkAudioClass.Instance.InvokeStateChangedEvent(0);
      this.btnPlayPause.BackgroundImage = (Image) Resources.play;
      this.previewState = PlayerState.Stopped;
      this.tbPreviewProgress.Value = 0;
      this.SetPositionTitle(song, this.tbPreviewProgress.Value);
    }

    private void tbPreviewProgress_ValueChanged(object sender, EventArgs e)
    {
      lock (this.posChangeLocker)
      {
        if (this.posInternalChanging || this.previewState != PlayerState.Playing && this.previewState != PlayerState.Paused)
          return;
        this.SetPositionTitle(this.gbPreview.Tag as VkSong, this.tbPreviewProgress.Value);
        Application.DoEvents();
        this.lastSelectedProgress = this.tbPreviewProgress.Value;
        VkAudioClass.Instance.InvokePositionChangedEvent(this.tbPreviewProgress.Value);
      }
    }

    private void tsmiCheckAll_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.lvResults.Items)
        listViewItem.Checked = true;
    }

    private void tsmiUncheckAll_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.lvResults.Items)
        listViewItem.Checked = false;
    }

    private void tsmiInvertCheck_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.lvResults.Items)
        listViewItem.Checked = !listViewItem.Checked;
    }

    private void cbPerformer_CheckedChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.query))
        return;
      this.tbSearch.Text = this.query;
      this.MakeSearch();
    }

    private void tsmiCheckSelected_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.lvResults.SelectedItems)
        listViewItem.Checked = true;
    }
  }
}
