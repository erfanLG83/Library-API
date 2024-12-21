using System.Security.Cryptography;

namespace Application.Common.Utilities;

public static class OTPCodeGenerator
{
    public static string GenerateCode(int length = 5)
    {
        char[] chars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

        return RandomNumberGenerator.GetString(chars, length);
    }
}
