using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTestingTool
{
	public static class FlowDiff
	{
		public static FlowChange FindChange(long num, string oldContent, string newContent)
		{
			// find position

			var oL = oldContent.Length;
			var nL = newContent.Length;

			int minL = oL > nL ? nL : oL;
			//int maxL = oL > nL ? oL : nL;


			int p1 = 0, p2 = 0;
			char c1, c2;

			int i = 0;
			for (; i < minL; i++)
			{
				c1 = oldContent[i];
				c2 = newContent[i];
				if (c1 != c2)
				{
					p1 = i;
					break;
				}
			}

			for (i = 1; i < minL; i++)
			{
				c1 = oldContent[oL - i];
				c2 = newContent[nL - i];
				if (c1 != c2)
				{
					p2 = oL - i;
					break;
				}
			}

			int d = p2 - p1;

			return new FlowChange()
			{
				N = num,
				P = p1,
				D = d,
				C = d > 0 ? newContent.Substring(p1, d) : ""
			};
		}

		public static string AcceptChange(string content, FlowChange change)
		{
			return
				content.Substring(0, change.P) +
				change.C +
				content.Substring(change.P + change.D);
		}
	}
}
