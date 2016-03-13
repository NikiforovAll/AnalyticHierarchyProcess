using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticHierarchyManager
{
    public class AnalyticHierarchyManager
    {
        public int NumExpert { get; set; }
        public int NumCriteria { get; set; }
        public int NumOptions { get; set; }
        // Матрицы парных сравнений альтернатив >> 
        public List<List<PairComparison>> OptionsList { get; set; }
        //Матрица парных сравнений для каждого из экспертов >>
        public List<PairComparison> PairComparisonList;

        public List<string> OptionsData { get; set; }
        public List<string> CriteriaData { get; set; }




        public AnalyticHierarchyManager()
        {
            OptionsList = new List<List<PairComparison>>();
            PairComparisonList = new List<PairComparison>();
            OptionsData = new List<string>();
            CriteriaData = new List<string>();
        }
        public void AddOption(List<PairComparison> options)
        {
            OptionsList.Add(options);
        }


        private double[,] CalculateSummaryTable(List<PairComparison> pairComparisons)
        {
            var dimension = pairComparisons[0].Matrix.GetLength(0);
            double[,] resultTable = new double[dimension, dimension];
            for (var i = 0; i < dimension; i++)
            {
                for (var j = 0; j < dimension; j++)
                {
                    resultTable[i, j] = 1;
                    for (var c = 0; c < pairComparisons.Count; c++)
                    {
                        resultTable[i, j] *= pairComparisons[c][i, j];
                    }
                    resultTable[i, j] = Math.Pow(resultTable[i, j], 1.0 / NumExpert);
                }
            }
            return resultTable;
        }

        private static double[] CalculateLocalPriority(double[,] summaryTable)
        {
            var dimension = summaryTable.GetLength(0);
            var betaLocal = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                betaLocal[i] = 1.0;
                for (int j = 0; j < dimension; j++)
                {
                    betaLocal[i] *= summaryTable[i, j];
                }
                betaLocal[i] = Math.Pow(betaLocal[i], 1.0 / dimension);
            }
            var betaSum = betaLocal.Sum();
            var localPriority = betaLocal.Select(x => x / betaSum).ToArray();
            return localPriority;
        }

        private void Validate(double[,] summaryTable, double[] localPriority)
        {

            var gammaSum = new double[NumCriteria];
            for (var i = 0; i < NumCriteria; i++)
            {
                for (var j = 0; j < NumCriteria; j++)
                {
                    gammaSum[i] += summaryTable[j, i];
                }
            }
            var lambdaMax = gammaSum.Select((x, i) => x * localPriority[i]).Sum();
            // Поиск индекса согласованности 
            var consistencyIndex = (lambdaMax - NumCriteria) / (NumCriteria - 1);
            var mathExpectationData = new[] { 0, 0, 0, 0.58, 0.9, 1.12, 1.24, 1.32, 1.41, 1.45 };
            bool newCycle = false;
            consistencyIndex = consistencyIndex / mathExpectationData[NumCriteria];
            switch (NumCriteria)
            {
                case 3:
                    newCycle = consistencyIndex > 0.05;
                    break;
                case 4:
                    newCycle = consistencyIndex > 0.08;
                    break;
                default:
                    newCycle = consistencyIndex > 0.2;
                    break;
            }
            if (newCycle)
            {
                throw new Exception("Try again...");
            }
        }

        public int Process()
        {
            //NumExpert = OptionsList[0].Count;
            //NumCriteria = PairComparisonList[0].Matrix.GetLength(0);
            //NumOptions = OptionsList[0][0].Matrix.GetLength(0);
            // Step3
            // калькуляция сводной таблицы 
            double[,] summaryTable = CalculateSummaryTable(PairComparisonList);
            //Локальные приоритеты критериев
            var localPriority = CalculateLocalPriority(summaryTable);
            //Step4
            Validate(summaryTable, localPriority);
            //Step6
            List<double[]> summaryLocalPriorityOptions = OptionsList.Select(x => CalculateLocalPriority(CalculateSummaryTable(x))).ToList();
            var result = summaryLocalPriorityOptions.Select((x, i) =>
                new
                {
                    Val = x.Select(
                        (el, j) => el * localPriority[j]).Sum(),
                    Index = i
                }).OrderByDescending(item => item.Val).FirstOrDefault()?.Index;
            
            return result ?? -1;
        }
    }
    // матрица парных сравнений 
    public class PairComparison
    {
        public double[,] Matrix { get; set; }

        public double this[int i, int j]
        {
            get { return Matrix[i, j]; }
            set { Matrix[i, j] = value; }
        }
        public PairComparison(int numCrit)
        {
            Matrix = new double[numCrit, numCrit];
            for (var i = 0; i < numCrit; i++)
            {
                Matrix[i, i] = 1;
            }
        }

        public void AddElement(int i, int j, double val)
        {
            Matrix[i, j] = val;
            Matrix[j, i] = 1.0 / val;
        }

    }
}
