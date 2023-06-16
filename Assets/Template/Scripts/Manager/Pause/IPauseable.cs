namespace Template.Manager
{
    /// <summary>
    /// インターフェース
    /// </summary>
    public interface IPauseable
    {
        #region Properties

        bool IsPausing { get; }

        #endregion

        #region Methods

        void Pause();
        void Resume();

        #endregion
    }
}
