using System.Threading.Tasks;
using UnityEngine;

public class NetworkManager : GameFrameworkComponent
{
    public InternetChecker InternetChecker { get; private set; }

    private async void Start()
    {
        InternetChecker = gameObject.GetOrAdd<InternetChecker>();
        var isOnline = await InternetChecker.CheckNetwork();
        Debug.Log($"Is online: {isOnline}");
    }
}