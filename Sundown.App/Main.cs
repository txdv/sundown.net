using System;
using System.IO;
using System.Collections.Generic;
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

			MaxNesting = 16;
			Extensions = new MarkdownExtensions();

			HtmlRenderMode = new HtmlRenderMode();
			BBCodeOptions = new BBCodeOptions();
		}

		public RendererType Renderer { get; set; }

		public int MaxNesting { get; set; }
		public MarkdownExtensions Extensions { get; protected set; }

		public HtmlRenderMode HtmlRenderMode { get; protected set; }
		public BBCodeOptions BBCodeOptions { get; protected set; }
	}

	class MainClass
	{
		public static void Main(string[] args)
		{
			bool showHelp = false;
			bool showVersion = false;
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
				.Add("m|maxnesting=", "specify the maximum nesting level, default 16, minimum 1",
					     (int i) => options.MaxNesting = Math.Max(1, i))
				.Add("v|version", "show version",
					     (_) => showVersion = true)
					;

			OptionSet markdownExtensionOptionSet = new OptionSet()
				.Add("autolink", "enable autolinks",
					     (_) => options.Extensions.Autolink = true)
				.Add("tables", "enable tables",
					     (_) => options.Extensions.Tables = true)
				.Add("fencedcode", "enable fenced code",
					     (_) => options.Extensions.FencedCode = true)
				.Add("strikethrough", "enable strikethrough",
					     (_) => options.Extensions.Strikethrough = true)
				.Add("htmlblocks", "enable html blocks",
					     (_) => options.Extensions.LaxHTMLBlocks = true)
				.Add("spaceheaders", "enable spaceheaders",
					     (_) => options.Extensions.SpaceHeaders = true)
				.Add("superscript", "enable superscript",
					     (_) => options.Extensions.SuperScript = true)
					;

			OptionSet htmlRendererModeOptionSet = new OptionSet()
				.Add("skiphtml", "skip html",
					     (_) => options.HtmlRenderMode.SkipHtml = true)
				.Add("skipstyle", "skip styles",
					     (_) => options.HtmlRenderMode.SkipStyle = true)
				.Add("skipimages", "skip images",
					     (_) => options.HtmlRenderMode.SkipImages = true)
				.Add("skiplinks", "skip links",
					     (_) => options.HtmlRenderMode.SkipLinks = true)
				.Add("skipexpandtabs", "don't expand tabs",
					     (_) => options.HtmlRenderMode.SkipExpandTabs = true)
				.Add("safelink", "check links if they are safe to use",
					     (_) => options.HtmlRenderMode.SafeLink = true)
				.Add("toc", "use the toc rendering mode",
					     (_) => options.HtmlRenderMode.TOC = true)
				.Add("hardwrap", "use hard wrapping",
					     (_) => options.HtmlRenderMode.HardWrap = true)
				.Add("xhtml", "generate xhtml",
					     (_) => options.HtmlRenderMode.UseXHTML = true)
				.Add("escape", "",
					     (_) => options.HtmlRenderMode.Escape = true)
					;

			OptionSet bbRendererOptionSet = new OptionSet()
				.Add("defaultheadersize=", "sets the default header size",
				     (int size) => options.BBCodeOptions.DefaultHeaderSize = size)
				.Add("headersizes=", "sets the header sizes starting with level 1 and increasing, " +
					"\"20,15,10\" will set the level 1 headers to the size 20, level 2 to 15, 3 to 10 " +
					"and the rest to defaultheadersize.",
				     (string sizes) => {
						var arr = sizes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
						options.BBCodeOptions.HeaderSizes = new Dictionary<int, int>();
						for (int i = 0; i < arr.Length; i++) {
							options.BBCodeOptions.HeaderSizes[i + 1] = int.Parse(arr[i]);
						}
					})
					;

			var files = markdownExtensionOptionSet.Parse(optionSet.Parse(args));
			if (showHelp) {
				Console.WriteLine("Usage: sundown [OPTIONS] file");
				Console.WriteLine();
				Console.WriteLine("Options:");
				optionSet.WriteOptionDescriptions(Console.Out);
				Console.WriteLine("\nMarkdown extension options:");
				markdownExtensionOptionSet.WriteOptionDescriptions(Console.Out);
				Console.WriteLine("\nHtml renderer options");
				htmlRendererModeOptionSet.WriteOptionDescriptions(Console.Out);
				Console.WriteLine ("\nBBCode renderer options");
				bbRendererOptionSet.WriteOptionDescriptions(Console.Out);
				return;
			} else if (showVersion) {
				Console.WriteLine("sundown.net 0.1, sundown {0}", Markdown.Version);
				return;
			}

			if (options.Renderer == RendererType.Html) {
				files = htmlRendererModeOptionSet.Parse(files);
			} else {
				files = bbRendererOptionSet.Parse(files);
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
				renderer = new BBCodeRenderer(options.BBCodeOptions);
				break;
			}

			var md = new Markdown(renderer, options.Extensions, options.MaxNesting);

			using (Buffer buffer = new Buffer())
			try {
				using (var sr = new StreamReader(File.OpenRead(inputfile)))
				md.Render(buffer, sr.ReadToEnd());
			} catch (Exception exception) {
				Console.WriteLine("Unable to open input file {0}: {1}", inputfile, exception.Message);
				return;
			} finally {
				Console.WriteLine(buffer);
			}
		}
	}
}
