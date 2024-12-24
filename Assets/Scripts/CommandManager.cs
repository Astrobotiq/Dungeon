using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    Stack<Command> CommandStack;
    public event Action onCommandActivated;
    public event Action onCommandDeactivated;

    public void AddCommand(Command command)
    {
        if (CommandStack == null)
            CommandStack = new Stack<Command>();

        if (CommandStack.Count == 0)
            onCommandActivated.Invoke();
            
        CommandStack.Push(command);
    }

    public void Undo()
    {
        Command command = CommandStack.Pop();
        command.Undo();
        
        if (!HasCommand())
            onCommandDeactivated.Invoke();
    }

    public bool HasCommand()
    {
        if (CommandStack == null || CommandStack.Count == 0)
            return false;
        
        return true;
    }

    public void ClearCommands()
    {
        CommandStack.Clear();
        CommandStack = new Stack<Command>();
    }
}

public class Command
{
    Player player;
    GameObject Grid;
    float offset;

    public Command( Player player, GameObject grid, float offset)
    {
        this.player = player;
        Grid = grid;
        this.offset = offset;
    }
    public void Undo()
    {
        PlayerManager.Instance.SetSelectedPlayer(player.gameObject);
        player.Undo(Grid,offset);
    }
}
