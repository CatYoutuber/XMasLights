using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Security.Principal;
using System.Collections.Generic;
using Microsoft.Win32;
using XMasLights.PlugInSystem;
using XMasLights.PlugInSystem.Default;
using IniLib;

namespace XMasLights
{
	public partial class MainForm : Form
	{
		public static int 
			nLights = 0,
			distance = 96,
			diameter = 12,
			offset = 0,
			libIndex = 0,
			effectIndex = 0;
		Color[] colors;
		int[] heightMap;
		Pen wirePen = new Pen(Color.DarkGray, 2f);
		Pen lightStrokePen = new Pen(Color.FromArgb(96, 96, 96), 1f);
		ContextMenuStrip menu;
		Random random = new Random(Environment.TickCount);
		public Point mouseDrag;
		public static bool canDrag = false, dragging = false;
		public static List<IEffectCollection> plugins = new List<IEffectCollection>();
		public static List<string> dllNames = new List<string>();
		public static readonly string pluginsDir = Environment.CurrentDirectory + "\\plugins";
		public static IniFile prefs = new IniFile(Environment.CurrentDirectory + "\\preferences.ini");
		public bool IsAdmin { get { return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator); } }
		public MainForm()
		{
			InitializeComponent();
			plugins.Add(new DefaultEffects());
			dllNames.Add(Assembly.GetExecutingAssembly().Location);
			ComputeLightsCount();
			LoadTypes();
			TransparencyKey = BackColor;

			#region Preferences
			prefs.CreateIfNotExists("distance", "common", "96");
			prefs.CreateIfNotExists("diameter", "common", "12");
			prefs.CreateIfNotExists("offset", "common", "0");
			prefs.CreateIfNotExists("libIx", "effect", "0");
			prefs.CreateIfNotExists("effectIx", "effect", "0");

			distance = int.Parse(prefs.Read("distance","common","96"));
			diameter = int.Parse(prefs.Read("diameter","common","12"));
			offset = int.Parse(prefs.Read("offset","common","0"));
			libIndex = int.Parse(prefs.Read("libIx","effect","0"));
			effectIndex = int.Parse(prefs.Read("effectIx","effect","0"));
            #endregion
			#region Menu
			menu = new ContextMenuStrip();
			//MenuItem file = new MenuItem("File");
			void SaveSettings()
            {
				prefs.Write("distance", distance.ToString(), "common");
				prefs.Write("diameter", diameter.ToString(), "common");
				prefs.Write("offset", offset.ToString(), "common");
            }
			ToolStripMenuItem windowStateItem = new ToolStripMenuItem("Window State",Properties.Resources.window_mode);
			void UpdateWindowState(object s, FormWindowState state)
			{
				foreach (ToolStripMenuItem i in windowStateItem.DropDownItems) i.Checked = i == (ToolStripMenuItem)s;
				WindowState = state;
				ComputeLightsCount();
			}
			windowStateItem.DropDownItems.AddRange(new ToolStripMenuItem[] {
				new ToolStripMenuItem("Maximized",null,(s,e) => { UpdateWindowState(s, FormWindowState.Maximized); }){ Checked = true },
				new ToolStripMenuItem("Normal",null,   (s,e) => { UpdateWindowState(s, FormWindowState.Normal   ); }){},
				new ToolStripMenuItem("Minimized",null,(s,e) => { UpdateWindowState(s, FormWindowState.Minimized); }){},
			});

			ToolStripMenuItem distanceItem = new ToolStripMenuItem("Distance",Properties.Resources.distance);
			void UpdateDistance(object s, int d)
			{
				foreach (ToolStripMenuItem i in distanceItem.DropDownItems) i.Checked = i == (ToolStripMenuItem)s;
				distance = d;
				ComputeLightsCount();
				SaveSettings();
			}
			distanceItem.DropDownItems.AddRange(new ToolStripMenuItem[] {
				new ToolStripMenuItem("96",null,(s,e) => { UpdateDistance(s, 96); }){ Tag = 96 },
				new ToolStripMenuItem("80",null,(s,e) => { UpdateDistance(s, 80); }){ Tag = 80 },
				new ToolStripMenuItem("64",null,(s,e) => { UpdateDistance(s, 64); }){ Tag = 64 },
				new ToolStripMenuItem("48",null,(s,e) => { UpdateDistance(s, 48); }){ Tag = 48 },
				new ToolStripMenuItem("32",null,(s,e) => { UpdateDistance(s, 32); }){ Tag = 32 },
			});

			ToolStripMenuItem diameterItem = new ToolStripMenuItem("Diameter",Properties.Resources.diameter);
			void UpdateDiameter(object s, int d)
			{
				foreach (ToolStripMenuItem i in diameterItem.DropDownItems) i.Checked = i == (ToolStripMenuItem)s;
				diameter = d;
				ComputeLightsCount();
				SaveSettings();
			}
			diameterItem.DropDownItems.AddRange(new ToolStripMenuItem[] {
				new ToolStripMenuItem("16",null,(s,e) => { UpdateDiameter(s, 16); }){ Tag = 16 },
				new ToolStripMenuItem("14",null,(s,e) => { UpdateDiameter(s, 14); }){ Tag = 14 },
				new ToolStripMenuItem("12",null,(s,e) => { UpdateDiameter(s, 12); }){ Tag = 12 },
				new ToolStripMenuItem("10",null,(s,e) => { UpdateDiameter(s, 10); }){ Tag = 10 },
				new ToolStripMenuItem("8", null,(s,e) => { UpdateDiameter(s, 08); }){ Tag = 08 },
			});

			ToolStripMenuItem offsetItem = new ToolStripMenuItem("Offset",Properties.Resources.offset);
			void UpdateOffset(object s, int o)
			{
				foreach (ToolStripMenuItem i in offsetItem.DropDownItems) i.Checked = i == (ToolStripMenuItem)s;
				offset = o;
				ComputeLightsCount();
				SaveSettings();
			}
			offsetItem.DropDownItems.AddRange(new ToolStripMenuItem[] {
				new ToolStripMenuItem("64",null,(s,e) => { UpdateOffset(s, 64); }){ Tag = 64 },
				new ToolStripMenuItem("48",null,(s,e) => { UpdateOffset(s, 48); }){ Tag = 48 },
				new ToolStripMenuItem("44 (TaskBar)",null,(s,e) => { UpdateOffset(s, 44); }){ Tag = 44 },
				new ToolStripMenuItem("32",null,(s,e) => { UpdateOffset(s, 32); }){ Tag = 32 },
				new ToolStripMenuItem("24",null,(s,e) => { UpdateOffset(s, 24); }){ Tag = 24 },
				new ToolStripMenuItem("16",null,(s,e) => { UpdateOffset(s, 16); }){ Tag = 16 },
				new ToolStripMenuItem("0", null,(s,e) => { UpdateOffset(s, 00); }){ Tag = 00 },
			});
			void UpdateDragging(object s)
			{
				ToolStripMenuItem i = (ToolStripMenuItem)s;
				i.Checked = !i.Checked;
				canDrag = i.Checked;
			}
			ToolStripMenuItem autorunItem = new ToolStripMenuItem("Autorun",null, (s,e) => {
				ToolStripMenuItem i = (ToolStripMenuItem)s;
				i.Checked = !i.Checked;
				EnableAutorun(!IsAutorunEnabled());
			}) { Checked = IsAutorunEnabled()};
			/*file*/
			menu.Items.AddRange(new ToolStripItem[] {
				distanceItem,
				diameterItem,
				offsetItem,
				windowStateItem,
				new ToolStripSeparator(),
				new ToolStripMenuItem("Dragging",null,(s,e) => UpdateDragging(s)),
				new ToolStripMenuItem("Effect Manager",null, (s,e) => new Thread(() => Application.Run(new EffectManagerDialog())).Start()),
				new ToolStripMenuItem("Regenerate Heightmap",null, (s,e) => ComputeLightsCount()),
				new ToolStripSeparator(),
				autorunItem,
				new ToolStripMenuItem("Exit",null, (sender, e) => { Application.Exit(); }) { ShowShortcutKeys = true, ShortcutKeys = Keys.Alt | Keys.F4},
				new ToolStripMenuItem("About",null, (s,e) => new Thread(() => Application.Run(new AboutBox())).Start()) { ShowShortcutKeys = true, ShortcutKeys = Keys.F1 }
			});
			menu.RenderMode = ToolStripRenderMode.Professional;
			ContextMenuStrip = menu;
			#endregion

			foreach (ToolStripMenuItem mi in distanceItem.DropDownItems) if ((int)mi.Tag == distance) mi.Checked = true;
			foreach (ToolStripMenuItem mi in diameterItem.DropDownItems) if ((int)mi.Tag == diameter) mi.Checked = true;
			foreach (ToolStripMenuItem mi in offsetItem.DropDownItems) if ((int)mi.Tag == offset) mi.Checked = true;

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
		private bool IsAutorunEnabled()
        {
			RegistryKey run = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
			bool autorun = run.GetValue("XMasLights") != null;
			run.Close();
			return autorun;
        }
		private void EnableAutorun(bool enable)
        {
			RegistryKey run = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run",true);
			if (enable)
				run.SetValue("XMasLights", Assembly.GetExecutingAssembly().Location);
			else
				run.DeleteValue("XMasLights");
        }
        #region Form Events
        protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ComputeLightsCount();
		}
		private void LoadEffectDefaults()
        {
			prefs.Write("libIx", (libIndex = 0).ToString(), "effect");
			prefs.Write("effectIx", (effectIndex = 0).ToString(), "effect");
        }
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
            #region IndexCheck
            if (libIndex > plugins.Count - 1) 
				if(MessageBox.Show("No Plugin with index " + libIndex + " found!\nLoading defaults!","XMasLights",MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
					LoadEffectDefaults();
			if (effectIndex > plugins[libIndex].GetEffects().Length - 1)
				if (MessageBox.Show(
					"No Effect with index " + effectIndex + " found in plugin \"" + plugins[libIndex].GetPlugInName() + "\"!\nLoading defaults!",
					"XMasLights", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
					LoadEffectDefaults();
            #endregion

			Graphics g = e.Graphics;
            plugins[libIndex].GetEffects()[effectIndex].FillColors(colors,nLights);
			for (int i = 0; i < nLights; i++)
			{
				int cx = i * distance + distance / 2,
					cy = heightMap[i] + offset;
				g.DrawCurve(wirePen, new Point[] {
					new Point(i * distance, offset),
					new Point(cx, cy),
					new Point(i * distance + distance, offset)});
				Rectangle lightBounds = new Rectangle(
					cx - diameter / 2,
					cy - diameter / 2,
					diameter, diameter);
				//g.FillEllipse(new SolidBrush(colors[i]), lightBounds);
				GraphicsPath path = new GraphicsPath();
				path.AddEllipse(lightBounds);
				PathGradientBrush brush = new PathGradientBrush(path) { 
					CenterPoint = new Point(cx, cy),
					CenterColor = colors[i],
					SurroundColors = new [] { Color.FromArgb((int)(colors[i].R / 1.75), (int)(colors[i].G / 1.75), (int)(colors[i].B / 1.75))}
				};
				g.FillPath(brush, path);
				path.Dispose();
				brush.Dispose();
				g.DrawEllipse(lightStrokePen, lightBounds);
			}
			Thread.Sleep(1000 / plugins[libIndex].GetEffects()[effectIndex].GetRequiredFrameRate());
			Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if(canDrag && e.Button == MouseButtons.Left)
			{
				dragging = true;
				mouseDrag = new Point(e.X, e.Y);
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(e.Button == MouseButtons.Left)
				dragging = false;
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(canDrag && dragging)
				this.Location = new Point(
					Location.X + (e.X - mouseDrag.X),
					Location.Y + (e.Y - mouseDrag.Y));
		}
		#endregion
		private void ComputeLightsCount()
		{
			nLights = Width / distance;
			colors = new Color[nLights];
			heightMap = new int[nLights];
			for (int i = 0; i < nLights; i++)
				heightMap[i] = random.Next(18, (int)(distance / 1.5f));
			MaximumSize = new Size(32767,offset + distance + diameter);
			Height = offset + distance + diameter;
			ShowInTaskbar = WindowState == FormWindowState.Minimized;
		}
		public void LoadTypes()
		{
			DirectoryInfo pluginDir = new DirectoryInfo(pluginsDir);
			if (!pluginDir.Exists) pluginDir.Create();
			foreach (string file in Directory.GetFiles(pluginDir.FullName, "*.dll"))
			{
				Assembly assembly = Assembly.LoadFrom(file);
				Type[] types = null;
				try {
					types = assembly.GetTypes();
                } catch (Exception ex) { MessageBox.Show(ex.Message + "\n" + ex.StackTrace); }
				foreach (Type type in types)
					if (type.GetInterfaces().Contains(typeof(IEffectCollection)))
					{
						plugins.Add((IEffectCollection)assembly.CreateInstance(type.FullName));
						dllNames.Add(file);
					}
			}
		}
		public static string GetFileName(string path) => path.Substring(path.LastIndexOf('\\') + 1);
	}
}
