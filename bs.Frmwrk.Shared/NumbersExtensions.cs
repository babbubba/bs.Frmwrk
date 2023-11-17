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
            int quotient = Math.DivRem(x, y, out int remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }

        /// <summary>
        /// Divides two long values and return an long value rounded up.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static long DivideRoundingUp(long x, long y)
        {
            long quotient = Math.DivRem(x, y, out long remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }
    }
}