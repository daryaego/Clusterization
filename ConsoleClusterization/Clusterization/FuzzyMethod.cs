﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClusterization.Clusterization
{
    public abstract class FuzzyMethod<T> : ClusteringMethod<T>
    {
        protected double[][] associationMatrix;
        protected double trustLevel;

        protected List<List<T>> prepareResult()
        {
            Console.WriteLine("Preparing the result");
            var result = new List<List<T>>();
            for (int i = 0; i < clustersCount; i++)
                result.Add(new List<T>());

            for (int i = 0; i < associationMatrix.Length; i++)
                for (int j = 0; j < associationMatrix[i].Length; j++)
                    if (associationMatrix[i][j] > trustLevel)
                        result[j].Add(set[i]);

            return result;
        }
    }
}
