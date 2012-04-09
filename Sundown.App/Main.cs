using System;
using System.IO;
using System.Runtime.InteropServices;
using NDesk.Options;
using Sundown;

namespace Sundown.App
{
	enum RendererType
	{
		Html,
		BBCode
	}

	class Options
	{
		public Options()
		{
			Renderer = RendererType.Html;
			Extensions = new MarkdownExtensions();
			HtmlRenderMode = new HtmlRenderMode();
			MaxNesting = 16;
		}

		public RendererType Renderer { get; set; }

		public MarkdownExtensions Extensions { get; protected set; }
		public HtmlRenderMode HtmlRenderMode { get; protected set; }
		public int MaxNesting { get; set; }
	}

	class MainClass
	{
		public static void Main(string[] args)
		{
			bool showHelp = false;
			Options options = new Options();

			OptionSet optionSet = new OptionSet()
				.Add("r|renderer=", (string renderer) => {
					switch (renderer) {
					case "html":
						options.Renderer = RendererType.Html;
						break;
					case "bb":
					case "bbcode":
						options.Renderer = RendererType.BBCode;
						break;
					default:
						Console.WriteLine("no such renderer");
						Environment.Exit(0);
						break;
					}
				})
				.Add("h|?|help", "show help",
					     (_) => showHelp = true)
				.Add("m|maxnesting=", "specify the maximum nesting level, default is 16, minimum is 1",
					     (int i) => options.MaxNesting = Math.Max(1, i))

					// Markdown Extensions
				.Add("autolink", "enables autolink extension",
					     (_) => options.Extensions.Autolink = true)
				.Add("tables", "enables table extension",
					     (_) => options.Extensions.Tables = true)
				.Add("fencedcode", "enables fenced code extension",
					     (_) => options.Extensions.FencedCode = true)
				.Add("strikethrough", "enables strikethrough extension",
					     (_) => options.Extensions.Strikethrough = true)
				.Add("htmlblocks", "enables html block extension",
					     (_) => options.Extensions.LaxHTMLBlocks = true)
				.Add("spaceheaders", "enables spaceheaders extension",
					     (_) => options.Extensions.SpaceHeaders = true)
				.Add("superscript", "enables superscript extension",
					     (_) => options.Extensions.SuperScript = true)

					// html renderer options
				.Add("skiphtml", "skips html in the html renderer",
					     (_) => options.HtmlRenderMode.SkipHtml = true)
				.Add("skipstyle", "skips style in the html renderer",
					     (_) => options.HtmlRenderMode.SkipStyle = true)
				.Add("skipimages", "skips images in the html renderer",
					     (_) => options.HtmlRenderMode.SkipImages = true)
				.Add("skiplinks", "skips links in the html renderer",
					     (_) => options.HtmlRenderMode.SkipLinks = true)
				.Add("skipexpandtabs", "doesn't expand tabs in the html renderer",
					     (_) => options.HtmlRenderMode.SkipExpandTabs = true)
				.Add("safelink", "uses the safe link mode html renderer, links are checked if they are save to use",
					     (_) => options.HtmlRenderMode.SafeLink = true)
				.Add("toc", "uses the toc rendering mode in the html renderer",
					     (_) => options.HtmlRenderMode.TOC = true)
				.Add("hardwrap", "uses hard wrapping in the html renderer",
					     (_) => options.HtmlRenderMode.HardWrap = true)
				.Add("xhtml", "uses xhtml in the html renderer",
					     (_) => options.HtmlRenderMode.UseXHTML = true)
				.Add("escape", "",
					     (_) => options.HtmlRenderMode.Escape = true)
					;

			var files = optionSet.Parse(args);
			if (showHelp) {
				Console.WriteLine("Usage: sundown [OPTIONS] file");
				Console.WriteLine();
				Console.WriteLine("Options:");
				optionSet.WriteOptionDescriptions(Console.Out);
				return;
			}
			if (files.Count < 1) {
				Console.WriteLine("You have to provide a filename as an argument");
			} else {
				Work(options, files[0]);
			}
		}

		static void Work(Options options, string inputfile)
		{
			Renderer renderer = null;
			switch (options.Renderer) {
			case RendererType.Html:
				renderer = new HtmlRenderer(options.HtmlRenderMode);
				break;
			case RendererType.BBCode:
				renderer = new BBCodeRenderer();
				break;
			}

			var md = new Markdown(renderer, options.Extensions, options.MaxNesting);

			using (Buffer buffer = new Buffer())
			try {
				using (var sr = new StreamReader(File.OpenRead(inputfile)))
				md.Render(buffer, sr.ReadToEnd());
			} catch (Exception exception) {
				Console.WriteLine("Unable to open input file {0: {1}", inputfile, exception.Message);
				return;
			} finally {
				Console.WriteLine(buffer);
			}
		}
	}
}
