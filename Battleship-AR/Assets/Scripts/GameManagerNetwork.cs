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
    private NetworkVariable<int> turnoActual = new NetworkVariable<int>(0); // 0: Jugador 1, 1: Jugador 2

    // Posiciones ocupadas por los barcos de cada jugador
    private Dictionary<ulong, List<int>> posicionesBarcos = new Dictionary<ulong, List<int>>();

    public TextMeshProUGUI estadoTexto;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            turnoActual.Value = 0;
        }
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
        ActualizarUIClientRpc();
    }

    // Método para cambiar de turno
    [ServerRpc(RequireOwnership = false)]
    public void CambiarTurnoServerRpc()
    {
        turnoActual.Value = (turnoActual.Value == 0) ? 1 : 0;
        ActualizarUIClientRpc();
    }

    // Método que los clientes pueden llamar para saber si es su turno
    public bool EsMiTurno(ulong clientId)
    {
        return (turnoActual.Value == (int)clientId);
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
        }
        else
        {
            estadoTexto.text = "Defensa. Espera el ataque del rival";
        }
    }
}

