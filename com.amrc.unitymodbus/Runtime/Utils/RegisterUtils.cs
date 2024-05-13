using System;

public static class RegisterUtils
{
    public static bool ReadRegisterAtAddress(this ushort register, ushort address)
    {
        var value = register & (1 << address);
        return Convert.ToBoolean(value);
    }

    public static ushort SetBitToTrueAtAddress(this ushort register, ushort address)
    {
        return (ushort)(register & 0xFFFF | 1 << address);
    }

    public static ushort SetBitToFalseAtAddress(this ushort register, ushort address)
    {
        return (ushort)(register & 0xFFFF & ~(1 << address));
    }
}
