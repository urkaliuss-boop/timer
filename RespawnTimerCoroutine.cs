namespace RespawnTimer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Exiled.API.Enums;
    using Exiled.API.Features;

    using MEC;

    using PlayerRoles;

    using Respawning;
    using Respawning.Waves;

    /// <summary>
    /// корутина таймера респавна, отображает хинт спектаторам
    /// </summary>
    public static class RespawnTimerCoroutine
    {
        /// <summary>
        /// основной цикл отображения таймера
        /// </summary>
        /// <returns>корутина MEC</returns>
        public static IEnumerator<float> RunTimerLoop()
        {
            while (Round.InProgress)
            {
                float interval = RespawnTimerPlugin.Instance.Config.HintInterval;

                yield return Timing.WaitForSeconds(interval);

                List<Player> spectators = Player.List
                    .Where(p => p.Role.Type == RoleTypeId.Spectator && !p.IsOverwatchEnabled)
                    .ToList();

                if (spectators.Count == 0)
                {
                    continue;
                }

                string hintMessage = BuildHintMessage();

                if (string.IsNullOrEmpty(hintMessage))
                {
                    continue;
                }

                float hintDuration = RespawnTimerPlugin.Instance.Config.HintDuration;

                foreach (Player spectator in spectators)
                {
                    spectator.ShowHint(hintMessage, hintDuration);
                }
            }
        }

        /// <summary>
        /// собирает текст хинта в зависимости от состояния респавна
        /// </summary>
        private static string BuildHintMessage()
        {
            SpawnableFaction nextFaction = Respawn.NextKnownSpawnableFaction;

            // команда уже выбрана — показываем фракцию
            if (nextFaction != SpawnableFaction.None)
            {
                string teamName = GetTeamDisplayName(nextFaction);
                string teamColor = GetTeamColor(nextFaction);

                return FormatEffectHint(teamName, teamColor);
            }

            // фаза ожидания ищем ближайший таймер
            float closestTime = float.MaxValue;

            foreach (SpawnableWaveBase wave in WaveManager.Waves)
            {
                if (wave is not TimeBasedWave timedWave)
                {
                    continue;
                }

                if (timedWave is IMiniWave)
                {
                    continue;
                }

                float timeLeft = timedWave.Timer.TimeLeft;

                if (timeLeft < closestTime)
                {
                    closestTime = timeLeft;
                }
            }

            if (closestTime <= 0f || closestTime >= 9999f)
            {
                return null;
            }

            int totalSeconds = (int)Math.Ceiling(closestTime);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            return FormatTimerHint(minutes, seconds);
        }

        /// <summary>
        /// хинт обратного отсчёта (команда ещё не выбрана)
        /// </summary>
        private static string FormatTimerHint(int minutes, int seconds)
        {
            string timeDisplay;

            if (minutes > 0)
            {
                timeDisplay = $"{minutes}:{seconds:D2}";
            }
            else
            {
                timeDisplay = $"{seconds}с";
            }

            return
                "\n\n\n\n\n\n\n\n\n\n" +
                "<size=26><color=#B0B0B0>━━━━━━━━━━━━━━━━━━━━━━━━</color></size>\n" +
                "<size=30><color=#E0E0E0>До прибытия подкрепления</color></size>\n" +
                $"<size=36><b><color=#FFD700>{timeDisplay}</color></b></size>\n" +
                "<size=26><color=#B0B0B0>━━━━━━━━━━━━━━━━━━━━━━━━</color></size>";
        }

        /// <summary>
        /// хинт при выбранной команде
        /// </summary>
        private static string FormatEffectHint(string teamName, string teamColor)
        {
            return
                "\n\n\n\n\n\n\n\n\n\n" +
                "<size=26><color=#B0B0B0>━━━━━━━━━━━━━━━━━━━━━━━━</color></size>\n" +
                $"<size=32><b><color={teamColor}>Прибывает: {teamName}</color></b></size>\n" +
                "<size=26><color=#B0B0B0>━━━━━━━━━━━━━━━━━━━━━━━━</color></size>";
        }

        /// <summary>
        /// возвращает имя команды по фракции
        /// </summary>
        private static string GetTeamDisplayName(SpawnableFaction faction)
        {
            if (faction == SpawnableFaction.NtfWave || faction == SpawnableFaction.NtfMiniWave)
            {
                return "МОГ";
            }

            if (faction == SpawnableFaction.ChaosWave || faction == SpawnableFaction.ChaosMiniWave)
            {
                return "ПХ";
            }

            return "Неизвестно";
        }

        /// <summary>
        /// возвращает hex-цвет для фракции
        /// </summary>
        private static string GetTeamColor(SpawnableFaction faction)
        {
            if (faction == SpawnableFaction.NtfWave || faction == SpawnableFaction.NtfMiniWave)
            {
                return "#5B9BD5";
            }

            if (faction == SpawnableFaction.ChaosWave || faction == SpawnableFaction.ChaosMiniWave)
            {
                return "#2ECC40";
            }

            return "#FFFFFF";
        }
    }
}
