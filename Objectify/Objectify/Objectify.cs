using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Objectify
{
    public class Objectify
    {
        public Assembly assembly;
        public Type typeClass;
        public object ClassInitialization;
        public Objectify(Assembly Container, String Class, Dictionary<String, object> parameters)
        {
            this.assembly = Container;
            typeClass = Container.GetType(Class);
            if (typeClass.IsClass)
            {
                ConstructorInfo[] constructors = typeClass.GetConstructors();
                foreach (ConstructorInfo constructor in constructors)
                {
                    if (constructor.Name == ".ctor")
                    {
                        ParameterInfo[] arguments = constructor.GetParameters();
                        if (arguments.Count() != parameters.Count())
                        {
                            throw new Exception("Argument count mismatch!");
                        }
                        object[] pass = new object[arguments.Count()];
                        int initial = 0;
                        foreach (ParameterInfo info in arguments)
                        {
                            pass[initial] = parameters[info.Name];
                            initial += 1;
                        }
                        ClassInitialization = Activator.CreateInstance(typeClass, pass);
                    }
                }
            }
            else
            {
                throw new Exception("Non-Class field requested!");
            }
        }

        public Objectify(Assembly Container, String Class, object Instance)
        {
            this.assembly = Container;
            this.typeClass = assembly.GetType(Class);
            this.ClassInitialization = Instance;
        }

        public object CallFunction(String FunctionName, Dictionary<String, object> parameters)
        {
            object ReturnValue = null;
            MethodInfo[] methods = typeClass.GetMethods();
            foreach (MethodInfo method in methods)
            {
                if (method.Name == FunctionName)
                {
                    ParameterInfo[] arguments = method.GetParameters();
                    if (arguments.Count() != parameters.Count())
                    {
                        throw new Exception("Argument Count Mismatch!");
                    }
                    object[] pass = new object[arguments.Count()];
                    int initial = 0;
                    foreach (ParameterInfo arg in arguments)
                    {
                        pass[initial] = parameters[arg.Name];
                        initial += 1;
                    }
                    ReturnValue = method.Invoke(ClassInitialization, pass);
                }
            }
            return ReturnValue;
        }

        public object CallVariable(String Variable, out Type VariableType) //Add Sub Stats
        {
            object SubInitialization = ClassInitialization;
            VariableType = null;
            try
            {
                List<String> TrueVariable = Variable.Split('.').ToList();
                foreach (String SubVariable in TrueVariable)
                {
                    PropertyInfo[] variables = SubInitialization.GetType().GetProperties();
                    foreach (PropertyInfo variable in variables)
                    {
                        if (variable.Name == SubVariable)
                        {
                            VariableType = variable.GetType();
                            SubInitialization = variable.GetValue(SubInitialization);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SubInitialization;
            throw new Exception("Variable not found!");
        }
    }
}
