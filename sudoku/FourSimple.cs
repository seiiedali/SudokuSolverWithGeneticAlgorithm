using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku {
    class FourSimple {
        public int[, ] firstState;
        public int[, ] answer;
        public int tryCounter;
        public long timePeriod;
        private int[] table;
        private int[] numberPool;
        private bool solved;
        public FourSimple () {
            table = new int[16];
            firstState = new int[4, 4];
            answer = new int[4, 4];
            numberPool = new int[] { 4, 4, 4, 4 };
            solved = false;
            tryCounter = 0;

        }
        public void getTheAnswer () {
            Console.WriteLine ("working on the solution of 4*4 sudoku table(depth first) ... ");
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
            if (cell == 16) {
                if (checkTable ())
                    solved = true;
                else
                    return;
            } else {
                for (int i = 0; i < 4; i++) {
                    if (checkPoolFor (i + 1) && !solved) {
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
            int[, ] temp = new int[4, 4];

            //map possible answer into 2D array
            int index = 0;
            int[] sumRow = new int[4];
            int[] sumColumn = new int[4];
            int[] sumRegion = new int[4];
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    temp[i, j] = table[index];
                    index++;
                }
            }
            //save the first state
            if (tryCounter == 0) {
                firstState = temp;
                sudokuPrint (firstState);
            }
            tryCounter++;
            // get the sum
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    sumRow[i] += temp[i, j];
                    sumColumn[j] += temp[i, j];
                    sumRegion[i] += temp[(i / 2) * 2 + j / 2, i * 2 % 4 + j % 2];
                }
            }
            //check each sum
            for (int i = 0; i < 4; i++) {
                if (sumRow[i] != 10 || sumColumn[i] != 10 || sumRegion[i] != 10)
                    return false;
            }
            //check row and column constraint
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    for (int k = j + 1; k < 4; k++) {
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
            int[] tempCheck = new int[4];
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    tempCheck[j] = temp[(i / 2) * 2 + j / 2, i * 2 % 4 + j % 2];

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
            for (int i = 0; i < 3; i++) {
                if (toCheck[i + 1] - toCheck[i] != 1)
                    return false;
            }
            return true;
        }
        private void sudokuPrint (int[, ] matrix) {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    Console.Write (matrix[i, j] + " ");
                }
                Console.WriteLine ();
            }
            Console.WriteLine ();
        }
    }
}