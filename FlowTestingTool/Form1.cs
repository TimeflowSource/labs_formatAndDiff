using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using DiffPlex.Model;

namespace FlowTestingTool
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			this.SizeChanged += Form1_SizeChanged;
			AdjustWidth();

			D2 = textBox2;
			D3 = textBox3;
			L1 = label1;

			comboBox1.Items.AddRange(new object[] {
				new OnTextChangeUpdatingMode(textBox1, this),
				new OnTimerUpdatingMode(textBox1, this, "Every 1 second", 1000),
				new OnTimerUpdatingMode(textBox1, this, "Every 5 seconds", 5000),
				new OnTimerUpdatingMode(textBox1, this, "Every 15 seconds", 15000),
				new OnTimerUpdatingMode(textBox1, this, "Every 1 minute", 60000),
				new ManualUpdatingMode()
				});

			comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

			comboBox1.SelectedIndex = 0;
		}

		private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selected = comboBox1.SelectedItem;
			if (!(selected is IUpdatingMode)) return;
			IUpdatingMode updatingMode = (IUpdatingMode)selected;
			currentUpdatingMode?.OnDeselect();
			currentUpdatingMode = updatingMode;
			updatingMode?.OnSelect();
		}

		private IUpdatingMode currentUpdatingMode;

		private float ColumnWidth { get; set; }

		private void Form1_SizeChanged(object sender, EventArgs e)
		{
			AdjustWidth();
		}

		private void AdjustWidth()
		{
			ColumnWidth = this.Width / 3;

			textBox1.Width = Convert.ToInt32(ColumnWidth);
			textBox2.Width = Convert.ToInt32(ColumnWidth);
			textBox3.Width = Convert.ToInt32(ColumnWidth);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//comboBox1.SelectedItem

			//comboBox1.Items.Add()
		}

		public void Track()
		{
			ff?.Commit(textBox1.Text);
		}

		public static TextBox D2;
		public static TextBox D3;
		public static Label L1;

		public FlowFile ff = new FlowFile();

		private void button1_Click(object sender, EventArgs e)
		{
			Track();
		}
	}



	public class FlowTracker
	{
		
	}

	public class FlowFile
	{
		public string ID { get; private set; }

		// @TODO basic file info

		public string FullContent { get; private set; } = "";
		public string PatchedContent { get; private set; } = "";

		public readonly List<FlowMicroCommit> MicroPathes = new List<FlowMicroCommit>();
		public readonly List<FlowChange> Changes = new List<FlowChange>();

		public long LastN { get; private set; }

		public bool Commit(string newContent)
		{
			if (newContent == FullContent) return false;

			var change = FlowDiff.FindChange(LastN++, FullContent, newContent);
			Changes.Add(change);

			Form1.D2.Text = "<" + FlowDiff.AcceptChangeDebug(FullContent, change) + ">";
			PatchedContent = FlowDiff.AcceptChange(PatchedContent, change);
			Form1.D3.Text = change.ToString() + "\r\n" + 
				Form1.D3.Text.Substring(0, Math.Min(Form1.D3.Text.Length, 1000));

			Form1.L1.Text = '"' + newContent + '"' + "\r\n\r\n" + '"' + PatchedContent + '"';
			

			FullContent = newContent;
			return true;
		}

		public bool Commit2(string newContent)
		{
			//var mc = new FlowMicroCommit();

			var diffBuilder = new InlineDiffBuilder(new Differ());
			var diff = diffBuilder.BuildDiffModel(FullContent, newContent);

			Form1.D3.Clear();

			string log3 = "";

			string log2 = "";

			foreach (var dl in diff.Lines)
			{
				switch (dl.Type)
				{
					case ChangeType.Unchanged:
						{
							log2 += "[ ] " + dl.Text + "\r\n";
							break;
						}
					case ChangeType.Deleted: break;
					case ChangeType.Modified:
						{
							log2 += "[m] " + dl.Text + "\r\n";
							break;
						}
					case ChangeType.Inserted:
						{
							log2 += "[i] " + dl.Text + "\r\n";
							break;
						}
					case ChangeType.Imaginary:
						{
							log2 += "[g] " + dl.Text + "\r\n";
							break;
						}
				}
				//var mc = new FlowMicroCommit();
				//mc.N = LastN++;
				//mc.P = dl.Position.Value;
				//mc.C = dl.Text;

				log3 += string.Format("{0}| {1}, \"{2}\"\r\n", dl.Type, dl.Position, dl.Text);

				//dl.Type
			}

			Form1.D2.Text = log2;
			Form1.D3.Text = log3;

			LastN++;
			FullContent = newContent;
			return false;
		}
	}

	public struct FlowMicroCommit
	{
		public long N; // ordering number in file
		public int P; // position in file
		public string C; // content

		public string T; // type of commit
		public int L; // leghth or count depends on type
	}

	public struct FlowChange
	{
		public long N; // Ordering number of change
		public int P; // Position in file
		public int D; // Number of chars to delete after position
		public string C; // Content to insert in position

		#region debug info
		public int d_ncL;
		public int d_p2;
		public int d_i;
		#endregion

		public override string ToString()
		{
			return string.Format("[{0}, {1}, {2}, '{3}' | ncl:{4}, p2:{5}, i:{6}]", N, P, D, C, d_ncL, d_p2, d_i);
		}
	}
}
