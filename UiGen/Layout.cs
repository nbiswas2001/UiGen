using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UiGen
{

    class Layout
    {
        public Container root;

        private int ROW = 0;
        private int COL = 1;

        //----------------------------------------------------------------------
        // Scan for divisions in the block by looking to unbroken lines of boundary
        // markers (- for rows and | for cols) that are equal in length to the 
        // enclosing boundary marker i.e. the first and last row or column
        //   grid: The MxN charchter array
        //   rowScan: true means go from top to bottom, else left to right
        //   layout: the resulting Layout object 
        public void process(char[,] grid)
        {
            root = new Container();
            root.type = ContainerType.Root;
            ProcessRec(grid, ROW, root);
        }

        //----------------------------------------------------------------------
        // Recursive processor
        private void ProcessRec(char[,] grid, int scanBy, Container container)
        {
            //Unless this is the root container ...
            if (container.type != ContainerType.Root)
            {
                //if scanning rows then this container holds columns
                container.type = scanBy == ROW ? ContainerType.Column : ContainerType.Row;
            }
            
            PrintGrid("grid", grid);

            //find the lenght of border to scan. If scanBy is ROW, this is the
            //  no. of rows in grid, else for COL, the no. of columns
            var rowCount = grid.GetLength(0);

            //Check the rows for boundary lines. That splits up the space.
            var startRow = 0;
            var hasDivider = false;
            var isLeaf = true;
            for (var i = 0; i < rowCount; i++)
            {
                if (IsBoundaryLine(i, grid, scanBy)) //end of a compartment
                {
                    //if this is a Row then get col widths specified, if any
                    if (scanBy == ROW) AddColWidthMarkers(container, grid, i);

                    if (i > 0 && i < rowCount) //if not the first boundary line
                    {
                        Container child = new Container();
                        container.children.Add(child);
 
                        //PrintGrid("g", grid);
                        var sliced = Slice(grid, startRow + 1, i-1);
                        //PrintGrid("s", sliced);
                        var transposed = Transpose(sliced);
                        //PrintGrid("t", transposed);
                        var newScanBy = scanBy == ROW ? COL : ROW;
                        ProcessRec(transposed, newScanBy, child);
                        isLeaf = false;
                        startRow = i; //new start pos
                        
                        //if there is a boundary in the middle
                        if (i < rowCount - 1) hasDivider = true;
                    }
                }

            }

            //if last divider row is missing - add the last compartment
            if (hasDivider && !IsBoundaryLine(rowCount-1, grid, scanBy)) 
            {
                Container child = new Container();
                container.children.Add(child);
                var sliced = Slice(grid, startRow + 1, rowCount - 1);
                var transposed = Transpose(sliced);
                var newScanBy = scanBy == ROW ? COL : ROW;
                ProcessRec(transposed, newScanBy, child);
            }

            //if this a leaf node i.e. it is either pure content 
            if (isLeaf)
            { 
                var contentContainer = new Container();
                contentContainer.type = ContainerType.Content;
                //PrintGrid("grid", grid);
                var g = scanBy == ROW ? grid : Transpose(grid);
                PrintGrid("content", g);
                contentContainer.content = GetContent(g);
                container.children.Add(contentContainer);
            }
        }

 
        //------------------------------------------------------------
        private void AddColWidthMarkers(Container cont, char[,] grid, int boundaryRow)
        {
            //PrintGrid("grid", grid);
        }

        //------------------------------------------------------------
        private char[,] Slice(char[,] grid, int startRow, int endRow)
        {
            int cols = grid.GetLength(1);
            int k = 0;
            char[,] result = new char[endRow-startRow+1, cols];
            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = 0; j < cols; j++) result[k, j] = grid[i, j];
                k++;
            }
            return result;
        }

        //---------------------------------------
        private char[,] Transpose(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            char[,] result = new char[cols, rows];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[j, i] = grid[i, j];
            return result;
        }

        //------------------------------------------------------------
        private bool IsBoundaryLine(int rowNum, char[,] grid, int scanBy)
        {
            int cols = grid.GetLength(1);
            var result = true;
            for (int i = 0; i < cols; i++)
            {
                var c = grid[rowNum, i];
                if ((scanBy == ROW && c != '-' && !Char.IsDigit(c)) || (scanBy == COL && c != '|'))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        //----------------------------------
        private String GetContent(char[,] grid)
        {
            var result = new StringBuilder();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    if (grid[i, j] != ' ') result.Append(grid[i, j]);
            }
            return result.ToString();
        }

        //----------------------------------
        private void PrintGrid(string name, char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            Console.WriteLine("\n**********"+name+"**********\n");
            for (int i = 0; i < rows; i++) {
                Console.Write('\n');
                for (int j = 0; j < cols; j++)
                {
                    var c = grid[i, j] == ' ' ? '.' : grid[i, j];
                    Console.Write(c);
                }
            }
            
        }

    }

}
