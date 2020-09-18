namespace MagicDart.Mvvm
{
    public static class DesignTime
    {
        /// <summary>
        /// Indicate if app is in design time (usefull to mock viewmodel data)
        /// </summary>
        public static bool IsOn { get; set; } = true;
    }
}
