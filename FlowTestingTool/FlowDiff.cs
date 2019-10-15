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


			int p = oL > 0 ? oL : 0;
			int ncL = 0, d = 0, p2 = 0;
			char c1, c2;

			int i = 0;
			for (; i < minL; i++)
			{
				c1 = oldContent[i];
				c2 = newContent[i];
				if (c1 != c2)
				{
					p = i;
					break;
				}
			}

			//oL--;
			//nL--;

			for (i = 0; i < minL - p; i++)
			{
				c1 = oldContent[oL - i - 1];
				c2 = newContent[nL - i - 1];
				if (c1 != c2)
				{
					break;
				}
			}
			p2 = oL + 1 - i;
			d = oL - i - p;
			ncL = nL - i - p;

			if (ncL < 0)
			{
				d -= ncL;
				p += ncL;
			}

			return new FlowChange()
			{
				N = num,
				P = p,
				D = d,
				C = ncL > 0 ? newContent.Substring(p, ncL) : "",

				d_p2 = p2,
				d_ncL = ncL,
				d_i = i,
			};
		}

		public static string AcceptChange(string content, FlowChange change)
		{
			return
				content.Substring(0, change.P) +
				change.C +
				content.Substring(change.P + change.D);
		}

		public static string AcceptChangeDebug(string content, FlowChange change)
		{
			string s1 = content.Substring(0, change.P);
			string s2 = "{" + change.C + "}";
			string s3 = content.Substring(change.P + change.D);
			return s1 + s2 + s3;
		}
	}
}
