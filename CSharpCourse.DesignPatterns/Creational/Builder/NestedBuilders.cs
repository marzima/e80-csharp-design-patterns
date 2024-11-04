using System.Text;

namespace CSharpCourse.DesignPatterns.Creational.Builder;

internal class NestedBuilders
{
    private readonly StringBuilder _builder = new();

    public NestedBuilders AddText(string text)
    {
        _builder.Append(text);
        return this;
    }

    public NestedBuilders AddHeader(int headerLevel, string text)
    {
        var prefix = new string(Enumerable.Repeat('#', headerLevel).ToArray());
        _builder.AppendLine($"{prefix} {text}");
        return this;
    }

    public NestedBuilders AddBold(string text)
    {
        _builder.Append($"**{text}**");
        return this;
    }

    public NestedBuilders AddItalic(string text)
    {
        _builder.Append($"*{text}*");
        return this;
    }

    public NestedBuilders AddLink(string name, string url)
    {
        _builder.Append($"[{name}]({url})");
        return this;
    }

    public NestedBuilders NewLine()
    {
        _builder.AppendLine();
        return this;
    }

    public override string ToString()
        => _builder.ToString();
}
