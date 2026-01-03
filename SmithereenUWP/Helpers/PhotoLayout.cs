using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

// https://github.com/grishka/Smithereen/blob/dev/src/main/java/smithereen/templates/MediaLayoutHelper.java

namespace SmithereenUWP.Helpers
{
    internal static class PhotoLayout
    {
        public const int MAX_WIDTH = 1000;
        public const int MAX_HEIGHT = 1777; // 9:16
        public const int MIN_HEIGHT = 475; // ~2:1
        public const double GAP = 2;

        public static TiledLayoutResult MakeLayout(List<ISizedAttachment> thumbs)
        {
            double maxRatio = MAX_WIDTH / (double)MAX_HEIGHT;

            TiledLayoutResult result = new TiledLayoutResult();
            if (thumbs.Count == 1)
            {
                ISizedAttachment att = thumbs[0];
                result.rowSizes = result.columnSizes = new int[] { 1 };
                double ratio = att.Width / (double)att.Height;
                if (ratio > maxRatio)
                {
                    result.width = MAX_WIDTH;
                    result.height = (int)Math.Max(MIN_HEIGHT, Math.Round(att.Height / (double)att.Width * MAX_WIDTH));
                }
                else
                {
                    result.height = MAX_HEIGHT;
                    result.width = MAX_WIDTH;
                }
                result.tiles = new Tile[] { new Tile(1, 1, 0, 0) };
                result.tiles[0].columnCount = result.tiles[0].rowCount = 1;
                return result;
            }
            else if (thumbs.Count == 0)
            {
                throw new ArgumentException("Empty thumbs array");
            }

            List<double> ratios = new List<double>();
            int cnt = thumbs.Count;

            bool allAreWide = true, allAreSquare = true;

            foreach (ISizedAttachment thumb in thumbs)
            {
                double ratio = Math.Max(0.45f, thumb.Width / (double)thumb.Height);
                if (ratio <= 1.2f)
                {
                    allAreWide = false;
                    if (ratio < 0.8f)
                        allAreSquare = false;
                }
                else
                {
                    allAreSquare = false;
                }
                ratios.Add(ratio);
            }

            double avgRatio = ratios.Count > 0 ? ratios.Sum() / ratios.Count : 1.0;

            if (cnt == 2)
            {
                if (allAreWide && avgRatio > 1.4 * maxRatio && Math.Abs(ratios[1] - ratios[0]) < 0.2)
                {
                    // two wide photos, one above the other
                    double h = Math.Max(Math.Min(MAX_WIDTH / ratios[0], Math.Min(MAX_WIDTH / ratios[1], (MAX_HEIGHT - GAP) / 2.0)), MIN_HEIGHT / 2);

                    result.width = MAX_WIDTH;
                    result.height = (int)Math.Round(h * 2 + GAP);
                    result.columnSizes = new int[] { result.width };
                    result.rowSizes = new int[] { (int)Math.Round(h), (int)Math.Round(h) };
                    result.tiles = new Tile[]{
                        new Tile(1, 1, 0, 0),
                        new Tile(1, 1, 0, 1)
                    };
                }
                else if (allAreWide)
                {
                    // two wide photos, one above the other, different ratios
                    result.width = MAX_WIDTH;
                    double h0 = MAX_WIDTH / ratios[0];
                    double h1 = MAX_WIDTH / ratios[1];
                    if (h0 + h1 < MIN_HEIGHT)
                    {
                        double prevTotalHeight = h0 + h1;
                        h0 = MIN_HEIGHT * (h0 / prevTotalHeight);
                        h1 = MIN_HEIGHT * (h1 / prevTotalHeight);
                    }
                    result.height = (int)Math.Round(h0 + h1 + GAP);
                    result.rowSizes = new int[] { (int)Math.Round(h0), (int)Math.Round(h1) };
                    result.columnSizes = new int[] { MAX_WIDTH };
                    result.tiles = new Tile[]{
                        new Tile(1, 1, 0, 0),
                        new Tile(1, 1, 0, 1)
                    };
                }
                else if (allAreSquare)
                {
                    // next to each other, same ratio
                    double w = (MAX_WIDTH - GAP) / 2;
                    double h = Math.Max(Math.Min(w / ratios[0], Math.Min(w / ratios[1], MAX_HEIGHT)), MIN_HEIGHT);

                    result.width = MAX_WIDTH;
                    result.height = (int)Math.Round(h);
                    result.columnSizes = new int[] { (int)Math.Round(w), (int)(MAX_WIDTH - Math.Round(w)) };
                    result.rowSizes = new int[] { (int)Math.Round(h) };
                    result.tiles = new Tile[]{
                        new Tile(1, 1, 0, 0),
                        new Tile(1, 1, 1, 0)
                    };
                }
                else
                {
                    // next to each other, different ratios
                    double w0 = ((MAX_WIDTH - GAP) / ratios[1] / (1 / ratios[0] + 1 / ratios[1]));
                    double w1 = (MAX_WIDTH - w0 - GAP);
                    double h = Math.Max(Math.Min(MAX_HEIGHT, Math.Min(w0 / ratios[0], w1 / ratios[1])), MIN_HEIGHT);

                    result.columnSizes = new int[] { (int)Math.Round(w0), (int)Math.Round(w1) };
                    result.rowSizes = new int[] { (int)Math.Round(h) };
                    result.width = (int)Math.Round(w0 + w1 + GAP);
                    result.height = (int)Math.Round(h);
                    result.tiles = new Tile[]{
                        new Tile(1, 1, 0, 0),
                        new Tile(1, 1, 1, 0)
                    };
                }
            }
            else if (cnt == 3)
            {
                if ((ratios[0] > 1.2 * maxRatio || avgRatio > 1.5 * maxRatio) || allAreWide)
                {
                    // 2nd and 3rd photos are on the next line
                    double hCover = Math.Min(MAX_WIDTH / ratios[0], (MAX_HEIGHT - GAP) * 0.66);
                    double w2 = ((MAX_WIDTH - GAP) / 2);
                    double h = Math.Min(MAX_HEIGHT - hCover - GAP, Math.Min(w2 / ratios[1], w2 / ratios[2]));
                    if (hCover + h < MIN_HEIGHT)
                    {
                        double prevTotalHeight = hCover + h;
                        hCover = MIN_HEIGHT * (hCover / prevTotalHeight);
                        h = MIN_HEIGHT * (h / prevTotalHeight);
                    }
                    result.width = MAX_WIDTH;
                    result.height = (int)Math.Round(hCover + h + GAP);
                    result.columnSizes = new int[] { (int)Math.Round(w2), (int)(MAX_WIDTH - Math.Round(w2)) };
                    result.rowSizes = new int[] { (int)Math.Round(hCover), (int)Math.Round(h) };
                    result.tiles = new Tile[]{
                        new Tile(2, 1, 0, 0),
                        new Tile(1, 1, 0, 1),
                        new Tile(1, 1, 1, 1)
                    };
                }
                else
                { // 2nd and 3rd photos are on the right part
                    double height = Math.Min(MAX_HEIGHT, MAX_WIDTH * 0.66f / avgRatio);
                    double wCover = Math.Min(height * ratios[0], (MAX_WIDTH - GAP) * 0.66);
                    double h1 = (ratios[1] * (height - GAP) / (ratios[2] + ratios[1]));
                    double h0 = (height - h1 - GAP);
                    double w = Math.Min(MAX_WIDTH - wCover - GAP, Math.Min(h1 * ratios[2], h0 * ratios[1]));
                    result.width = (int)Math.Round(wCover + w + GAP);
                    result.height = (int)Math.Round(height);
                    result.columnSizes = new int[] { (int)Math.Round(wCover), (int)Math.Round(w) };
                    result.rowSizes = new int[] { (int)Math.Round(h0), (int)Math.Round(h1) };
                    result.tiles = new Tile[]{
                        new Tile(1, 2, 0, 0),
                        new Tile(1, 1, 1, 0),
                        new Tile(1, 1, 1, 1)
                    };
                }
            }
            else if (cnt == 4)
            {
                if ((ratios[0] > 1.2 * maxRatio || avgRatio > 1.5 * maxRatio) || allAreWide)
                { // 2nd, 3rd and 4th photos are on the next line
                    double hCover = Math.Min(MAX_WIDTH / ratios[0], (MAX_HEIGHT - GAP) * 0.66);
                    double h = (MAX_WIDTH - 2 * GAP) / (ratios[1] + ratios[2] + ratios[3]);
                    double w0 = h * ratios[1];
                    double w1 = h * ratios[2];
                    double w2 = h * ratios[3];
                    h = Math.Min(MAX_HEIGHT - hCover - GAP, h);
                    if (hCover + h < MIN_HEIGHT)
                    {
                        double prevTotalHeight = hCover + h;
                        hCover = MIN_HEIGHT * (hCover / prevTotalHeight);
                        h = MIN_HEIGHT * (h / prevTotalHeight);
                    }
                    result.width = MAX_WIDTH;
                    result.height = (int)Math.Round(hCover + h + GAP);
                    result.columnSizes = new int[] { (int)Math.Round(w0), (int)Math.Round(w1), (int)(MAX_WIDTH - Math.Round(w0) - Math.Round(w1)) };
                    result.rowSizes = new int[] { (int)Math.Round(hCover), (int)Math.Round(h) };
                    result.tiles = new Tile[]{
                        new Tile(3, 1, 0, 0),
                        new Tile(1, 1, 0, 1),
                        new Tile(1, 1, 1, 1),
                        new Tile(1, 1, 2, 1),
                    };
                }
                else
                { // 2nd, 3rd and 4th photos are on the right part
                    double height = Math.Min(MAX_HEIGHT, MAX_WIDTH * 0.66 / avgRatio);
                    double wCover = Math.Min(height * ratios[0], (MAX_WIDTH - GAP) * 0.66);
                    double w = (height - 2 * GAP) / (1 / ratios[1] + 1 / ratios[2] + 1 / ratios[3]);
                    double h0 = w / ratios[1];
                    double h1 = w / ratios[2];
                    double h2 = w / ratios[3] + GAP;
                    w = Math.Min(MAX_WIDTH - wCover - GAP, w);
                    result.width = (int)Math.Round(wCover + GAP + w);
                    result.height = (int)Math.Round(height);
                    result.columnSizes = new int[] { (int)Math.Round(wCover), (int)Math.Round(w) };
                    result.rowSizes = new int[] { (int)Math.Round(h0), (int)Math.Round(h1), (int)Math.Round(h2) };
                    result.tiles = new Tile[]{
                        new Tile(1, 3, 0, 0),
                        new Tile(1, 1, 1, 0),
                        new Tile(1, 1, 1, 1),
                        new Tile(1, 1, 1, 2),
                    };
                }
            }
            else
            {
                List<double> ratiosCropped = new List<double>();
                if (avgRatio > 1.1)
                {
                    foreach (double ratio in ratios)
                    {
                        ratiosCropped.Add(Math.Max(1.0, ratio));
                    }
                }
                else
                {
                    foreach (double ratio in ratios)
                    {
                        ratiosCropped.Add(Math.Min(1.0, ratio));
                    }
                }

                Dictionary<int[], double[]> tries = new Dictionary<int[], double[]>();

                // One line
                int firstLine, secondLine;
                tries.Add(new int[] { cnt }, new double[] { CalculateMultiThumbsHeight(ratiosCropped, MAX_WIDTH, GAP) });

                // Two lines
                for (firstLine = 1; firstLine <= cnt - 1; firstLine++)
                {
                    tries.Add(new int[] { firstLine, cnt - firstLine }, new double[]{
                                CalculateMultiThumbsHeight(ratiosCropped.GetRange(0, firstLine), MAX_WIDTH, GAP),
                                CalculateMultiThumbsHeight(ratiosCropped.GetRange(firstLine, ratiosCropped.Count - firstLine), MAX_WIDTH, GAP)
                        }
                    );
                }

                // Three lines
                for (firstLine = 1; firstLine <= cnt - 2; firstLine++)
                {
                    for (secondLine = 1; secondLine <= cnt - firstLine - 1; secondLine++)
                    {
                        tries.Add(new int[] { firstLine, secondLine, cnt - firstLine - secondLine }, new double[]{
                                    CalculateMultiThumbsHeight(ratiosCropped.GetRange(0, firstLine), MAX_WIDTH, GAP),
                                    CalculateMultiThumbsHeight(ratiosCropped.GetRange(firstLine, secondLine), MAX_WIDTH, GAP),
                                    CalculateMultiThumbsHeight(ratiosCropped.GetRange(firstLine+secondLine, ratiosCropped.Count - (firstLine+secondLine)), MAX_WIDTH, GAP)
                            }
                        );
                    }
                }

                // Looking for minimum difference between thumbs block height and maxHeight (may probably be little over)
                int realMaxHeight = Math.Min(MAX_HEIGHT, MAX_WIDTH);
                int[] optConf = null;
                double optDiff = 0;
                foreach (int[] conf in tries.Keys)
                {
                    double[] heights = tries[conf];
                    double confH = GAP * (heights.Length - 1);
                    foreach (double h in heights) confH += h;
                    double confDiff = Math.Abs(confH - realMaxHeight);
                    if (conf.Length > 1)
                    {
                        if (conf[0] > conf[1] || conf.Length > 2 && conf[1] > conf[2])
                        {
                            confDiff *= 1.1;
                        }
                    }
                    if (optConf == null || confDiff < optDiff)
                    {
                        optConf = conf;
                        optDiff = confDiff;
                    }
                }

                Queue<ISizedAttachment> thumbsRemain = new Queue<ISizedAttachment>(thumbs);
                Queue<double> ratiosRemain = new Queue<double>(ratiosCropped);
                double[] optHeights = tries[optConf];
                int k = 0;

                result.width = MAX_WIDTH;
                result.rowSizes = new int[optHeights.Length];
                result.tiles = new Tile[thumbs.Count];
                double totalHeight = 0f;
                List<int> gridLineOffsets = new List<int>();
                List<List<Tile>> rowTiles = new List<List<Tile>>(optHeights.Length);

                for (int i = 0; i < optConf.Length; i++)
                {
                    int lineChunksNum = optConf[i];
                    List<ISizedAttachment> lineThumbs = new List<ISizedAttachment>();
                    for (int j = 0; j < lineChunksNum; j++) lineThumbs.Add(thumbsRemain.Dequeue());
                    double lineHeight = optHeights[i];
                    totalHeight += lineHeight;
                    result.rowSizes[i] = (int)Math.Round(lineHeight);
                    int totalWidth = 0;
                    List<Tile> row = new List<Tile>();
                    for (int j = 0; j < lineThumbs.Count; j++)
                    {
                        double thumb_ratio = ratiosRemain.Dequeue();
                        double w = j == lineThumbs.Count - 1 ? (MAX_WIDTH - totalWidth) : (thumb_ratio * lineHeight);
                        totalWidth += (int)Math.Round(w);
                        if (j < lineThumbs.Count - 1 && !gridLineOffsets.Contains(totalWidth))
                            gridLineOffsets.Add(totalWidth);
                        Tile tile = new Tile(1, 1, 0, i, (int)Math.Round(w));
                        result.tiles[k] = tile;
                        row.Add(tile);
                        k++;
                    }
                    rowTiles.Add(row);
                }
                gridLineOffsets.Sort();
                gridLineOffsets.Add(MAX_WIDTH);
                result.columnSizes = new int[gridLineOffsets.Count];
                result.columnSizes[0] = gridLineOffsets[0];
                for (int i = gridLineOffsets.Count - 1; i > 0; i--)
                {
                    result.columnSizes[i] = gridLineOffsets[i] - gridLineOffsets[i - 1];
                }

                foreach (List<Tile> row in rowTiles)
                {
                    int columnOffset = 0;
                    foreach (Tile tile in row)
                    {
                        int startColumn = columnOffset;
                        tile.startCol = startColumn;
                        int width = 0;
                        tile.colSpan = 0;
                        for (int i = startColumn; i < result.columnSizes.Length; i++)
                        {
                            width += result.columnSizes[i];
                            tile.colSpan++;
                            if (width == tile.width)
                            {
                                break;
                            }
                        }
                        columnOffset += tile.colSpan;
                    }
                }
                result.height = (int)Math.Round(totalHeight + GAP * (optHeights.Length - 1));
            }

            foreach (Tile tile in result.tiles)
            {
                tile.columnCount = result.columnSizes.Length;
                tile.rowCount = result.rowSizes.Length;
            }

            return result;
        }

        private static double CalculateMultiThumbsHeight(List<double> ratios, double width, double margin)
        {
            return (width - (ratios.Count - 1) * margin) / ratios.Sum();
        }
    }
}
