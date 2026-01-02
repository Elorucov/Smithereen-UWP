namespace SmithereenUWP.DataModels
{
    internal class Tile
    {
        public int colSpan, rowSpan, startCol, startRow, columnCount, rowCount;
        public int width;

        public Tile(int colSpan, int rowSpan, int startCol, int startRow)
        {
            this.colSpan = colSpan;
            this.rowSpan = rowSpan;
            this.startCol = startCol;
            this.startRow = startRow;
        }

        public Tile(int colSpan, int rowSpan, int startCol, int startRow, int width) : this(colSpan, rowSpan, startCol, startRow)
        {
            this.width = width;
        }
    }

    internal class TiledLayoutResult
    {
        public int[] columnSizes, rowSizes; // sizes in grid fractions
        public Tile[] tiles;
        public int width, height; // in pixels
    }
}
