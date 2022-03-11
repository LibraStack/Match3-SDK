using System;

public readonly struct GridPosition : IEquatable<GridPosition>
{
    public int RowIndex { get; }

    public int ColumnIndex { get; }

    public GridPosition(int rowIndex, int columnIndex)
    {
        RowIndex = rowIndex;
        ColumnIndex = columnIndex;
    }

    public bool Equals(GridPosition other)
    {
        return RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;
    }

    public static GridPosition Up { get; } = new GridPosition(1, 0);
    public static GridPosition Down { get; } = new GridPosition(-1, 0);
    public static GridPosition Left { get; } = new GridPosition(0, -1);
    public static GridPosition Right { get; } = new GridPosition(0, 1);

    public static GridPosition operator +(GridPosition a, GridPosition b) =>
        new GridPosition(a.RowIndex + b.RowIndex, a.ColumnIndex + b.ColumnIndex);

    public static GridPosition operator -(GridPosition a, GridPosition b) =>
        new GridPosition(a.RowIndex - b.RowIndex, a.ColumnIndex - b.ColumnIndex);
}