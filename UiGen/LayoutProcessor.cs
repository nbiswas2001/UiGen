using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UiGen
{

    class LayoutProcessor
    {
        public const int ROW = 0;
        public const int COL = 1;

        //----------------------------------------------------------------------
        // Scan for divisions in the block by looking to unbroken lines of boundary
        // markers (- for rows and | for cols) that are equal in length to the 
        // enclosing boundary marker i.e. the first and last row or column
        //   grid: The MxN charchter array
        //   rowScan: true means go from top to bottom, else left to right
        //   layout: the resulting Layout object 
        public Container process(char[,] grid)
        {
            Container root = new Container();
            root.type = ContainerType.Root;
            ProcessRec(grid, ROW, root);
            return root;
        }

        //----------------------------------------------------------------------
        // Recursive processor
        private void ProcessRec(char[,] grid, int scanBy, Container container)
        {
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
                    if (i > 0 && i < rowCount) //if not the first boundary line
                    {
                        var childCont = container.CreateChild(scanBy);
                        if (childCont.type == ContainerType.Row)
                        {
                            childCont.colWidthMarkers = GetColWidthMarkers(grid, startRow);
                        }
                        else if(childCont.type == ContainerType.Column)
                        {
                            childCont.ResolveColumnWidth(i);
                        }
                        

                        var sliced = Slice(grid, startRow + 1, i-1);
                        var transposed = Transpose(sliced);
                        var newScanBy = scanBy == ROW ? COL : ROW;
                        ProcessRec(transposed, newScanBy, childCont);
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
                var childCont = container.CreateChild(scanBy);
                childCont.ResolveColumnWidth(startRow);

                var sliced = Slice(grid, startRow + 1, rowCount - 1);
                var transposed = Transpose(sliced);
                var newScanBy = scanBy == ROW ? COL : ROW;
                ProcessRec(transposed, newScanBy, childCont);
            }

            //if this a leaf node i.e. it is either pure content 
            if (isLeaf)
            { 
                var contentCont = container.CreateChild(-1); //-1 -> type=Content
                var g = scanBy == ROW ? grid : Transpose(grid);
                contentCont.contentId = GetContent(g);
            }
        }

        //------------------------------------------------------------
        private List<ColWidthMarker> GetColWidthMarkers(char[,] grid, int boundaryRow)
        {
            var g = Slice(grid, boundaryRow, boundaryRow);
            var result = new List<ColWidthMarker>();
            var l = grid.GetLength(1);
            StringBuilder num = null;
            int pos = 0;
            for (var i = 0; i < l; i++)
            {
                var c = grid[boundaryRow, i];
                if (Char.IsDigit(c))
                {
                    if (num == null)
                    {
                        pos = i;
                        num = new StringBuilder();
                    }
                    num.Append(c);
                }
                else
                {
                    if (num != null)
                    {
                        var cwm = new ColWidthMarker(int.Parse(num.ToString()), pos);
                        result.Add(cwm);
                        num = null;
                    }
                }
            }
            return result;
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
