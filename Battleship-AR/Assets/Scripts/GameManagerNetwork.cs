using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using System.Collections;



public class GameManagerNetwork : NetworkBehaviour
{
    public static GameManagerNetwork Instance;

    // Variable para saber si los jugadores están listos
    private NetworkVariable<bool> jugador1Listo = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> jugador2Listo = new NetworkVariable<bool>(false);

    // Control de turnos
    public NetworkVariable<bool> turnoJugador1 = new NetworkVariable<bool>(
        true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // Partida iniciada
    public NetworkVariable<bool> partidaIniciada = new NetworkVariable<bool>(
        true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // Posiciones ocupadas por los barcos de cada jugador
    public List<int> posicionesBarcosJugador1 = new List<int>();
    public List<int> posicionesBarcosJugador2 = new List<int>();


    public NetworkVariable<FixedString128Bytes> textoPlayer1 = new NetworkVariable<FixedString128Bytes>();

    public NetworkVariable<FixedString128Bytes> textoPlayer2 = new NetworkVariable<FixedString128Bytes>();

    public NetworkVariable<int> casillaAtacadaJugador1Int = new NetworkVariable<int>(
     0, // Valor inicial
     NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); // Solo el servidor puede escribir

    public NetworkVariable<int> casillaAtacadaJugador2Int = new NetworkVariable<int>(
    0, // Valor inicial
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); // Solo el servidor puede escribir


    public NetworkVariable<int> golpesRecibidosJugador1 = new NetworkVariable<int>(
    -1, // Valor inicial
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); // Solo el servidor puede escribir

    public NetworkVariable<int> golpesRecibidosJugador2 = new NetworkVariable<int>(
    -1, // Valor inicial
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); // Solo el servidor puede escribir

    public NetworkVariable<int> erradosRecibidosJugador1 = new NetworkVariable<int>(
  -1, // Valor inicial
  NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); // Solo el servidor puede escribir

    public NetworkVariable<int> erradosRecibidosJugador2 = new NetworkVariable<int>(
  -1, // Valor inicial
  NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); // Solo el servidor puede escribir

    




    public Core corePlayer1, corePlayer2;



    private void Awake()
    {
        Instance = this;
        partidaIniciada.Value = false;

    }




    // Método para marcar que un jugador está listo
    [ServerRpc(RequireOwnership = false)]
    public void MarcarListoServerRpc(ulong playerId, bool estado)
    {
        if (playerId == 0)
            jugador1Listo.Value = estado;
        else if (playerId == 1)
            jugador2Listo.Value = estado;

        // Si ambos jugadores están listos, comienza el juego
        if (jugador1Listo.Value && jugador2Listo.Value)
        {
            IniciarJuego();
        }
    }

    private void IniciarJuego()
    {
        Debug.Log("Ambos jugadores están listos. ¡Comienza la partida!");
        partidaIniciada.Value = true;
        ActualizarUIServerRpc();
    }

    // Método para cambiar de turno
    [ServerRpc(RequireOwnership = false)]
    public void CambiarTurnoServerRpc()
    {
        turnoJugador1.Value = (turnoJugador1.Value == true) ? false : true;
        ActualizarUIServerRpc();
    }

    // Método que los clientes pueden llamar para saber si es su turno
    public bool EsMiTurno(ulong clientId)
    {
        if (clientId == 0 && turnoJugador1.Value)
        {
            return true;
        }
        else if (clientId == 1 && !turnoJugador1.Value)
        {
            return true;
        }

        return false;


    }

    // Método para registrar las posiciones de los barcos de cada jugador
    [ServerRpc(RequireOwnership = false)]
    public void RegistrarPosicionesJugador1ServerRpc(int[] posicionesBarcos)
    {
        foreach (var item in posicionesBarcos)
        {
            posicionesBarcosJugador1.Add(item);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RegistrarPosicionesJugador2ServerRpc(int[] posicionesBarcos)
    {
        foreach (var item in posicionesBarcos)
        {
            posicionesBarcosJugador2.Add(item);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RevisarAtaqueServerRpc(int atacante, int casillaDeAtaque)
    {
        bool impacto = false;

        if (atacante == 1)
        {

            impacto = posicionesBarcosJugador2.Contains(casillaDeAtaque);
            casillaAtacadaJugador1Int.Value = impacto ? 1 : 0; // 1 si hubo impacto, 0 si falló
            if(impacto)
            {
                ActualizarDefensorServerRpc(atacante, casillaDeAtaque);
            }
            else
            {
                ActualizarErradorServerRpc(atacante, casillaDeAtaque);
            }
        }
        else if (atacante == 2)
        {
            impacto = posicionesBarcosJugador1.Contains(casillaDeAtaque);
            casillaAtacadaJugador2Int.Value = impacto ? 1 : 0;
            if (impacto)
            {
                ActualizarDefensorServerRpc(atacante, casillaDeAtaque);
            }
            else
            {
                ActualizarErradorServerRpc(atacante, casillaDeAtaque);
            }
        }


        if (!impacto)
        {
            CambiarTurnoServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActualizarDefensorServerRpc(int atacante, int casillaDeAtaque)
    {
        if(atacante == 1)
        {
            golpesRecibidosJugador2.Value = casillaDeAtaque;
        }
        else
        {
            golpesRecibidosJugador1.Value = casillaDeAtaque;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActualizarErradorServerRpc(int atacante, int casillaDeAtaque)
    {
        if (atacante == 1)
        {
            erradosRecibidosJugador2.Value = casillaDeAtaque;
        }
        else
        {
            erradosRecibidosJugador1.Value = casillaDeAtaque;
        }
    }




    // Notificar a todos los jugadores sobre el resultado del ataque
    [ClientRpc]
    public void NotificarResultadoAtaqueClientRpc(int posicion, bool impacto)
    {
        Debug.Log($"Ataque en {posicion}, Impacto: {impacto}");
    }

    // Actualizar el texto de turno en cada cliente
    [ServerRpc(RequireOwnership = false)]
    private void ActualizarUIServerRpc()
    {
        Debug.Log("Actualizando turnos");
        corePlayer1.enTurno = turnoJugador1.Value;
        corePlayer2.enTurno = !turnoJugador1.Value;

        if (turnoJugador1.Value)
        {
            textoPlayer1.Value = "¡Tu turno! Ataca";
            textoPlayer2.Value = "Defensa. Espera al ataque del rival";
        }
        else
        {
            textoPlayer1.Value = "Defensa. Espera al ataque del rival";
            textoPlayer2.Value = "¡Tu turno! Ataca";
        }

    }



    [ServerRpc(RequireOwnership = false)]
    public void RegistrarTableroServerRpc(ulong playerId, NetworkObjectReference tableroRef)
    {
        if (tableroRef.TryGet(out NetworkObject tableroObj))
        {
            Core core = tableroObj.GetComponent<Core>();

            if (playerId == NetworkManager.ServerClientId) // El host (server)
            {
                corePlayer1 = core;
            }
            else // Un cliente normal
            {
                corePlayer2 = core;
            }

            Debug.Log($"[GameManagerNetwork] Registrado tablero del jugador {playerId}: {core.name}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void WinStateServerRpc(int jugador)
    {

        if (jugador == 1)
        {
            textoPlayer1.Value = "¡VICTORIA!";
            textoPlayer2.Value = "DERROTA";
        }
        else
        {
            textoPlayer1.Value = "Derrota";
            textoPlayer2.Value = "Victoria";
        }





    }
}

