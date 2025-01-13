namespace OSK
{
    public static class CSVReader
    {
        /// <summary>
        /// Outputs the content of a 2D array to the Unity console.
        /// Can be useful for checking the importer.
        /// </summary>
        /// <param name="grid">2D string array to debug.</param>
        public static void DebugOutputGrid(string[,] grid)
        {
            string textOutput = string.Empty;
            
            for (int y = 0; y < grid.GetUpperBound(1); ++y)
            {	
                for (int x = 0; x < grid.GetUpperBound(0); ++x)
                    textOutput += grid[x,y] + "|";
                
                textOutput += "\n"; 
            }
            
            UnityEngine.Debug.Log(textOutput);
        }
     
        /// <summary>
        /// Splits a CSV text to a 2D string array.
        /// New lines are not handled correctly, so make sure no cell contains a new line.
        /// </summary>
        /// <param name="csvText">CSV text.</param>
        /// <returns>2D string array.</returns>
        public static string[,] SplitCSVGrid(string csvText)
        {
            string[] lines = csvText.Split("\n"[0]);
     
            // Finds the max width of row.
            int width = 0; 
            for (int i = 0; i < lines.Length; i++)
            {
                string[] row = SplitCSVLine(lines[i]);
                if (row.Length > width)
                    width = row.Length;
            }
     
            // Creates new 2D string array to output to.
            string[,] outputGrid = new string[width, lines.Length]; 
            for (int y = 0; y < lines.Length; y++)
            {
                string[] row = SplitCSVLine(lines[y]);
                for (int x = 0; x < row.Length; x++)
                {
                    outputGrid[x,y] = row[x];
                    outputGrid[x,y] = outputGrid[x,y].Replace("\"\"", "\""); // Replace double quotes.
                }
            }

            return outputGrid;
        }
     
        /// <summary>
        /// Splits a CSV line to a string array.
        /// </summary>
        /// <param name="csvLine">CSV line.</param>
        /// <returns>String array with line values.</returns>
        private static string[] SplitCSVLine(string csvLine)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            const string pattern = @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)";
            
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(csvLine, pattern, System.Text.RegularExpressions.RegexOptions.ExplicitCapture))
                list.Add(match.Groups[1].Value);

            return list.ToArray();
        }
    }
}