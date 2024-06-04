using System;

namespace ModBus.Utils
{
    /// <summary>
    /// Useful functions when working with registers over ModBus
    /// </summary>
    public static class RegisterUtils
    {
        /// <summary>
        /// Read a specific bit of a register
        /// </summary>
        /// <param name="register">The register to read</param>
        /// <param name="address">The address of the bit within the register</param>
        /// <returns></returns>
        public static bool ReadRegisterAtAddress(this ushort register, ushort address)
        {
            var value = register & (1 << address);
            return Convert.ToBoolean(value);
        }

        /// <summary>
        /// Sets the bit of a register to 1 at a specific address
        /// </summary>
        /// <param name="register">The register containing the bit to change</param>
        /// <param name="address">The address of the bit to set to 1</param>
        /// <returns>The full register with the bit changed to 1</returns>
        public static ushort SetBitToTrueAtAddress(this ushort register, ushort address)
            => (ushort)(register & 0xFFFF | 1 << address);

        /// <summary>
        /// Sets the bit of a register to 0 at a specific address
        /// </summary>
        /// <param name="register">The register containing the bit to change</param>
        /// <param name="address">The address of the bit to set to 0</param>
        /// <returns>The full register with the bit changed to 0</returns>
        public static ushort SetBitToFalseAtAddress(this ushort register, ushort address)
            => (ushort)(register & 0xFFFF & ~(1 << address));
    }
}
