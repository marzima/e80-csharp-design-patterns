using System;
using System.Text;

namespace CSharpCourse.DesignPatterns.Assignments;

public interface IMarkdownBuilder
{
    IMarkdownBuilder AddText(string text);
    IMarkdownBuilder AddHeader(int headerLevel, string text);
    IMarkdownBuilder AddBold(string text);
    IMarkdownBuilder AddItalic(string text);
    IMarkdownBuilder AddLink(string linkText, string url);
    IMarkdownBuilder AddLink(Action<IMarkdownBuilder> configureLinkText, string url);
    IMarkdownBuilder NewLine();
    string ToString();
}

public interface ITableBuilder
{
    ITableBuilder AddRow(Action<IRowBuilder> configureRow);
}

public interface IRowBuilder
{
    IRowBuilder AddCell(Action<IMarkdownBuilder> configureCell);
}

internal abstract class MarkdownElement : IMarkdownBuilder
{
    protected readonly StringBuilder _builder = new();

    public IMarkdownBuilder AddText(string text)
    {
        _builder.Append(text);
        return this;
    }

    public IMarkdownBuilder AddHeader(int headerLevel, string text)
    {
        var prefix = new string('#', headerLevel);
        _builder.AppendLine($"{prefix} {text}");
        return this;
    }

    public IMarkdownBuilder AddBold(string text)
    {
        _builder.Append($"**{text}**");
        return this;
    }

    public IMarkdownBuilder AddItalic(string text)
    {
        _builder.Append($"*{text}*");
        return this;
    }
    public IMarkdownBuilder AddLink(string linkText, string url)
    {
        _builder.Append($"[{linkText}]({url})");
        return this;
    }

    public IMarkdownBuilder AddLink(Action<IMarkdownBuilder> configureLinkText, string url)
    {
        _builder.Append("[");
        var linkBuilder = new FluentMarkdownBuilder();
        configureLinkText(linkBuilder);
        _builder.Append(linkBuilder);
        _builder.Append($"]({url})");
        return this;
    }

    public IMarkdownBuilder NewLine()
    {
        _builder.AppendLine();
        return this;
    }

    public override string ToString() => _builder.ToString();
}

internal class FluentMarkdownBuilder : MarkdownElement
{
    public FluentMarkdownBuilder AddTable(string[] headers, string[][] rows)
    {
        _builder.Append("|");
        foreach (var header in headers)
        {
            _builder.Append(header).Append("|");
        }
        _builder.AppendLine();

        _builder.Append("|");
        foreach (var _ in headers)
        {
            _builder.Append("---|");
        }
        _builder.AppendLine();

        foreach (var row in rows)
        {
            _builder.Append("|");
            foreach (var cell in row)
            {
                _builder.Append(cell).Append("|");
            }
            _builder.AppendLine();
        }

        return this;
    }

    public FluentMarkdownBuilder AddTable(string[] headers, Action<ITableBuilder> configureTable)
    {
        var tableBuilder = new TableBuilder(headers);
        configureTable(tableBuilder);
        _builder.Append(tableBuilder);
        return this;
    }
    public FluentMarkdownBuilder AddLink(string name, string url)
    {
        _builder.Append($"[{name}]({url})");
        return this;
    }
}

internal class TableBuilder : ITableBuilder
{
    private readonly StringBuilder _tableBuilder = new();

    public TableBuilder(string[] headers)
    {
        _tableBuilder.Append("|");
        foreach (var header in headers)
        {
            _tableBuilder.Append(header).Append("|");
        }
        _tableBuilder.AppendLine();

        _tableBuilder.Append("|");
        foreach (var _ in headers)
        {
            _tableBuilder.Append("---|");
        }
        _tableBuilder.AppendLine();
    }

    public ITableBuilder AddRow(Action<IRowBuilder> configureRow)
    {
        var rowBuilder = new RowBuilder();
        configureRow(rowBuilder);
        _tableBuilder.Append(rowBuilder);
        return this;
    }

    public override string ToString() => _tableBuilder.ToString();
}

internal class RowBuilder : IRowBuilder
{
    private readonly StringBuilder _rowBuilder = new();

    public RowBuilder()
    {
        _rowBuilder.Append("|");
    }

    public IRowBuilder AddCell(Action<IMarkdownBuilder> configureCell)
    {
        var cellBuilder = new FluentMarkdownBuilder();
        configureCell(cellBuilder);
        _rowBuilder.Append(cellBuilder).Append("|");
        return this;
    }

    public override string ToString()
    {
        _rowBuilder.AppendLine();
        return _rowBuilder.ToString();
    }
}