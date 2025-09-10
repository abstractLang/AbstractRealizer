using System.Text;

namespace Abstract.Realizer;

internal static class StringExtensions
{
    public static string TabAllLines(this string? str)
    {
        if (str == null) return "<nil>";
        var sb = new StringBuilder();
        var lines = str.Split(Environment.NewLine);
        foreach (var l in lines) sb.AppendLine($"\t{l}");
        if (lines.Length > 0) sb.Length -= Environment.NewLine.Length;
        return sb.ToString();
    }
}
