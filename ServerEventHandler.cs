namespace RespawnTimer
{
    using Exiled.Events.EventArgs.Server;

    /// <summary>
    /// обработчик серверных событий для управления корутиной
    /// </summary>
    public class ServerEventHandler
    {
        /// <summary>
        /// ожидание игроков — останавливаем корутину
        /// </summary>
        public void OnWaitingForPlayers()
        {
            RespawnTimerPlugin.Instance.StopTimerCoroutine();
        }

        /// <summary>
        /// старт раунда — запускаем корутину
        /// </summary>
        public void OnRoundStarted()
        {
            RespawnTimerPlugin.Instance.StartTimerCoroutine();
        }

        /// <summary>
        /// конец раунда — останавливаем корутину
        /// </summary>
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            RespawnTimerPlugin.Instance.StopTimerCoroutine();
        }

        /// <summary>
        /// рестарт раунда  останавливаем корутину
        /// </summary>
        public void OnRestartingRound()
        {
            RespawnTimerPlugin.Instance.StopTimerCoroutine();
        }
    }
}
