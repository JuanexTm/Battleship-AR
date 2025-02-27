using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

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
    private Dictionary<ulong, List<int>> posicionesBarcos = new Dictionary<ulong, List<int>>();

    public TextMeshProUGUI estadoTexto;

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
        ActualizarUIClientRpc();
    }

    // Método para cambiar de turno
    [ServerRpc(RequireOwnership = false)]
    public void CambiarTurnoServerRpc()
    {
        turnoJugador1.Value = (turnoJugador1.Value == true) ? false : true;
        ActualizarUIClientRpc();
    }

    // Método que los clientes pueden llamar para saber si es su turno
    public bool EsMiTurno(ulong clientId)
    {
        if(clientId == 0 && turnoJugador1.Value)
        {
            return true;
        }
        else if(clientId == 1 && !turnoJugador1.Value)
        {
            return true;
        }

        return false;


    }

    // Método para registrar las posiciones de los barcos de cada jugador
    [ServerRpc(RequireOwnership = false)]
    public void RegistrarPosicionesServerRpc(ulong playerId, int[] posiciones)
    {
        if (!posicionesBarcos.ContainsKey(playerId))
        {
            posicionesBarcos[playerId] = new List<int>();
        }
        posicionesBarcos[playerId].Clear();
        posicionesBarcos[playerId].AddRange(posiciones);
    }

    // Método para procesar un ataque
    [ServerRpc(RequireOwnership = false)]
    public void EnviarAtaqueServerRpc(ulong atacanteId, int posicion)
    {
        ulong oponenteId = (atacanteId == 0) ? 1UL : 0UL;
        bool impacto = posicionesBarcos.ContainsKey(oponenteId) && posicionesBarcos[oponenteId].Contains(posicion);

        NotificarResultadoAtaqueClientRpc(posicion, impacto);
    }

    // Notificar a todos los jugadores sobre el resultado del ataque
    [ClientRpc]
    public void NotificarResultadoAtaqueClientRpc(int posicion, bool impacto)
    {
        Debug.Log($"Ataque en {posicion}, Impacto: {impacto}");
    }

    // Actualizar el texto de turno en cada cliente
    [ClientRpc]
    private void ActualizarUIClientRpc()
    {

        ulong miId = NetworkManager.Singleton.LocalClientId;
        if (EsMiTurno(miId))
        {
            estadoTexto.text = "¡Tu turno! Ataca";

            Debug.Log("Actualizando turnos");
            corePlayer1.enTurno = turnoJugador1.Value;
            corePlayer2.enTurno = !turnoJugador1.Value;

            Debug.Log("EnTurno player1: " + corePlayer1.enTurno);
            Debug.Log("EnTurno player2: " + corePlayer2.enTurno);



        }
        else
        {
            estadoTexto.text = "Defensa. Espera el ataque del rival";
        }

        NotificarCambioServerRpc();




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
    private void NotificarCambioServerRpc()
    {
        corePlayer1.cambiosImportantes.Value = true;
        corePlayer2.cambiosImportantes.Value = true;

    }
}

