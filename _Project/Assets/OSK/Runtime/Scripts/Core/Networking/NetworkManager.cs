using System.Threading.Tasks;
using UnityEngine;

public class NetworkManager : GameFrameworkComponent
{
    public InternetChecker InternetChecker { get; private set; }
    public bool IsOnline;
    private async void Start()
    {
        InternetChecker = gameObject.GetOrAdd<InternetChecker>();
        IsOnline = await InternetChecker.CheckNetwork();

        Debug.Log($"Is online: {IsOnline}");
    }
}