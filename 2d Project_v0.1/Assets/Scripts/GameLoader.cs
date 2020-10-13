using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameLoad
{
    // Written by Lukas Sacher / Camo
    public class GameLoader : MonoBehaviour
    {

        //Has to be only executed at the start of the game
        void Awake()
        {
            Assembly a = Assembly.GetExecutingAssembly();

            MethodInfo[] methods = a.GetTypes()
                      .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                      .Where(m => m.GetCustomAttributes(typeof(ExecuteOnGameLoadAttribute), false).Length > 0)
                      .ToArray();

			foreach (MethodInfo m in methods)
			{
                m.Invoke(m.DeclaringType, null);
			}
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteOnGameLoadAttribute : Attribute
    {

    }
}