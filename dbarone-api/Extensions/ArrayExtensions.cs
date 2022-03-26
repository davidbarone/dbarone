namespace dbarone_api.Extensions;
using System.Text;
using System.Collections;

public static class ArrayExtensions
{
    public class PrettyPrintColumn
    {
        public string PropertyName { get; set; }
        public string Title { get; set; }
        public int Width { get; set; }
        public Justification Justification { get; set; }
    }

    /// <summary>
    /// Pretty prints a single object or Enumeration of objects by reflecting through
    /// the properties. The pretty print based on 132 column display.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="columns"></param>
    /// <returns></returns>
    public static string PrettyPrint(this object source, IList<PrettyPrintColumn> columns = null)
    {
        Type t = source.GetType();
        StringBuilder sb = new StringBuilder();
        if (columns != null && columns.Count() > 0)
        {
            sb.AppendLine(string.Join(" ", columns.Select(c => c.Title.Justify(c.Width, c.Justification))));
            sb.AppendLine(string.Join(" ", columns.Select(c => "".PadRight(c.Width, '-'))));
        }

        var sourceAsEnumerable = source as IEnumerable;
        if (sourceAsEnumerable != null)
        {
            // IEnumerable - render table
            // Each row allows for text wrapping
            foreach (var item in sourceAsEnumerable)
            {
                var elementType = item.GetType();
                List<List<string>> rowValues = new List<List<string>>();
                foreach (var column in columns)
                {
                    List<string> cell = new List<string>();
                    rowValues.Add(cell);
                    // First split cells by new line
                    var value = (elementType.GetProperty(column.PropertyName).GetValue(item) ?? string.Empty).ToString();
                    var valueRows = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (var row in valueRows)
                    {
                        foreach (var wrappedLine in row.WordWrap(column.Width))
                        {
                            cell.Add(wrappedLine);
                        }
                    }
                }

                // At this point check the max height of the rowValues array.
                int maxHeight = rowValues.Max(r => r.Count());
                foreach (var column in columns)
                {
                    var i = columns.IndexOf(column);
                    var k = rowValues[i].Count();
                    for (int j = k; j < maxHeight; j++)
                        rowValues[i].Add(string.Empty);
                }

                // At this point, all rowValues have same dimensionality
                // Just pad values to cell widths
                foreach (var column in columns)
                {
                    var i = columns.IndexOf(column);
                    for (int j = 0; j < maxHeight; j++)
                        rowValues[i][j] = rowValues[i][j].Justify(column.Width, column.Justification);
                }

                for (int i = 0; i < maxHeight; i++)
                {
                    sb.AppendLine(string.Join(" ", columns.Select(c => rowValues[columns.IndexOf(c)][i])));
                }
            }
        }
        else
        {
            const int keyWidth = 30;
            const int valueWidth = 132 - 30 - 1;
            sb.AppendLine(string.Format("{0} {1}", "Name".Justify(keyWidth, Justification.LEFT), "Description".Justify(keyWidth, Justification.LEFT)));
            sb.AppendLine(string.Format("{0} {1}", "".PadRight(keyWidth, '-'), "".PadRight(valueWidth, '-')));

            // Single object - iterate properties
            Hashtable ht = new Hashtable();
            // loop through properties (excluding IEnumerables (which we can't really pretty print)
            foreach (var property in t.GetProperties().Where(p => !typeof(IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string)))
                ht[property.Name] = (property.GetValue(source) ?? string.Empty).ToString().WordWrap(valueWidth).ToList();

            foreach (var key in ht.Keys)
            {
                List<string> k = new List<string>();
                k.Add(key.ToString());
                var lines = (IList<string>)ht[key];
                for (int i = 1; i <= ((IList<string>)ht[key]).Count(); i++)
                    k.Add(string.Empty);

                for (int i = 0; i < ((IList<string>)ht[key]).Count(); i++)
                    sb.AppendLine(string.Format("{0} {1}", k[i].ToString().Justify(keyWidth, Justification.LEFT), ((IList<string>)ht[key])[i].Justify(valueWidth, Justification.LEFT)));
            }
        }
        // remove trailing newline
        var ret = sb.ToString();
        if (ret.Length > 0)
            ret = ret.Substring(0, ret.Length - Environment.NewLine.Length);
        return ret;
    }

    /// <summary>
    /// Splices an array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="start"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    public static T[] Splice<T>(this T[] source, long start, long? number = null)
    {
        var len = source.Length;
        var c = len - start;
        if (number.HasValue)
            c = number.Value;
        T[] result = new T[c];

        while (c > 0)
        {
            result[c - 1] = source[start + c - 1];
            c--;
        }
        return result;
    }
}