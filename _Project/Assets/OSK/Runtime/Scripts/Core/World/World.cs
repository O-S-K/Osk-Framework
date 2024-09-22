using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ShutdownType : byte
{
    None = 0,
    Restart,
    Quit,
}

[DefaultExecutionOrder(-101)]
public partial class World : MonoBehaviour
{
    public static readonly GameFrameworkLinkedList<GameFrameworkComponent> SGameFrameworkComponents = new();

    public static T GetFrameworkComponent<T>() where T : GameFrameworkComponent
    {
        return (T)Get(typeof(T));
    }
 
    private static GameFrameworkComponent Get(Type type)
    {
        var current = SGameFrameworkComponents.First;
        while (current != null)
        {
            if (current.Value.GetType() == type)
            {
                return current.Value;
            }

            current = current.Next;
        }

        return null;
    }

    public static GameFrameworkComponent Get(string typeName)
    {
        var current = SGameFrameworkComponents.First;
        while (current != null)
        {
            Type type = current.Value.GetType();
            if (type.FullName == typeName || type.Name == typeName)
            {
                return current.Value;
            }

            current = current.Next;
        }

        return null;
    }

    public static void Shutdown(ShutdownType shutdownType)
    {
        Debug.Log($"Shutdown Game Framework ({shutdownType})...");

        SGameFrameworkComponents.Clear();

        if (shutdownType == ShutdownType.None)
        {
            return;
        }

        if (shutdownType == ShutdownType.Restart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (shutdownType == ShutdownType.Quit)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            return;
        }
    }

    internal static void Register(GameFrameworkComponent gameFrameworkComponent)
    {
        if (gameFrameworkComponent == null)
        {
            Debug.Log("Game Framework component is invalid.");
            return;
        }

        Type type = gameFrameworkComponent.GetType();

        var current = SGameFrameworkComponents.First;
        while (current != null)
        {
            if (current.Value.GetType() == type)
            {
                Debug.Log($"Game Framework component type {type.FullName} is already exist.");
                return;
            }

            current = current.Next;
        }

        SGameFrameworkComponents.AddLast(gameFrameworkComponent);
    }
}