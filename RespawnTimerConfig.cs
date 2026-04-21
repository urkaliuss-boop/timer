namespace RespawnTimer
{
    using System.ComponentModel;

    using Exiled.API.Interfaces;

    /// <summary>
    /// конфигурация плагина.
    /// </summary>
    public sealed class RespawnTimerConfig : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets or sets интервал обновления хинта в секундах.
        /// </summary>
        [Description("Интервал обновления хинта (в секундах). Рекомендуется 1.")]
        public float HintInterval { get; set; } = 1f;

        /// <summary>
        /// Gets or sets длительность показа хинта в секундах.
        /// </summary>
        [Description("Длительность показа хинта (в секундах).")]
        public float HintDuration { get; set; } = 1.5f;
    }
}
