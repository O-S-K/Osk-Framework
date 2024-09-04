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
    public static readonly GameFrameworkLinkedList<GameFrameworkComponent> s_GameFrameworkComponents =
        new GameFrameworkLinkedList<GameFrameworkComponent>();

    public static T GetFrameworkComponent<T>() where T : GameFrameworkComponent
    {
        return (T)Get(typeof(T));
    }

    public static GameFrameworkComponent Get(Type type)
    {
        LinkedListNode<GameFrameworkComponent> current = s_GameFrameworkComponents.First;
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
        LinkedListNode<GameFrameworkComponent> current = s_GameFrameworkComponents.First;
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

            s_GameFrameworkComponents.Clear();

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

        LinkedListNode<GameFrameworkComponent> current = s_GameFrameworkComponents.First;
        while (current != null)
        {
            if (current.Value.GetType() == type)
            {
                Debug.Log($"Game Framework component type {type.FullName} is already exist.");
                return;
            }

            current = current.Next;
        }

        s_GameFrameworkComponents.AddLast(gameFrameworkComponent);
    }
}