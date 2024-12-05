using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CommandDataBase 
{
    private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

    public bool HasCommands(string commandName) => database.ContainsKey(commandName);

    public void AddCommand(string commandName, Delegate command)
    {
        if (!database.ContainsKey(commandName))
        {
            database.Add(commandName, command);
        }
        else
        {
            Debug.Log($"Command already exists in the database '{commandName}'");
        }
    }

    public Delegate GetCommand(string commandName)
    {
        if (!database.ContainsKey(commandName))
        {
            Debug.Log($"Command already exists in the database '{commandName}'");
            return null;
        }
        return database[commandName];
    }
}
