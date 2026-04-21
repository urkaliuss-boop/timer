namespace RespawnTimer
{
    using Exiled.API.Features;

    using MEC;

    /// <summary>
    /// плагин таймера респавна, показывает хинт спектаторам.
    /// </summary>
    public class RespawnTimerPlugin : Plugin<RespawnTimerConfig>
    {
        private static RespawnTimerPlugin singleton = new();

        private CoroutineHandle timerCoroutine;
        private ServerEventHandler serverEventHandler;

        private RespawnTimerPlugin()
        {
        }

        /// <summary>
        /// Gets singleton экземпляр плагина.
        /// </summary>
        public static RespawnTimerPlugin Instance => singleton;

        /// <inheritdoc/>
        public override string Name => "RespawnTimer";

        /// <inheritdoc/>
        public override string Author => "noxiss";

        /// <inheritdoc/>
        public override string Prefix => "respawn_timer";

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            serverEventHandler = new ServerEventHandler();

            Exiled.Events.Handlers.Server.RoundStarted += serverEventHandler.OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += serverEventHandler.OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound += serverEventHandler.OnRestartingRound;
            Exiled.Events.Handlers.Server.WaitingForPlayers += serverEventHandler.OnWaitingForPlayers;

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= serverEventHandler.OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= serverEventHandler.OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound -= serverEventHandler.OnRestartingRound;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= serverEventHandler.OnWaitingForPlayers;

            StopTimerCoroutine();

            serverEventHandler = null;

            base.OnDisabled();
        }

        /// <summary>
        /// запускает корутину хинтов.
        /// </summary>
        public void StartTimerCoroutine()
        {
            StopTimerCoroutine();
            timerCoroutine = Timing.RunCoroutine(RespawnTimerCoroutine.RunTimerLoop());
        }

        /// <summary>
        /// останавливает корутину хинтов.
        /// </summary>
        public void StopTimerCoroutine()
        {
            if (timerCoroutine.IsRunning)
            {
                Timing.KillCoroutines(timerCoroutine);
            }
        }
    }
}
