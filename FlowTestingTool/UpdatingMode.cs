using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowTestingTool
{

	public interface IUpdatingMode
	{
		void OnSelect();
		void OnDeselect();
	}

	public class OnTextChangeUpdatingMode : IUpdatingMode
	{
		public TextBox TextBox { get; set; }
		public Form1 Form { get; set; }

		public OnTextChangeUpdatingMode(TextBox textBox, Form1 form)
		{
			this.TextBox = textBox;
			this.Form = form;
		}

		public override string ToString()
		{
			return "By OnTextChanged event";
		}

		public void OnSelect()
		{
			TextBox.TextChanged += TextBox_TextChanged;
		}

		public void OnDeselect()
		{
			TextBox.TextChanged -= TextBox_TextChanged;
		}

		private void TextBox_TextChanged(object sender, EventArgs e)
		{
			Form?.Track();
		}
	}

	public class OnTimerUpdatingMode : IUpdatingMode
	{

		public TextBox TextBox { get; set; }
		public Form1 Form { get; set; }
		public string Caption { get; set; }

		public OnTimerUpdatingMode(TextBox textBox, Form1 form, string caption, int timerInterval)
		{
			this.TextBox = textBox;
			this.Form = form;
			this.Caption = caption;
			Timer.Interval = timerInterval;
		}
		public override string ToString()
		{
			return Caption;
		}

		private readonly Timer Timer = new Timer();

		public void OnSelect()
		{
			Timer.Tick += Timer_Tick;
			Timer.Start();
		}

		public void OnDeselect()
		{
			Timer.Stop();
			Timer.Tick -= Timer_Tick;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			Form?.Track();
		}
	}

	public class ManualUpdatingMode : IUpdatingMode
	{
		public ManualUpdatingMode()
		{
		}

		public override string ToString()
		{
			return "Manual";
		}

		public void OnSelect()
		{
		}

		public void OnDeselect()
		{
		}

	}
}
