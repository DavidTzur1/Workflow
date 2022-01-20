using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using WFBuilder;

namespace UnitTestWFBuilder
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //string path = "WFBuilder.Adapters";
            //string typeName = "Template";
            //var adapter = BaseAdapter.Create(path, typeName);

            //PropertyInfo[] properties = adapter.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            //foreach (var item in properties)
            //{
            //    Debug.WriteLine($"Property is {item.Name}");
            //}
            object str = "5";
            //object a = int.Parse(str);
            object a = str;
            MessageBox.Show($"{a.ToString()}");
           // Debug.WriteLine($"Property is {item.Name}");

        }
    }
}
