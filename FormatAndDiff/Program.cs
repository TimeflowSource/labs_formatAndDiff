using System;
using DiffPlex.DiffBuilder.Model;
using DiffPlex.DiffBuilder;
using DiffPlex;
using DiffPlex.Model;

namespace FormatAndDiff
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var diffBuilder = new InlineDiffBuilder(new Differ());
			var before = "The text without changes\nThe text without changes\nThe text without changes\n";
			var after = "The text without changes\nThe text with change. A little one\nThe text without changes\n";
			var diff = diffBuilder.BuildDiffModel(before, after);
			_ = diff;
		}
	}
}
