using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using WFBuilder;
using Expression = NCalc.Expression;

namespace UnitTestWFBuilder
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            string ValueText = "3*5";

            try
            {
                var exp = new NCalc.Expression(ValueText);
                var obj = exp.Evaluate();
                MessageBox.Show($"{obj}");


            }
            catch (Exception)
            {
                
            }

           

            
           
        }
    }
}
