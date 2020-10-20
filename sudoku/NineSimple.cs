using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku {
    class NineSimple {
        public int[, ] firstState;
        public int[, ] answer;
        public int tryCounter;
        public long timePeriod;
        private int[] table;
        private int[] numberPool;
        private bool solved;
        public NineSimple () {
            table = new int[81];
            firstState = new int[9, 9];
            answer = new int[9, 9];
            numberPool = new int[] { 9, 9, 9, 9, 9, 9, 9, 9, 9 };
            solved = false;
            tryCounter = 0;

        }
        public void getTheAnswer () {
            Console.WriteLine ("working on the solution of 9*9 sudoku table(depth first) ... ");
            Console.WriteLine ("\n*******************\nSTARTING ITERATION\n*******************");
            Console.WriteLine ("starting with first state: \n");
            var watch = System.Diagnostics.Stopwatch.StartNew ();
            solve (0);
            Console.WriteLine ("\n*******************\nFOUND SOLUTION\n*******************\nTHE Answer is:\n");
            sudokuPrint (answer);
            Console.WriteLine ("\n\nIt took " + tryCounter + " try");
            watch.Stop ();
            timePeriod = watch.ElapsedMilliseconds;
            Console.WriteLine ("ended in " + timePeriod + " ms");
        }
        private void solve (int cell = 0) {
            if (cell == 81) {
                if (checkTable ())
                    solved = true;
                else
                    return;
            } else {
                for (int i = 0; i < 9; i++) {
                    if (forwardChecking (cell, i + 1) && checkPoolFor (i + 1) && !solved) {
                        //Console.WriteLine("does this happen");
                        //Console.ReadKey();
                        //Console.WriteLine("cell " + cell+1 + "got the value: " + (i+1));
                        table[cell] = i + 1;
                        solve (cell + 1);
                        addToPool (i + 1);
                    }
                }
                return;
            }
        }

        private bool checkPoolFor (int number) {
            if (numberPool[number - 1] != 0) {
                numberPool[number - 1]--;
                return true;
            } else return false;
        }
        private void addToPool (int number) {
            numberPool[number - 1]++;
        }
        private bool checkTable () {
            //define variables
            int[, ] temp = new int[9, 9];

            //map possible answer into 2D array
            int index = 0;
            int[] sumRow = new int[9];
            int[] sumColumn = new int[9];
            int[] sumRegion = new int[9];
            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    temp[i, j] = table[index];
                    index++;
                }
            }
            if (tryCounter % 1000000 == 0)
                sudokuPrint (temp);
            //save the first state
            if (tryCounter == 0) {
                firstState = temp;
                sudokuPrint (firstState);
            }
            //Console.ReadKey();
            tryCounter++;
            // get the sum
            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    sumRow[i] += temp[i, j];
                    sumColumn[j] += temp[i, j];
                    sumRegion[i] += temp[(i / 3) * 3 + j / 3, i * 3 % 9 + j % 3];
                }
            }
            //Console.WriteLine("going to the sum");
            //check each sum
            for (int i = 0; i < 9; i++) {
                if (sumRow[i] != 45 || sumColumn[i] != 45 || sumRegion[i] != 45)
                    return false;
            }
            //Console.WriteLine("sum was ok");
            //check row and column constraint
            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    for (int k = j + 1; k < 9; k++) {
                        //check row
                        if (temp[i, j] == temp[i, k])
                            return false;
                        //check column
                        if (temp[j, i] == temp[k, i])
                            return false;
                    }
                }
            }
            //check regions constraint
            int[] tempCheck = new int[9];
            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    tempCheck[j] = temp[(i / 3) * 3 + j / 3, i * 3 % 9 + j % 3];

                }
                //check each region
                if (!regionValidate (tempCheck))
                    return false;
            }
            //solution is correct
            answer = temp;
            return true;
        }
        private bool regionValidate (int[] toCheck) {
            Array.Sort (toCheck);
            for (int i = 0; i < 8; i++) {
                if (toCheck[i + 1] - toCheck[i] != 1)
                    return false;
            }
            return true;
        }
        private void sudokuPrint (int[, ] matrix) {
            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    Console.Write (matrix[i, j] + " ");
                }
                Console.WriteLine ();
            }
            Console.WriteLine ();
        }
        private bool forwardChecking (int index, int value) {
            int count = index % 9;
            if (count != 0) {
                for (int i = 1; i < count + 1; i++) {
                    if (table[index - i] == value)
                        return false;
                }
            }
            if (index > 8) {
                for (int i = index - 9; i >= 0; i -= 9) {
                    if (table[i] == value)
                        return false;
                }
            }
            return true;
        }
    }
}