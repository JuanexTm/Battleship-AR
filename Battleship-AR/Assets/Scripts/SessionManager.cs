using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;
using TMPro;

public class SessionManager : MonoBehaviour
{
    public TextMeshProUGUI textoCodigo;
    public InputField joinCodeInput; // InputField para ingresar el código de sesión
    public GameObject[] aDesctivar;

    async void Start()
    {
        textoCodigo.text = "";
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async void StartSessionAsHost()
    {
        try
        {
            var options = new SessionOptions
            {
                MaxPlayers = 2
            }.WithRelayNetwork(); // Usa Relay Network

            var session = await MultiplayerService.Instance.CreateSessionAsync(options);
            Debug.Log($"Session {session.Id} created! Join code: {session.Code}");

            // Mostrar el código de sesión en la UI
            textoCodigo.text = $"Sesión: {session.Code}";
            DesactivarCanvas();
            
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create session: {e.Message}");
        }
    }

    public async void JoinSession()
    {
        try
        {
            string joinCode = joinCodeInput.text;
            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.LogError("Join code is empty!");
                return;
            }

            var session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);
            Debug.Log($"Joined session {session.Id} successfully!");
            DesactivarCanvas();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to join session: {e.Message}");
        }
    }

    void DesactivarCanvas()
    {
        foreach (var item in aDesctivar)
        {
            item.SetActive(false);
        }
    }

}
