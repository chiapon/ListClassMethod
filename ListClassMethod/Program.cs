using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ListClassMethod {
    class Program {
        static void Main(string[] args) {
            ListDeclaredMethodWithClassName(args[0]);
        }

        public static void ListDeclaredMethodWithClassName(string testClassPath) {
            try {
                AssemblyName assamblyName = AssemblyName.GetAssemblyName(testClassPath);
                Console.WriteLine(assamblyName);

                var asm = Assembly.Load(assamblyName);
                //Regex regEx = new Regex(@".+\.TestCase.?\..*");
                //var testClassTypes = asm.GetTypes().Where( 
                //t => regEx.IsMatch(t.FullName)
                //);

                //foreach (var c in testClassTypes) {
                //    var methods = c.GetMethods();
                //    foreach (var m in methods) {
                //        Console.WriteLine(c.FullName + "." + m.Name);
                //    }
                //}

                var types = asm.GetTypes().Where(t => t.IsClass);
                foreach (var c in types) {
                    var methods = c.GetMethods();
                    foreach (var m in methods) {
                        if (m.GetBaseDefinition().DeclaringType != typeof(object))
                        Console.WriteLine(c.FullName + "." + m.Name);
                    }
                }
            } catch (ReflectionTypeLoadException ex) {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exLoader in ex.LoaderExceptions) {
                    sb.AppendLine(exLoader.Message);
                    FileNotFoundException exFileNotFound = exLoader as FileNotFoundException;
                    if (exFileNotFound != null) {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog)) {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errMessage = sb.ToString();
                Console.WriteLine(errMessage);
            }

        }
    }
}
