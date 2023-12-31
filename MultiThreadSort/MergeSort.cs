


using System;
using System.Collections;
using System.Text;
using System.Threading;


namespace MultiThreadSort
{
    public class MergeSort
    {
        #region Helper Functions [TASK 1]
        public static object Params2Object(int[] A, int s, int e, int m, int node_idx)
        {
            #region [TASK 1.1]
            //TODO: Encapsulate the given params into a single ArrayList object, then return it
            ArrayList enteredParameters = new ArrayList();
            enteredParameters.Add(A);
            enteredParameters.Add(s);
            enteredParameters.Add(e);
            enteredParameters.Add(m);
            enteredParameters.Add(node_idx);
            return enteredParameters;
            #endregion
        }

        public static void Object2Params(object parameters, ref int[] A, ref int s, ref int e, ref int m, ref int node_idx)
        {
            #region [TASK 1.2]
            //TODO: Extract ALL params from the given ArrayList object "parameters", then store each of them in the corresponding "ref" variable 
            ArrayList paramList = (ArrayList)parameters;
            A = (int[])paramList[0];
            s = (int)paramList[1];
            e = (int)paramList[2];
            m = (int)paramList[3];
            node_idx = (int)paramList[4];
            #endregion
        }
        #endregion

        //DO NOT CHANGE THIS CODE
        #region Sequential Sort 

        public static void Sort(int[] array)
        {
            MSort(array, 1, array.Length);
        }

        private static void MSort(int[] A, int s, int e)
        {
            if (s >= e)
            {
                return;
            }

            int m = (s + e) / 2;

            MSort(A, s, m);

            MSort(A, m + 1, e);

            Merge(A, s, m, e);
        }

        private static void Merge(int[] A, int s, int m, int e)
        {
            int leftCapacity = m - s + 1;

            int rightCapacity = e - m;

            int leftIndex = 0;

            int rightIndex = 0;

            int[] Left = new int[leftCapacity];

            int[] Right = new int[rightCapacity];

            for (int i = 0; i < Left.Length; i++)
            {
                Left[i] = A[s + i - 1];
            }

            for (int j = 0; j < Right.Length; j++)
            {
                Right[j] = A[m + j];
            }

            for (int k = s; k <= e; k++)
            {
                if (leftIndex < leftCapacity && rightIndex < rightCapacity)
                {
                    if (Left[leftIndex] < Right[rightIndex])
                    {
                        A[k - 1] = Left[leftIndex++];
                    }
                    else
                    {
                        A[k - 1] = Right[rightIndex++];
                    }
                }
                else if (leftIndex < leftCapacity)
                {
                    A[k - 1] = Left[leftIndex++];
                }
                else
                {
                    A[k - 1] = Right[rightIndex++];
                }
            }
        }
        #endregion

        //TODO: Change this function to be MULTITHREADED
        //HINT: Remember to handle any dependency and/or critical section issues

        #region Multithreaded Sort [REMAINING TASKS]
        static int NumMergeSortThreads;

        #region Semaphores
        //TODO: Define any required semaphore here
        public static Semaphore semaphore1;
        public static Semaphore semaphore2;
        #endregion

        #region Threads
        //TODO: Define any required thread objects here
        public static Thread sortThread1;
        public static Thread sortThread2;

        public static Thread mergeThread1;


        #endregion

        #region Sort Function
        public static void SortMT(int[] array)
        {
            int s = 1;
            int e = array.Length;
            int m = (s + e) / 2;
            int node_idx = 0;

            NumMergeSortThreads = 2;               //TASK 2
            //NumMergeSortThreads = 4;              //TASK 3

            #region [TASK 2]
            if (NumMergeSortThreads == 2)       //TASK 2
            {
                /*TODO: 
                 * 1) Initialize any required semaphore
                 * 2) Create & Start TWO Merge Sort concurrent threads & ONE Merge thread
                 * 3) Use semaphores to handle any dependency or critical section
                 */

                semaphore1 = new Semaphore(0);
                semaphore2 = new Semaphore(0);

                object parameters1 = Params2Object(array, s, m, 0, 1);
                object parameters2 = Params2Object(array, m + 1, e, 0, 2);
                object parameters3 = Params2Object(array, s, e, m, 0);

                sortThread1 = new Thread(MSortMT);
                sortThread2 = new Thread(MSortMT);

                sortThread1.Start(parameters1);
                sortThread2.Start(parameters2);



                mergeThread1 = new Thread(MergeMT);
                mergeThread1.Start(parameters3);

                semaphore2.Wait();
            }
            #endregion

            #region [TASK 3]
            else if (NumMergeSortThreads == 4)   //TASK 3
            {
                /*TODO: 
                 * 1) Initialize any required semaphore
                 * 2) Create & Start FOUR Merge Sort concurrent threads & THREE Merge thread
                 * 3) Use semaphores to handle any dependency or critical section
                 */


            }

            #endregion

        }

        private static void MSortMT(object parameters)
        {
            #region Extract params from the given object 
            int[] A = null;
            int s = 0;
            int e = 0;
            int m = 0;
            int node_idx = 0;
            Object2Params(parameters, ref A, ref s, ref e, ref m, ref node_idx);
            #endregion


            MSort(A, s, e);

            #region [TASK 2] 
            if (NumMergeSortThreads == 2)       //TASK 2
            {
                //TODO: Use semaphores to handle any dependency or critical section
                semaphore1.Signal();
                semaphore1.Signal();
            }
            #endregion

            #region [TASK 3]
            else if (NumMergeSortThreads == 4)   //TASK 3
            {
                //TODO: Use semaphores to handle any dependency or critical section
            }
            #endregion            
        }

        private static void MergeMT(object parameters)
        {
            #region Extract params from the given object
            int[] A = null;
            int s = 0;
            int e = 0;
            int m = 0;
            int node_idx = 0;
            Object2Params(parameters, ref A, ref s, ref e, ref m, ref node_idx);
            #endregion

            #region [TASK 2]
            if (NumMergeSortThreads == 2)       //TASK 2
            {
                //TODO: Use semaphores to handle any dependency or critical section
                semaphore1.Wait();
                semaphore1.Wait();
                Merge(A, s, m, e);
                semaphore2.Signal();

            }
            #endregion

            #region [TASK 3]
            else if (NumMergeSortThreads == 4)   //TASK 3
            {
                //TODO: Use semaphores to handle any dependency or critical section
                Merge(A, s, m, e);
            }

            #endregion
        }
        #endregion

        #endregion

    }
}


