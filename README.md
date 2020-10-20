# Sudoku Solver with AI algorithms

 Solving 3\*3 and 9\*9 Sudoku table using:
- Simple search
- Simulated annealing
- Genetic algorithm

All methods and algorithm are created from scratch without the help of any pre-built module. This program is coded in C# and is developed with MS visual studio.

Procedure start from the `./Properties/Program.cs` and it will test following algorithms to solve sudoku table:
- `FourSimple.cs` which would try to solve the 4\*4 sudoku table by testing all the possible arrangement of the numbers
- `FourSA.cs` which would try to solve 4\*4 table using simulated annealing algorithm
- `FourGa` which would try to solve 4\*4 table using genetic algorithm
- `NineSimple.cs` which would try to solve the 9\*9 sudoku table by testing all the possible arrangement of the numbers
- `NineSA.cs` which would try to solve 9\*9 table using simulated annealing algorithm
- `NineGA` which would try to solve 9\*9 table using genetic algorithm
> Note that this program does not solve the table with pre entered numbers in the table but try to find the first possible arrangement of numbers into the empty table that satisfy the sudoku rules.
