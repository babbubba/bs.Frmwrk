namespace bs.Frmwrk.Shared
{
    public static class NumbersExtensions
    {
        /// <summary>
        /// Divides two integer values and return an integer value rounded up.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static int DivideRoundingUp(int x, int y)
        {
            int remainder;
            int quotient = Math.DivRem(x, y, out remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }
    }
}