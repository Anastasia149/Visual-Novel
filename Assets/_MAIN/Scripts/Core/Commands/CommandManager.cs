using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

namespace COMMANDS
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager instance {get;private set;}
        private static Coroutine process = null;
        public static bool isRunningProcess => process != null;

        private CommandDataBase dataBase;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                dataBase = new CommandDataBase();

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] extensionTypes = assembly.GetTypes().Where(t=>t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

                foreach (Type extension in extensionTypes)
                {
                    MethodInfo extendMethod = extension.GetMethod("Extend");
                    extendMethod.Invoke(null,new object[] { dataBase });
                }
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public Coroutine Execute(string commandName, params string[] args)
        {
            Delegate command = dataBase.GetCommand(commandName);
            
            if (command == null)
            {
            return null;
            }

            return Startprocess(commandName, command, args);
            
        }

        private Coroutine Startprocess(string commandName, Delegate command, string[] args)
        {
            StopCurrentProcess();

            process = StartCoroutine(RunningProcess(command, args));

            return process;
        }

        private void StopCurrentProcess()
        {
            if (process != null)
            {
                StopCoroutine(process);
            }
            process=null;
        }

        private IEnumerator RunningProcess(Delegate command, string[] args)
        {
            yield return WaitingForProcessToComplete(command, args);
            
            process = null;
        }

        private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
            {
                command.DynamicInvoke();
            }
            else if (command is Action<string>)
            {
                command.DynamicInvoke(args[0]);
            }
            else if (command is Action<string>)
            {
                command.DynamicInvoke((object)args);
            }
            else if (command is Func<IEnumerator>)
            {
                yield return((Func<IEnumerator>)command)();
            }
            else if (command is Func<string,IEnumerator>)
            {
                yield return((Func<string,IEnumerator>)command)(args[0]);
            }
            else if (command is Func<string[],IEnumerator>)
            {
                yield return((Func<string[],IEnumerator>)command)(args);
            }
        }
    }
}

