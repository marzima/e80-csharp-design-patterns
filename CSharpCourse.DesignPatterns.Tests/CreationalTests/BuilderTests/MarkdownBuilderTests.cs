using System.Text;
using CSharpCourse.DesignPatterns.Assignments;
using CSharpCourse.DesignPatterns.Creational.Builder;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.BuilderTests;

public class MarkdownBuilderTests
{
    private const string FinalMarkdownString = """
                                               ### This is my header
                                               **bold text** *italic text*
                                               [Link](https://example.com)
                                               """;
    
    [Fact]
    public void VeryBad()
    {
        var markdown = "### This is my header";
        markdown += Environment.NewLine;
        markdown += "**bold text**";
        markdown += " *italic text*";
        markdown += Environment.NewLine;
        markdown += "[Link](https://example.com)";
        
        Assert.Equal(FinalMarkdownString, markdown);
    }
    
    [Fact]
    public void Bad()
    {
        var sb = new StringBuilder();
        sb.AppendLine("### This is my header");
        sb.Append("**bold text**");
        sb.AppendLine(" *italic text*");
        sb.Append("[Link](https://example.com)");
        var markdown = sb.ToString();
        
        Assert.Equal(FinalMarkdownString, markdown);
    }

    [Fact]
    public void RegularBuilder()
    {
        var builder = new MarkdownBuilder();
        builder.AddHeader(3, "This is my header");
        builder.AddBold("bold text");
        builder.AddText(" ");
        builder.AddItalic("italic text");
        builder.NewLine();
        builder.AddLink("Link", "https://example.com");
        
        var markdown = builder.ToString();
        
        Assert.Equal(FinalMarkdownString, markdown);
    }

    [Fact]
    public void FluentBuilder()
    {
        var markdown = new FluentMarkdownBuilder()
            .AddHeader(3, "This is my header")
            .AddBold("bold text")
            .AddText(" ")
            .AddItalic("italic text")
            .NewLine()
            .AddLink("Link", "https://example.com")
            .ToString();
        
        Assert.Equal(FinalMarkdownString, markdown);
    }
}
