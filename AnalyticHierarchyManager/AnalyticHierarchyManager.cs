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
        public List<string> CriteriaList { get; set; }
        //Матрица парных сравнений для каждого из экспертов
        public List<PairComparison> PairComparisonList;
        //Сводная таблица
        public double[,] SummaryTable { get; set; }




        public AnalyticHierarchyManager(int numCriteria)
        {
            SummaryTable = new double[numCriteria, numCriteria];
        }

        public string Process()
        {
            #region Step3
            // калькуляция сводной таблицы 
            for (var i = 0; i < NumCriteria; i++)
            {
                for (var j = 0; j < NumCriteria; j++)
                {
                    SummaryTable[i, j] = 1;
                    for (var c = 0; c < NumExpert; c++)
                    {
                        SummaryTable[i, j] *= PairComparisonList[c][i, j];
                    }
                    SummaryTable[i, j] = Math.Pow(SummaryTable[i, j], 1.0 / NumExpert);
                }

            }
            //Локальные приоритеты критериев
            double[] betaLocal = new double[NumCriteria];
            for (int i = 0; i < NumCriteria; i++)
            {
                betaLocal[i] = 1.0;
                for (int j = 0; j < NumCriteria; j++)
                {
                    betaLocal[i] *= SummaryTable[i, j];
                }
                betaLocal[i] = Math.Pow(betaLocal[i], 1.0 / NumCriteria);
            }
            var betaSum = betaLocal.Sum();
            var localPriority = betaLocal.Select(x => x / betaSum).ToList();

            #endregion
            #region Step4
            // Сумма элементов по столбцам сводной таблицы 
            var gammaSum = new double[NumCriteria];
            for (var i = 0; i < NumCriteria; i++)
            {
                for (var j = 0; j < NumCriteria; j++)
                {
                    gammaSum[i] += SummaryTable[j, i];
                }
            }
            var lambdaMax = gammaSum.Select((x, i) => x * localPriority[i]).Sum();
            #endregion
            return "";
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

    public class Expert
    {
        public string Name { get; set; }
    }

}
