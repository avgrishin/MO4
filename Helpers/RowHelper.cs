using NPOI.SS.UserModel;

namespace MO.Helpers
{
  public static class RowHelper
  {
    public static ICell GetOrCreateCell(this IRow row, int column)
    {
      return row.GetCell(column) ?? row.CreateCell(column);
    }

    public static IRow GetOrCreateRow(this ISheet sheet, int row)
    {
      return sheet.GetRow(row) ?? sheet.CreateRow(row);
    }
  }
}

