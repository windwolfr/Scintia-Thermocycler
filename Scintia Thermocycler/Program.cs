﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Scintia_Thermocycler
{
    static class Program
    {
        /// <summary>
        /// Program Constants
        /// </summary>
        /// 
        /// Steinhart Constants
        public static double c1 = 0.001125308852122, c2 = 0.000234711863267, c3 = 0.000000085663516;
        public static float tempRaiseConstant = 0.0F;
        public static float tempDropConstant = 0.0F;

        /// <summary>
        /// Global variables.
        /// -----------------
        /// Flags
        /// </summary>
        public static bool OKbtn = false;
        public static bool running = false;
        public static bool tempRead = false;
        public static bool readingBottom = false;
        /// <summary>
        /// GUI Helpers
        /// </summary>
        public static string stepTemperature = "0.0";
        public static string stepDuration = "0";
        public static string cycleName = "Main";
        public static string cycleReps = "0";
        /// <summary>
        /// GUI to Logic Helpers
        /// </summary>
        public static List<List<float>> cycleToPerform = new List<List<float>> { };
        /// <summary>
        /// Temperature Conversion Helpers
        /// </summary>
        public static string aux;
        public static string nuevo;
        public static int ADC;
        public static float voltres;
        public static float volttherm;
        public static float Rt;
        public static double Tc;
        public static double Tcelsius;
        /// <summary>
        /// Step Helpers
        /// </summary>
        public static float topTemp;
        public static float botTemp;
        
        /// <summary>
        /// Global functions.
        /// </summary>
        public static void TraverseTree(TreeNodeCollection nodes)
        {
            foreach (TreeNode child in nodes)
            {
                addToStep(child);
                TraverseTree(child.Nodes);
            }
        }

        public static void addToStep(TreeNode node)
        {
            if (node.Name == "Root")
            {
                return;
            }
            else if (node.Text.Contains("Cycle Name"))
            {
                int reps = 0;
                while(reps < (int)node.Tag){
                    foreach (TreeNode child in node.Nodes)
                    {
                        addToStep(child);
                    }
                    reps++;
                }
            }
            else if (node.Text.Contains("Temperature") && node.Parent.Name == "Root")
            {
                cycleToPerform.Add((List<float>)node.Tag);
            }
        }

        public static double inDataToTemp(String inputData)
        {
            Program.ADC = Convert.ToInt32(inputData);
            Program.voltres = ((ADC * 5) / 1023);
            Program.volttherm = 5 - voltres;
            Program.Rt = (10000 * ((5 / volttherm) - 1));
            Program.Tc = (1 / (c1 + (c2 * Math.Log(Rt)) + (c3 * (Math.Log(Rt) * Math.Log(Rt) * Math.Log(Rt)))));
            Program.Tcelsius = Tc - 273.15;
            return Program.Tcelsius;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
